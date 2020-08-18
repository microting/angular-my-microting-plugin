using Microsoft.Extensions.Logging;
using Microting.eForm.Infrastructure.Constants;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.DigitalOceanBase.Infrastructure.Data.Entities;
using Microting.eFormApi.BasePn.Infrastructure.Extensions;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Droplets;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microting.DigitalOceanBase.Managers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MyMicroting.Pn.Services
{
    public class DropletsService : IDropletsService
    {
        private readonly ILocalizationService localizationService;
        private readonly ILogger<DropletsService> logger;
        private readonly DigitalOceanDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IDigitalOceanManager digitalOceanManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DropletsService(ILocalizationService localizationService, ILogger<DropletsService> logger,
            DigitalOceanDbContext dbContext, IMapper mapper, IDigitalOceanManager digitalOceanManager, IHttpContextAccessor httpContextAccessor)
        {
            this.localizationService = localizationService;
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.digitalOceanManager = digitalOceanManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationDataResult<DropletsModel>> Index(DropletsRequestModel pnRequestModel)
        {
            try
            {
                DropletsModel dropletsPnModel = new DropletsModel();

                IQueryable<Droplet> dropletsQuery = dbContext.Droplets.AsQueryable();
                if (!string.IsNullOrEmpty(pnRequestModel.SortColumnName))
                {
                    if (pnRequestModel.IsSortDsc)
                    {
                        dropletsQuery = dropletsQuery
                            .CustomOrderByDescending(pnRequestModel.SortColumnName);
                    }
                    else
                    {
                        dropletsQuery = dropletsQuery
                            .CustomOrderBy(pnRequestModel.SortColumnName);
                    }
                }
                else
                {
                    dropletsQuery = dbContext.Droplets.OrderBy(x => x.Id);
                }

                if (!string.IsNullOrEmpty(pnRequestModel.Name))
                {
                    dropletsQuery = dropletsQuery.Where(x =>
                        x.DoUid.ToString().Contains(pnRequestModel.Name.ToLower()) ||
                        x.CustomerNo.ToString().Contains(pnRequestModel.Name.ToLower()) ||
                        x.PrivateIpV4.ToLower().Contains(pnRequestModel.Name.ToLower()) ||
                        x.PublicIpV4.ToLower().Contains(pnRequestModel.Name.ToLower()) ||
                        x.PublicIpV6.ToLower().Contains(pnRequestModel.Name.ToLower()) ||
                        x.DropletTags.Select(t => t.Tag.Name).Any(t => t.Contains(pnRequestModel.Name)));
                }

                List<Droplet> dropletsResult = await  dropletsQuery.Where(x => x.WorkflowState != Constants.WorkflowStates.Removed)
                    .Skip(pnRequestModel.Offset)
                    .Take(pnRequestModel.PageSize)
                    .ToListAsync();

                List<DropletModel> thelist = new List<DropletModel>();

                // List<DropletModel> droplets = mapper.Map<List<DropletModel>>(dropletsResult);
                foreach (Droplet droplet in dropletsResult)
                {
                    DropletModel dropletModel = new DropletModel(droplet);
                    dropletModel.Tags = new List<string>();
                    var tags = await dbContext.DropletTag
                        .Where(x => x.DropletId == droplet.Id)
                        .Join(dbContext.Tags,
                        DropletTag => DropletTag.TagId, tag => tag.Id, ((tag, tag1) => new
                    {
                        Id = tag.Id,
                        Name = tag1.Name
                    })).ToListAsync();
                    foreach (var dropletTag in tags)
                    {
                        dropletModel.Tags.Add(dropletTag.Name);
                    }
                    thelist.Add(dropletModel);
                }
                dropletsPnModel.Total = await dbContext.Droplets.CountAsync(x => x.WorkflowState != Constants.WorkflowStates.Removed);
                dropletsPnModel.Droplets = thelist;

                return new OperationDataResult<DropletsModel>(true, dropletsPnModel);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<DropletsModel>(false,
                    localizationService.GetString("ErrorObtainingDropletsInfo") + e.Message);
            }
        }

        public async Task<OperationDataResult<DropletsModel>> Fetch(DropletsRequestModel pnRequestModel)
        {
            try
            {
                await digitalOceanManager.FetchDropletsAsync(UserId);

                return await Index(pnRequestModel);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<DropletsModel>(false,
                    localizationService.GetString("ErrorFetchingDropletsFromDO") + e.Message);
            }
        }

        public async Task<OperationDataResult<DropletsModel>> Upgrade(int dropletId, int imageId, DropletsRequestModel pnRequestModel)
        {
            try
            {
                await digitalOceanManager.RebuildDropletAsync(UserId, dropletId, imageId);

                return await Index(pnRequestModel);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                logger.LogError(e.Message);
                return new OperationDataResult<DropletsModel>(false,
                    localizationService.GetString("ErrorUpgradingDroplets") + e.Message);
            }
        }

        public int UserId
        {
            get
            {
                var value = httpContextAccessor?.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return value == null ? 0 : int.Parse(value);
            }
        }
    }
}
