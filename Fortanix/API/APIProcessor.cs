using Keyfactor.Extensions.Orchestrator.Fortanix.Models;
using Keyfactor.Logging;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.Orchestrator.Fortanix.API
{
    class APIProcessor
    {
        private const string API_URL = "https://apps.sdkms.fortanix.com";
        private string ApiKey { get; set; }
        private string BearerToken { get; set; }

        public async Task Initialize(string apiKey)
        {
            ILogger logger = LogHandler.GetClassLogger<APIProcessor>();
            logger.MethodEntry(LogLevel.Debug);

            ApiKey = apiKey;
            await SetBearerToken();

            logger.MethodExit(LogLevel.Debug);
        }


        #region Public Methods
        public async Task<List<SecurityObject>> GetCertificates()
        {
            ILogger logger = LogHandler.GetClassLogger<APIProcessor>();
            logger.MethodEntry(LogLevel.Debug);

            string rtnMessage = string.Empty;

            string RESOURCE = $"/crypto/v1/keys";
            RestRequest request = new RestRequest(RESOURCE, Method.Get);

            logger.MethodExit(LogLevel.Debug);

            return JsonConvert.DeserializeObject<List<SecurityObject>>(await SubmitRequest(request, BearerToken)).Where(p => p.Type.ToUpper() == "CERTIFICATE").ToList();
        }
        #endregion


        #region Private Methods
        private async Task SetBearerToken()
        {
            ILogger logger = LogHandler.GetClassLogger<APIProcessor>();
            logger.MethodEntry(LogLevel.Debug);

            string rtnMessage = string.Empty;

            string RESOURCE = $"/sys/v1/session/auth";
            RestRequest request = new RestRequest(RESOURCE, Method.Post);
            BearerToken = $"Bearer {JsonConvert.DeserializeObject<BearerToken>(await SubmitRequest(request, $"Basic {ApiKey}")).Token}";

            logger.MethodExit(LogLevel.Debug);
        }

        private async Task<string> SubmitRequest(RestRequest request, string auth)
        {
            ILogger logger = LogHandler.GetClassLogger<APIProcessor>();
            logger.MethodEntry(LogLevel.Debug);
            logger.LogTrace($"Request Resource: {request.Resource}");
            logger.LogTrace($"Request Method: {request.Method.ToString()}");
            if (request.Method != Method.Get)
            {
                StringBuilder body = new StringBuilder("Request Body: ");
                foreach(Parameter parameter in request.Parameters)
                {
                    body.Append($"{parameter.Name}={parameter.Value}");
                }
                logger.LogTrace(body.ToString());
            }

            RestResponse response;

            RestClient client = new RestClient(API_URL);
            request.AddHeader("Authorization", auth);

            try
            {
                response = await client.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                string exceptionMessage = FortanixException.FlattenExceptionMessages(ex, $"Error processing {request.Resource}");
                logger.LogError(exceptionMessage);
                throw new FortanixException(exceptionMessage);
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK &&
                response.StatusCode != System.Net.HttpStatusCode.Accepted &&
                response.StatusCode != System.Net.HttpStatusCode.Created &&
                response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                string errorMessage;

                try
                {
                    APIError error = JsonConvert.DeserializeObject<APIError>(response.Content);
                    errorMessage = $"{error.Message}";
                }
                catch (JsonReaderException ex)
                {
                    errorMessage = response.Content;
                }

                string exceptionMessage = $"Error processing {request.Resource}: {errorMessage}";
                logger.LogError(exceptionMessage);
                throw new FortanixException(exceptionMessage);
            }

            logger.LogTrace($"API Result: {response.Content}");
            logger.MethodExit(LogLevel.Debug);

            return response.Content;
        }
        #endregion
    }
}
