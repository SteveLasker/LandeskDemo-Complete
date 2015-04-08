using LandeskUMP.Models;
using LandeskUMP.Salesforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace LandeskUMP.API
{
    public class UmpController : ApiController
    {
        public async Task<IEnumerable<Models.UmpScoreResult>> Get()
        {
            Business.UmpCalculator umpCalculator = new Business.UmpCalculator();
            IEnumerable<UmpScoreResult> umpResult = Enumerable.Empty<UmpScoreResult>();
            umpResult = await umpCalculator.GetUmpScores();

            return umpResult;
        }
    }
}
