#region usings
using LandeskUMP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion
namespace LandeskUMP.API
{
    public class UmpController : ApiController
    {
        public async Task<IEnumerable<Models.UmpScoreResult>> Get()
        {
            return await new Business.UmpCalculator().GetUmpScores();
        }
    }
}
