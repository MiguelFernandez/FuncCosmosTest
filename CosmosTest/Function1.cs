using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace CosmosTest
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var usConnectionPolicy = new ConnectionPolicy
                {

                    ConnectionMode = Microsoft.Azure.Documents.Client.ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp,
                    UseMultipleWriteLocations = true
                };

                var EndpointUrl = "https://mifernacosmos.documents.azure.com:443/";
                var AuthorizationKey = "hpomSqk1BMBOHQJ3dcq5OdmJwT6oCZXuxyTv0z4o01vxmgXE5B.......==";
                var databaseName = "ToDoList";
                var collectionName = "Items";
                var cosmosClient = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey, usConnectionPolicy);

                var response = await cosmosClient.ReadDocumentAsync(
                    UriFactory.CreateDocumentUri(databaseName, collectionName, "1"));

                var responseStatusCode = response.StatusCode.ToString();

                string name = req.Query["name"];

                var responseMessage = $"The response code is {responseStatusCode}";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {

                return new OkObjectResult($"There was an error in the function execution: {ex.Message}");
            }
           
        }
    }
}
