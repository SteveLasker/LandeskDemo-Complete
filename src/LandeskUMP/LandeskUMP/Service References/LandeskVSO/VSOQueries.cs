using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandeskUMP.Service_References.LandeskVSO
{
    public class VSOQueries
    {
        internal Dictionary<VSOQueryTypes, string> _queries;

        internal VSOQueries()
        {
            _queries = new Dictionary<VSOQueryTypes, string>();

        }
        internal void LoadQueries()
        {
            _queries.Add(VSOQueryTypes.AllBugs,
                "SELECT [System.Id]"
                    + " FROM WorkItems"
                    + " WHERE [System.TeamProject] = 'SSM'"
                    + " AND  [System.WorkItemType] = 'Bug'"
                    + " ORDER BY [System.CreatedDate]");
        }
        internal enum VSOQueryTypes
        {
            AllBugs,
            ActiveBugs
        }
        
    }
}