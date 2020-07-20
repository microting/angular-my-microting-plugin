using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Controllers
{
    [Authorize]
    [Route("api/my-microting-pn/droplets")]
    public class DropletsController : Controller
    {
        private readonly IDropletsService dropletsService;

        public DropletsController(IDropletsService dropletsService)
        {
            this.dropletsService = dropletsService;
        }

        [HttpGet]
        public async Task<OperationDataResult<DropletsModel>> Index(DropletsRequestModel requestModel)
        {
            return await dropletsService.Index(requestModel);
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<OperationDataResult<DropletsModel>> Fetch(int id, DropletsRequestModel requestModel)
        {
            return await dropletsService.Fetch(requestModel);
        }

        [HttpGet]
        [Route("upgrade/{dropletId}/{imageId}")]
        public async Task<OperationDataResult<DropletsModel>> Upgrade(int dropletId, int imageId, DropletsRequestModel requestModel)
        {
            return await dropletsService.Upgrade(dropletId, imageId, requestModel);
        }
    }
}
