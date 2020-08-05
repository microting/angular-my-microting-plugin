using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Abstractions
{
    public interface IOrganizationsService
    {
        Task<OperationDataResult<OrganizationsModel>> Index(OrganizationsRequestModel requestModel);
    }
}
