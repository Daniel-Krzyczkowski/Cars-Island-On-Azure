using CarsIsland.API.Dto;
using CarsIsland.Core.Entities;
using CarsIsland.Core.Interfaces;
using CarsIsland.Infrastructure.Services.Storage.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CarsIsland.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IDataRepository<Enquiry> _enquiryRepository;
        private readonly IBlobStorageService _blobStorageService;

        public EnquiryController(IDataRepository<Enquiry> enquiryRepository,
                                 IBlobStorageService blobStorageService)
        {
            _enquiryRepository = enquiryRepository
                                 ?? throw new ArgumentNullException(nameof(enquiryRepository));
            _blobStorageService = blobStorageService
                                  ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        /// <summary>
        /// Add new customer enquiry
        /// </summary>
        /// <returns>
        /// Returns created customer enquiry
        /// </returns> 
        /// <response code="200">Created customer enquiry</response>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid</response>
        /// <response code="500">Oops! something went wrong</response>
        [ProducesResponseType(typeof(Enquiry), 200)]
        [HttpPost()]
        public async Task<IActionResult> AddNewEnquiryAsync([FromForm] CustomerEnquiryDto customerEnquiry)
        {
            string attachmentUrl = string.Empty;
            string fileTempPath = string.Empty;
            if (customerEnquiry.Attachment != null)
            {
                var fileName = $"{Guid.NewGuid()}-{customerEnquiry.Attachment.FileName}";
                fileTempPath = @$"{Path.GetTempPath()}{fileName}";
                using var stream = new FileStream(fileTempPath, FileMode.Create, FileAccess.ReadWrite);
                await customerEnquiry.Attachment.CopyToAsync(stream);
                attachmentUrl = await _blobStorageService.UploadBlobAsync(stream, fileName);
            }

            var enquiry = new Enquiry
            {
                Id = Guid.NewGuid().ToString(),
                Title = customerEnquiry.Title,
                Content = customerEnquiry.Content,
                CustomerContactEmail = customerEnquiry.CustomerContactEmail,
                AttachmentUrl = attachmentUrl
            };

            var createdEnquiry = await _enquiryRepository.AddAsync(enquiry);

            if (!string.IsNullOrEmpty(fileTempPath))
            {
                System.IO.File.Delete(fileTempPath);
            }

            return Ok(createdEnquiry);
        }
    }
}
