using Newtonsoft.Json;
using Salesforce.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandeskUMP.Models.Salesforce
{
    public class Account
    {
        [Key]
        [Display(Name = "Account ID")]
        [Createable(false), Updateable(false)]
        public string Id { get; set; }

        [Display(Name = "Deleted")]
        [Createable(false), Updateable(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "Master Record ID")]
        [Createable(false), Updateable(false)]
        public string MasterRecordId { get; set; }

        [Display(Name = "Account Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Account Type")]
        public string Type { get; set; }

        [Display(Name = "Parent Account ID")]
        public string ParentId { get; set; }

        [Display(Name = "Billing Street")]
        public string BillingStreet { get; set; }

        [Display(Name = "Billing City")]
        [StringLength(40)]
        public string BillingCity { get; set; }

        [Display(Name = "Billing State/Province")]
        [StringLength(80)]
        public string BillingState { get; set; }

        [Display(Name = "Billing Zip/Postal Code")]
        [StringLength(20)]
        public string BillingPostalCode { get; set; }

        [Display(Name = "Billing Country")]
        [StringLength(80)]
        public string BillingCountry { get; set; }

        [Display(Name = "Billing Latitude")]
        public double? BillingLatitude { get; set; }

        [Display(Name = "Billing Longitude")]
        public double? BillingLongitude { get; set; }

        [Display(Name = "Shipping Street")]
        public string ShippingStreet { get; set; }

        [Display(Name = "Shipping City")]
        [StringLength(40)]
        public string ShippingCity { get; set; }

        [Display(Name = "Shipping State/Province")]
        [StringLength(80)]
        public string ShippingState { get; set; }

        [Display(Name = "Shipping Zip/Postal Code")]
        [StringLength(20)]
        public string ShippingPostalCode { get; set; }

        [Display(Name = "Shipping Country")]
        [StringLength(80)]
        public string ShippingCountry { get; set; }

        [Display(Name = "Shipping Latitude")]
        public double? ShippingLatitude { get; set; }

        [Display(Name = "Shipping Longitude")]
        public double? ShippingLongitude { get; set; }

        [Display(Name = "Account Phone")]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Account Fax")]
        [Phone]
        public string Fax { get; set; }

        [Display(Name = "Account Number")]
        [StringLength(40)]
        public string AccountNumber { get; set; }

        [Url]
        public string Website { get; set; }

        [Display(Name = "Photo URL")]
        [Url]
        [Createable(false), Updateable(false)]
        public string PhotoUrl { get; set; }

        [Display(Name = "SIC Code")]
        [StringLength(20)]
        public string Sic { get; set; }

        public string Industry { get; set; }

        [Display(Name = "Annual Revenue")]
        public double? AnnualRevenue { get; set; }

        [Display(Name = "Employees")]
        public int? NumberOfEmployees { get; set; }

        public string Ownership { get; set; }

        [Display(Name = "Ticker Symbol")]
        [StringLength(20)]
        public string TickerSymbol { get; set; }

        [Display(Name = "Account Description")]
        public string Description { get; set; }

        [Display(Name = "Account Rating")]
        public string Rating { get; set; }

        [Display(Name = "Account Site")]
        [StringLength(80)]
        public string Site { get; set; }

        [Display(Name = "Owner ID")]
        [Updateable(false)]
        public string OwnerId { get; set; }

        [Display(Name = "Created Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Created By ID")]
        [Createable(false), Updateable(false)]
        public string CreatedById { get; set; }

        [Display(Name = "Last Modified Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset LastModifiedDate { get; set; }

        [Display(Name = "Last Modified By ID")]
        [Createable(false), Updateable(false)]
        public string LastModifiedById { get; set; }

        [Display(Name = "System Modstamp")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset SystemModstamp { get; set; }

        [Display(Name = "Last Activity")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastActivityDate { get; set; }

        [Display(Name = "Last Viewed Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastViewedDate { get; set; }

        [Display(Name = "Last Referenced Date")]
        [Createable(false), Updateable(false)]
        public DateTimeOffset? LastReferencedDate { get; set; }

        [Display(Name = "Data.com Key")]
        [StringLength(20)]
        public string Jigsaw { get; set; }

        [Display(Name = "Jigsaw Company ID")]
        [StringLength(20)]
        [Createable(false), Updateable(false)]
        public string JigsawCompanyId { get; set; }

        [Display(Name = "Clean Status")]
        public string CleanStatus { get; set; }

        [Display(Name = "Account Source")]
        public string AccountSource { get; set; }

        [Display(Name = "D-U-N-S Number")]
        [StringLength(9)]
        public string DunsNumber { get; set; }

        [StringLength(255)]
        public string Tradestyle { get; set; }

        [Display(Name = "NAICS Code")]
        [StringLength(8)]
        public string NaicsCode { get; set; }

        [Display(Name = "NAICS Description")]
        [StringLength(120)]
        public string NaicsDesc { get; set; }

        [Display(Name = "Year Started")]
        [StringLength(4)]
        public string YearStarted { get; set; }

        [Display(Name = "SIC Description")]
        [StringLength(80)]
        public string SicDesc { get; set; }

        [Display(Name = "D&B Company ID")]
        public string DandbCompanyId { get; set; }

        [Display(Name = "Customer Priority")]
        [JsonProperty("CustomerPriority__c")]
        public string CustomerPriority { get; set; }

        [Display(Name = "SLA")]
        [JsonProperty("SLA__c")]
        public string SLA { get; set; }

        [Display(Name = "Active")]
        [JsonProperty("Active__c")]
        public string Active { get; set; }

        [Display(Name = "Number of Locations")]
        [JsonProperty("NumberofLocations__c")]
        public double? NumberofLocations { get; set; }

        [Display(Name = "Upsell Opportunity")]
        [JsonProperty("UpsellOpportunity__c")]
        public string UpsellOpportunity { get; set; }

        [Display(Name = "SLA Serial Number")]
        [StringLength(10)]
        [JsonProperty("SLASerialNumber__c")]
        public string SLASerialNumber { get; set; }

        [Display(Name = "SLA Expiration Date")]
        [JsonProperty("SLAExpirationDate__c")]
        public DateTimeOffset? SLAExpirationDate { get; set; }

        [Display(Name = "SAP_Customer_Number")]
        [StringLength(10)]
        [JsonProperty("SAP_Customer_Number__c")]
        public string SAPCustomerNumber { get; set; }

        [Display(Name = "Total_Entitlement_Points")]
        [JsonProperty("Total_Entitlement_Points__c ")]
        public double? TotalEntitlementPoints{ get; set; }

    }
}
