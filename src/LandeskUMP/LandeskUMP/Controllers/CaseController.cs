using Salesforce.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LandeskUMP.Models.Salesforce;
using LandeskUMP.Salesforce;

namespace LandeskUMP.Controllers
{
	[Authorize]
    public class CaseController : Controller
    {
        // Note: the SOQL Field list, and Binding Property list have subtle differences as custom properties may be mapped with the JsonProperty attribute to remove __c
        const string _CasePostBinding = "CaseNumber,Subject,Description,Reason,Category,EngineeringReqNumber,Status,Priority,Severity,Product_Line,Product,SLAViolation";
        /*
                    Id,
                    AccountId,
                    CaseNumber,
                    Subject,
                    Description,
                    Reason,
                    Category__c,
                    EngineeringReqNumber__c,
                    Status,
                    Priority,
                    Severity__c,
                    Product_Line__c,
                    Product__c,
                    SLAViolation__c,
                    CreatedDate,
                    ClosedDate,
                    CreatedById,
                    IsClosed,
                    IsDeleted
        */


        public async Task<ActionResult> Index()
        {
            IEnumerable<Case> selectedCases = Enumerable.Empty<Case>();
            try
            {
                selectedCases = await SalesforceService.MakeAuthenticatedClientRequestAsync(
                    async (client) =>
                    {
                        QueryResult<Case> cases =
                        await client.QueryAsync<Case>("SELECT Id,CaseNumber,Subject,Description,Reason,Category__c,EngineeringReqNumber__c,Status,Priority,Severity__c,Product_Line__c,Product__c,SLAViolation__c,CreatedDate,ClosedDate,CreatedById,IsClosed,IsDeleted FROM Case WHERE Severity__c > ' '");
                        return cases.records;
                    }
                    );
            }
            catch (Exception e)
            {
                this.ViewBag.OperationName = "query Salesforce Cases";
                this.ViewBag.AuthorizationUrl = SalesforceOAuthRedirectHandler.GetAuthorizationUrl(this.Request.Url.ToString());
                this.ViewBag.ErrorMessage = e.Message;
            }
            if (this.ViewBag.ErrorMessage == "AuthorizationRequired")
            {
                return Redirect(this.ViewBag.AuthorizationUrl);
            }
            return View(selectedCases);

        }
    }
}