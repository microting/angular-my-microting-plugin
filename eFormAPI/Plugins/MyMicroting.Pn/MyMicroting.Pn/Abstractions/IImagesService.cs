using System.Threading.Tasks;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Infrastructure.Models.Images;

namespace MyMicroting.Pn.Abstractions
{
    public interface IImagesService
    {
        Task<OperationDataResult<ImagesModel>> Index(ImagesRequestModel requestModel);
    }
}