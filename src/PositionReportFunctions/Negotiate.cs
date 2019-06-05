using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;

namespace PositionReportFunctions
{
    public static class Negotiate
    {
        [FunctionName("negotiate")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options")]HttpRequestMessage req,
                                        [SignalRConnectionInfo(HubName = "aircraft")]SignalRConnectionInfo info)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
      
            response.Headers.Add("Access-Control-Allow-Credentials", "true");
            response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:57579");
            response.Headers.Add("Access-Control-Allow-Origins", "http://localhost:57579");
            response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            
            return response;
        }
    }
}
