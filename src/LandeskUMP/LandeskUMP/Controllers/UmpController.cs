using LandeskUMP.Models;
using LandeskUMP.Salesforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LandeskUMP.Controllers
{
	[Authorize]
    public class UmpController : Controller
    {
        // GET: Ump
        public async Task<ActionResult> Index()
        {
            Business.UmpCalculator umpCalculator = new Business.UmpCalculator();
            IEnumerable<UmpScoreResult> umpResult = Enumerable.Empty<UmpScoreResult>();
            try
            {
                umpResult = await umpCalculator.GetUmpScores();
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "Query Salesforce Cases";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }

            return View(umpResult);
        }
    }
}