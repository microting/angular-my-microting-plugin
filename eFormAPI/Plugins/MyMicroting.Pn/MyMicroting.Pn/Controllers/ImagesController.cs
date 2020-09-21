using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Images;

namespace MyMicroting.Pn.Controllers
{
    [Authorize]
    [Route("api/my-microting-pn/images")]
    public class ImagesController
    {
        private readonly IImagesService _imagesService;
        
        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet]
        public Task<OperationDataResult<ImagesModel>> Index(ImagesRequestModel requestModel)
        {
            return _imagesService.Index(requestModel);
        }
    }
}