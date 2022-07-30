﻿/*
 * HttpService provides logic oparating with HttpClient to get data from third-party API
 */

using WebApp.Interfaces;

namespace WebApp.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public string GetResponse()
        {
            // Creating Http client for third-party API to get current rates for Bitcoin
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://bitpay.com/api/rates");
            var response = client.Send(request);

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
