using CarsIsland.API.Dto;
using CarsIsland.API.Mappers;
using CarsIsland.Core.Interfaces;
using CarsIsland.Infrastructure.Services.Integration.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarsIsland.API.Controllers
{
    [Authorize(Policy = "AccessAsUser")]
    [ApiController]
    [Route("api/[controller]")]
    public class CarReservationController : ControllerBase
    {

        private readonly ICarReservationService _carReservationService;
        private readonly ICarReservationMessagingService _carReservationMessagingService;

        public CarReservationController(ICarReservationService carReservationService,
            ICarReservationMessagingService carReservationMessagingService)
        {
            _carReservationService = carReservationService
                                     ?? throw new ArgumentNullException(nameof(carReservationService));
            _carReservationMessagingService = carReservationMessagingService
                                              ?? throw new ArgumentNullException(nameof(carReservationMessagingService));
        }

        /// <summary>
        /// Make reservation for a specific car
        /// </summary>
        /// <response code="201">Created customer reservation for a specific car</response>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid or car is already reserved</response>
        /// <response code="500">Oops! something went wrong</response>
        [ProducesResponseType(201)]
        [HttpPost()]
        public async Task<IActionResult> CreateReservationAsync([FromBody] CarReservationDto customerCarReservation)
        {
            var carReservation = CarReservationMapper.MapFromDto(customerCarReservation);
            var operationResult = await _carReservationService.MakeReservationAsync(carReservation);

            if (operationResult.CompletedWithSuccess)
            {
                await _carReservationMessagingService.PublishNewCarReservationMessageAsync(operationResult.Result);
                return StatusCode(StatusCodes.Status201Created);
            }

            else
            {
                return BadRequest(operationResult.OperationError);
            }
        }
    }
}
