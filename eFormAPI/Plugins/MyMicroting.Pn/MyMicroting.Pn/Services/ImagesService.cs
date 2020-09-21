using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.DigitalOceanBase.Infrastructure.Data;
using Microting.eFormApi.BasePn.Infrastructure.Models.API;
using MyMicroting.Pn.Abstractions;
using MyMicroting.Pn.Infrastructure.Models.Images;

namespace MyMicroting.Pn.Services
{
    public class ImagesService : IImagesService
    {
        private readonly DigitalOceanDbContext dbContext;

        public ImagesService(DigitalOceanDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<OperationDataResult<ImagesModel>> Index(ImagesRequestModel pnRequestModel)
        {
            var thelist = await dbContext.Images.OrderBy(x => x.Name).ToListAsync();
            
            ImagesModel imagesModel = new ImagesModel();
            imagesModel.Images = new List<ImageModel>();

            foreach (var image in thelist)
            {
                var imageModel = new ImageModel()
                {
                    Id = image.Id,
                    Name = image.Name
                };
                imagesModel.Images.Add(imageModel);
            }
            
            return new OperationDataResult<ImagesModel>(true, imagesModel);
        }
    }
}