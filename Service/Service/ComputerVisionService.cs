using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Service.IService;

namespace Service.Service
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly string _subscriptionKey = "4HkZosMnbrGCY3tnIX7KRUzAuqBxz00NiVxONB5DXDWMqhDIEmvqJQQJ99ALACi0881XJ3w3AAAFACOGGsuh";
        private readonly string _endpoint = "https://abcneewer.cognitiveservices.azure.com/";

        private ComputerVisionClient Authenticate()
        {
            return new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
            {
                Endpoint = _endpoint
            };
        }

        public async Task<string> ExtractTextFromImageAsync(string imagePath)
        {
            var client = Authenticate();

            using var stream = new FileStream(imagePath, FileMode.Open);
            var textHeaders = await client.ReadInStreamAsync(stream);
            string operationLocation = textHeaders.OperationLocation;

            string operationId = operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);

            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
                await Task.Delay(1000);
            } while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

            var extractedText = string.Empty;
            if (results.Status == OperationStatusCodes.Succeeded)
            {
                foreach (var page in results.AnalyzeResult.ReadResults)
                {
                    foreach (var line in page.Lines)
                    {
                        extractedText += line.Text + " ";
                    }
                }
            }
            Console.WriteLine("Extracted Text: " + extractedText);

            return extractedText.Trim();
        }
    }
}
