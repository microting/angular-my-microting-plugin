using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microting.MyMicrotingBase.Infrastructure.Data.Entities;
using Newtonsoft.Json.Linq;

namespace MyMicroting.Pn.Infrastructure.Helpers
{
    public static class OrganizationHelper
    {
        public static async Task FetchFromApi(string token)
        {
            string endPoint = "https://basic.microting.com/";
            
            WebRequest request = WebRequest.Create($"{endPoint}/v1/organizations?token={token}");
            request.Method = "GET";

            var result = await PostToServer(request).ConfigureAwait(false);

            var parsedData = JRaw.Parse(result);

            // sites site = JsonConvert.DeserializeObject<sites>(item.ToString(), settings);
        }
        
        private static async Task<string> PostToServer(WebRequest request)
        {
            Console.WriteLine($"[DBG] Http.PostToServer: Calling {request.RequestUri}");
            // Hack for ignoring certificate validation.
            
            ServicePointManager.ServerCertificateValidationCallback = Validator;

            WebResponse response = request.GetResponse();
            Stream dataResponseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataResponseStream);
            string responseFromServer = await reader.ReadToEndAsync();

            // Clean up the streams.
            try
            {
                reader.Close();
                dataResponseStream.Close();
                response.Close();
            }
            catch
            {

            }

            return responseFromServer;
        }
        
        private static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyErrors)
        {
            return true;
        }
    }
}