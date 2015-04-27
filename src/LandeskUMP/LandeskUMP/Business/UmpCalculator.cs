﻿#region usings
using LandeskUMP.Models;
using LandeskUMP.Salesforce;
using LandeskUMP.VSOnline;
using Salesforce.Common.Models;
using Salesforce.Force;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endregion
namespace LandeskUMP.Business
{
    /// <summary>
    /// Does the UMP calculations by bringing in the defects, the cases, matching them up and outputting the UMP score
    /// </summary>
    public class UmpCalculator
    {
        public async Task<IEnumerable<UmpScoreResult>> GetUmpScores()
        {
            // *******************************
            // Get the open bugs from VSOnline
            // *******************************
            VSOnlineService vsoService = new VSOnlineService();
            string vsoQuery = "SELECT [System.Id]"
                            + " FROM WorkItems"
                            + " WHERE [System.TeamProject] = 'SSM'"
                            + " AND  [System.WorkItemType] = 'Bug'"
                            + " ORDER BY [System.CreatedDate]";
            List<Models.VSOnline.WorkItem> vsoBugs = await vsoService.GetWorkItems<Models.VSOnline.WorkItem>(vsoQuery);

            // *******************************
            // Get the open cases from Salesforce
            // *******************************
            string sfQuery = "SELECT Id, CaseNumber, Severity__c, VSOnlineId__c, CreatedDate, ClosedDate, "
                                          + " Product_Line__c, Category__c, Status, AccountId, Account.Name, "
                                          + " Account.SAP_Customer_Number__c, Account.Total_Entitlement_Points__c "
                                + "    FROM Case "
                                + " WHERE Status <>'Closed' ";

            ForceClient forceClient = await SalesforceService.GetUserNamePasswordForceClientAsync();

            //TODO: Move results to next line
            var result = await forceClient.QueryAsync<Models.Salesforce.Case>(sfQuery);
            List<Models.Salesforce.Case> sfCases = result.records;

            // Connect cases to defects:
            // For each WorkItem, get a collection of Salesforce Cases
            #region Create a graph of VSOnline WorkItem IDs, with the collection of Salesforce Cases associated with each
            // Order the two collections
            vsoBugs = vsoBugs.OrderBy(d => d.Id).ToList();
            sfCases = sfCases.OrderBy(c => c.VSOnlineId).ToList();
            Dictionary<int, List<Models.Salesforce.Case>> umpItems = vsoBugs.ToDictionary(d => d.Id, d => new List<Models.Salesforce.Case>());

            // For each Salesforce Case, see if the associated WorkItem (VSOnlineId) is in the umpItems collection
            foreach (Models.Salesforce.Case c in sfCases)
            {
                if (umpItems.ContainsKey((int)c.VSOnlineId))
                {
                    umpItems[(int)c.VSOnlineId].Add(c);
                }
            }

            // Put the list of Salesforce Cases back on each WorkItem in the vsoBugs collection
            foreach (Models.VSOnline.WorkItem b in vsoBugs)
            {
                List<Models.Salesforce.Case> temp = new List<Models.Salesforce.Case>();
                umpItems.TryGetValue(b.Id, out temp);
                b.Cases = temp;
            }
            #endregion

            // Calculate UMP Score
            var umpResults =
                from u in vsoBugs
                where (u.Cases.Count > 0)
                select new Models.UmpScoreResult(u, GetUmpScore(u));

            return umpResults.OrderByDescending(ump => ump.TotalUmp);
        }

        /// <summary>
        /// Get the UMP score for a particular defect
        /// </summary>
        /// <returns>UMP score as int</returns>
        public int GetUmpScore(Models.VSOnline.WorkItem workItem)
        {
            // Constants:
            const int k = 5000; // max for each UMP score

            // Adjustors:
            const double pointsModifier = 15000.0; // divided into total entitlement points for UMP 2

            // First half of UMP score - using only information from the defect
            // get the starting value - determined by the severity of the defect
            int start1 = 1;
            switch (workItem.Severity)
            {
                case "1 - Critical":
                    start1 = 1000;
                    break;
                case "2 - High":
                    start1 = 500;
                    break;
                case "3 - Medium":
                    start1 = 100;
                    break;
                case "4 - Low":
                    start1 = 1;
                    break;
            }

            // get the rate, which is determined by the number of cases attached
            double rate1;
            try
            { rate1 = (.01 * Math.Sqrt(workItem.Cases.Count)) + (.005 / workItem.Cases.Count); }
            catch (DivideByZeroException) { rate1 = .001; } // if there are zero cases, the world of math gets angry, so we set it to the lowest possible value

            // How long has the bug been open
            int time = ((DateTime.Now) - (workItem.CreatedDate)).Days;

            if (time < 0)
                time = 0;

            // Now we are ready for UMP 1
            double ump1 = k / (1 + (((k - start1) / start1) * Math.Pow(Math.E, (-rate1 * time))));

            // Second half of the UMP score, using information about the cases that are attached 
            // to the defect and the size of the associated customers
            // This one always starts at 1
            int start2 = 1;

            // now the rate, which is determined by the severity of the attached cases
            double rate2;
            var severityList =
                from c in workItem.Cases
                select new
                {
                    Severity = c.Severity__c,
                    Score = 5 - Int32.Parse(Regex.Match(c.Severity__c, @"\d+").Value)
                };

            // this gets the average of the severitys ( sev 1 = 4 ... sev 4 = 1) with the highest severity thrown out
            double severityScore;
            if (severityList.Count() == 0)
                severityScore = .075 - (1 / 30);
            if (severityList.Count() < 2)
                severityScore = severityList.Average(s => s.Score);
            else
                severityScore = (severityList.Sum(s => s.Score) - severityList.Max(s => s.Score)) / (severityList.Count() - 1);
            rate2 = .075 + ((severityScore - 1) / 30);

            // This half of the score increases over entitlements points instead of time so:
            double totalPoints = workItem.Cases.Sum(c => (c.Account.Total_Entitlement_Points__c ?? 0)) / pointsModifier;

            double ump2 = k / (1 + (((k - start2) / start2) * Math.Pow(Math.E, (-rate2 * totalPoints))));

            return (int)Math.Round(ump1 + ump2, 0);

        }

    }
}