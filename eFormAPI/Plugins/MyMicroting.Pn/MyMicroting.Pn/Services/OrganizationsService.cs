using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eForm.Infrastructure.Constants;
using Microting.eForm.Infrastructure.Extensions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using Microting.eFormBaseCustomerBase.Infrastructure.Data;
using Microting.MyMicrotingBase.Infrastructure.Data;
using Microting.MyMicrotingBase.Infrastructure.Data.Entities;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyMicroting.Pn.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly ILocalizationService localizationService;
        private readonly ILogger<OrganizationsService> logger;
        private readonly MyMicrotingDbContext myMicrotingDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DigitalOceanDbContext doDbContext;
        private readonly CustomersPnDbAnySql customersDbContext;

        public OrganizationsService(ILocalizationService localizationService, ILogger<OrganizationsService> logger,
            DigitalOceanDbContext doDbContext, MyMicrotingDbContext myMicrotingDbContext, CustomersPnDbAnySql customersDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.localizationService = localizationService;
            this.doDbContext = doDbContext;
            this.customersDbContext = customersDbContext;
            this.myMicrotingDbContext = myMicrotingDbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        public async Task<OperationDataResult<OrganizationsModel>> Index(OrganizationsRequestModel requestModel)
        {
            try
            {
                OrganizationsModel organizationsPnModel = new OrganizationsModel();

                if (new[] { "Status"}.Contains(requestModel.SortColumnName))
                {

                }
                IQueryable<Organization> organizationsQuery = myMicrotingDbContext.Organizations.AsQueryable();
                if (!string.IsNullOrEmpty(requestModel.SortColumnName))
                {
                    if (requestModel.IsSortDsc)
                    {
                        organizationsQuery = organizationsQuery
                            .CustomOrderByDescending(requestModel.SortColumnName);
                    }
                    else
                    {
                        organizationsQuery = organizationsQuery
                            .CustomOrderBy(requestModel.SortColumnName);
                    }
                }
                else
                {
                    organizationsQuery = myMicrotingDbContext.Organizations.OrderBy(x => x.Id);
                }

                if (!string.IsNullOrEmpty(requestModel.Name))
                {
                    organizationsQuery = organizationsQuery.Where(x =>
                        x.CustomerId.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.DomainName.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.Id.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.InstanceId.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.InstanceStatus.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.NextUpgrade.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.NumberOfLicenses.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.NumberOfLicensesUsed.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.ServiceEmail.ToString().Contains(requestModel.Name.ToLower()) ||
                        x.UpToDateStatus.ToString().Contains(requestModel.Name.ToLower()));
                }

                var organizationsResult = await organizationsQuery.Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Skip(requestModel.Offset)
                    .Take(requestModel.PageSize)
                    .ToListAsync();

                //List<OrganizationModel> organizations = mapper.Map<List<OrganizationModel>>(organizationsResult);
                organizationsPnModel.Total = await myMicrotingDbContext.Organizations.CountAsync(x => x.WorkflowState != Constants.WorkflowStates.Removed);
                //organizationsPnModel.Organizations = organizations;
                organizationsPnModel.Organizations = new List<OrganizationModel>();

                return new OperationDataResult<OrganizationsModel>(true, organizationsPnModel);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<OrganizationsModel>(false,
                    localizationService.GetString("ErrorObtainingOrganizationsInfo") + e.Message);
            }
        }
    }
}
