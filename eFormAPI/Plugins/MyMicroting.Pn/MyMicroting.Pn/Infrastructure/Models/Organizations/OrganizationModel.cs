using Microting.DigitalOceanBase.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;

namespace MyMicroting.Pn.Infrastructure.Models.Droplets
{
    public class OrganizationModel
    {
        public int Id { get; set; }
        public int DoUid { get; set; }
        public string CustomerNo { get; set; }
        public string PublicIpV4 { get; set; }
        public string PrivateIpV4 { get; set; }
        public string PublicIpV6 { get; set; }
        public string CurrentImageName { get; set; }
        public string RequestedImageName { get; set; }
        public int CurrentImageId { get; set; }
        public int RequestedImageId { get; set; }
        public string UserData { get; set; } 
        public bool MonitoringEnabled { get; set; }
        public bool IpV6Enabled { get; set; }
        public bool BackupsEnabled { get; set; }
        public Size Size { get; set; }
        public List<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public string WorkflowState { get; set; }
        public int Version { get; set; }
        public int MicrotingId { get; set; }
        public bool PaymentOverdue { get; set; }
        public string PaymentStatus { get; set; }
        public int UnitLicenseNumber { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
        public string Name { get; set; }
        public string DomainName { get; set; }
        public string Status { get; set; }
    }
}
