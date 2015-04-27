using System;
using System.Collections.Generic;

namespace LandeskUMP.Models
{
    /// <summary>
    /// The aggregate of combining bugs and customer data to prioritize a list of bugs in business priority order
    /// </summary>
    public class UmpScoreResult
    {
        public UmpScoreResult(VSOnline.WorkItem workItem, int umpScore)
        {
            this.Id = workItem.Id;
            this.Title = workItem.Title;
            this.Severity = workItem.Severity;
            this.AreaPath = workItem.AreaPath;
            this.DevTeam = "";
            this.CreatedDate = workItem.CreatedDate;
            this.TotalUmp = umpScore;
            this.Cases = workItem.Cases;
            this.State = workItem.State;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Severity { get; set; }
        public string State { get; set; }
        public string AreaPath { get; set; }
        public string DevTeam { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalUmp { get; set; }
        public int Priority { get; set; }
        public List<Models.Salesforce.Case> Cases { get; set; }
    }
}