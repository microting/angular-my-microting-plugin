using System.Collections.Generic;

namespace MyMicroting.Pn.Infrastructure.Models.Droplets
{
    public class OrganizationsModel
    {
        public int Total { get; set; }
        public List<OrganizationModel> Organizations { get; set; }

        public OrganizationsModel()
        {
            Organizations = new List<OrganizationModel>();
        }
    }
}
