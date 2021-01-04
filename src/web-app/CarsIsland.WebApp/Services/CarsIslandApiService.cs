using CarsIsland.WebApp.Common;
using CarsIsland.WebApp.Data;
using CarsIsland.WebApp.Extensions;
using CarsIsland.WebApp.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CarsIsland.WebApp.Services
{
    public class CarsIslandApiService : ICarsIslandApiService
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CarsIslandApiService(ITokenAcquisition tokenAcquisition,
                                    HttpClient httpClient,
                                    IConfiguration configuration)
        {
            _tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IReadOnlyCollection<Car>> GetAvailableCarsAsync()
        {
            var response = await _httpClient.GetAsync("api/car/all");
            return await response.ReadContentAs<List<Car>>();
        }

        public async Task SendEnquiryAsync(string attachmentFileName, CustomerEnquiry enquiry)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(enquiry.Title), "title");
            multipartContent.Add(new StringContent(enquiry.Content), "content");
            multipartContent.Add(new StringContent(enquiry.CustomerContactEmail), "customerContactEmail");
            if (enquiry.Attachment != null)
            {
                multipartContent.Add(new StreamContent(enquiry.Attachment), "Attachment", attachmentFileName);
            }
            await _httpClient.PostAsync("api/enquiry", multipartContent);
        }

        public async Task<OperationResponse> CreateNewCarReservationAsync(CarReservation carReservation)
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJson("api/carreservation", carReservation);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {

                    var operationError = await response.ReadContentAs<OperationError>();
                    return new OperationResponse().SetAsFailureResponse(operationError);
                }
            }

            return new OperationResponse();
        }

        private async Task GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string[] scopes = new[] { _configuration["CarsIslandApi:Scope"] };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
