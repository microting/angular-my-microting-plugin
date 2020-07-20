using System.Collections.Generic;

namespace MyMicroting.Pn.Infrastructure.Models.Droplets
{
    public class DropletsModel
    {
        public int Total { get; set; }
        public List<DropletModel> Droplets { get; set; }

        public DropletsModel()
        {
            Droplets = new List<DropletModel>();
        }
    }
}
