using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace InvestorApi.ComponentTests.Internal
{
    internal sealed class TestContext
    {
        private const string BaseAddress = "/api/1.0";

        private readonly TestServer _server;

        private string _accessToken;
        private string _responseContent;

        public TestContext()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        public HttpResponseMessage LastResponse { get; private set; }

        public T ReadResponse<T>()
        {
            if (_responseContent == null)
            {
                _responseContent = LastResponse.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<T>(_responseContent);
        }

        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
        }

        public void Get(string resourceUri)
        {
            LastResponse = CreateRequest(resourceUri).GetAsync().Result;
        }

        public void Post<T>(string resourceUri, T body)
        {
            LastResponse = CreateRequest(resourceUri, body).SendAsync("POST").Result;
        }

        public void Put<T>(string resourceUri, T body)
        {
            LastResponse = CreateRequest(resourceUri, body).SendAsync("PUT").Result;
        }

        public void Delete(string resourceUri)
        {
            LastResponse = CreateRequest(resourceUri).SendAsync("DELETE").Result;
        }

        private RequestBuilder CreateRequest(string resourceUri)
        {
            _responseContent = null;

            var request = _server.CreateRequest(BaseAddress + resourceUri);

            if (!string.IsNullOrEmpty(_accessToken))
            {
                request = request.AddHeader("Authorization", "Bearer " + _accessToken);
            }

            return request;
        }

        private RequestBuilder CreateRequest<T>(string resourceUri, T body)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                "application/json");

            return CreateRequest(resourceUri).And(message => message.Content = content);
        }
    }
}
