using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Abstractions
{
    public interface IDropletsService
    {
        Task<OperationDataResult<DropletsModel>> Index(DropletsRequestModel pnRequestModel);

        Task<OperationDataResult<DropletsModel>> Fetch(DropletsRequestModel pnRequestModel);

        Task<OperationDataResult<DropletsModel>> Upgrade(int dropletId, int imageId, DropletsRequestModel pnRequestModel);
    }
}
