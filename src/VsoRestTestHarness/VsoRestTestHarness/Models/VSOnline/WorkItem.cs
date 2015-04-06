using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Models.VSOnline
{
    public class WorkItem
    {
        [JsonProperty("System.Id")]
        public string Id { get; set; }

        [JsonProperty("System.Title")]
        public string Title { get; set; }

        [JsonProperty("System.AreaPath")]
        public string AreaPath { get; set; }

        [JsonProperty("System.TeamProject")]
        public string TeamProject { get; set; }

        [JsonProperty("System.IterationPath")]
        public string IterationPath { get; set; }

        [JsonProperty("System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty("System.State")]
        public string State { get; set; }

        [JsonProperty("System.Reason")]
        public string Reason { get; set; }

        [JsonProperty("System.CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("System.CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("System.ChangedDate")]
        public DateTime ChangedDate { get; set; }

        [JsonProperty("System.ChangedBy")]
        public string ChangedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Severity")]
        public string Severity { get; set; }
    }
}
