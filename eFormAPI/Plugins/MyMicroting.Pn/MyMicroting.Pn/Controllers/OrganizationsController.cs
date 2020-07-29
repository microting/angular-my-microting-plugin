using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Controllers
{
    [Authorize]
    [Route("api/my-microting-pn/organizations")]
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationsService organizationsService;

        public OrganizationsController(IOrganizationsService organizationsService)
        {
            this.organizationsService = organizationsService;
        }

        [HttpGet]
        public async Task<OperationDataResult<OrganizationsModel>> Index(OrganizationsRequestModel requestModel)
        {
            return await organizationsService.Index(requestModel);
        }

        //[HttpPost]
        //[Route("create")]
        //public async Task<OperationDataResult<DropletsModel>> Create(int id, DropletsRequestModel requestModel)
        //{
        //    return await organizationsService.Create(requestModel);
        //}

        //[HttpPost]
        //[Route("edit")]
        //public async Task<OperationDataResult<DropletsModel>> Edit(int id, DropletsRequestModel requestModel)
        //{
        //    return await organizationsService.Edit(requestModel);
        //}
        //[HttpDelete]
        //[Route("delete")]
        //public async Task<OperationDataResult<DropletsModel>> Delete(int id, DropletsRequestModel requestModel)
        //{
        //    return await organizationsService.Delete(requestModel);
        //}
    }
}
