using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eForm.Infrastructure.Constants;
using Microting.eFormBaseCustomerBase.Infrastructure.Data;
using Microting.eFormBaseCustomerBase.Infrastructure.Data.Entities;
using Microting.MyMicrotingBase.Infrastructure.Data;
using Microting.MyMicrotingBase.Infrastructure.Data.Entities;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyMicroting.Pn.Infrastructure.Helpers
{
    public static class OrganizationHelper
    {
        public static async Task FetchFromApi(string token, MyMicrotingDbContext myMicrotingDbContext, CustomersPnDbAnySql customersDbContext, DigitalOceanDbContext doDbContext)
        {
            string endPoint = "https://basic.microting.com/";
            
            WebRequest request = WebRequest.Create($"{endPoint}/v2/organizations?token={token}");
            request.Method = "GET";

            var result = await PostToServer(request).ConfigureAwait(false);

            var parsedData = JRaw.Parse(result);
            
            List<OrganizationModel> organizationModels = new List<OrganizationModel>();
            
            var settings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
            foreach (JToken item in parsedData)
            {
                var customer = customersDbContext.Customers.SingleOrDefault(x => x.CustomerNo == item["CustomerNo"].ToString());
                if (customer == null)
                {
                    customer = new Customer()
                    {
                        CustomerNo = item["CustomerNo"].ToString(),
                        CompanyName = item["Name"].ToString(),
                        PaymentStatus = item["PaymentStatus"].ToString(),
                        PaymentOverdue = item["PaymentOverdue"].ToString() == "true",
                        WorkflowState = item["WorkflowState"].ToString()
                    };
                    await customer.Create(customersDbContext);
                }
                else
                {
                    customer.WorkflowState = item["WorkflowState"].ToString();
                    await customer.Update(customersDbContext);
                }

                var organization = myMicrotingDbContext.Organizations.SingleOrDefault(x => x.CustomerId == customer.Id);
                var droplet = doDbContext.Droplets.SingleOrDefault(x => x.Name == $"{customer.CustomerNo}-microting" && x.WorkflowState != Constants.WorkflowStates.Removed);
                if (organization == null)
                {
                    organization = new Organization()
                    {
                        CustomerId = customer.Id,
                        NumberOfLicenses = int.Parse(item["UnitLicenseNumber"].ToString()),
                        WorkflowState = item["WorkflowState"].ToString(),
                        DomainName = $"{customer.CustomerNo}.microting.com"
                    };
                    if (droplet != null)
                    {
                        organization.InstanceId = droplet.Id;
                    }
                    await organization.Create(myMicrotingDbContext);
                }
                else
                {
                    organization.WorkflowState = item["WorkflowState"].ToString();
                    if (droplet != null)
                    {
                        organization.InstanceId = droplet.Id;
                    }
                    await organization.Update(myMicrotingDbContext);
                }
                
                // var organization = myMicrotingDbContext.Organizations.SingleOrDefault(x =>
                //     x.CustomerId == int.Parse(item["CustomerNo"].ToString()));
                // if (organization == null)
                // {
                //     var org = JsonConvert.DeserializeObject<Organization>(item.ToString(), settings);
                //     org.Create(myMicrotingDbContext);
                // }
                //var org = JsonConvert.DeserializeObject<OrganizationModel>(item.ToString(), settings);
                //organizationModels.Add(org);
            }
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