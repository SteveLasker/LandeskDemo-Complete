using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandeskUMP.Controllers
{
    public class VSOController : Controller
    {
        // GET: VSO
        public ActionResult Index()
        {
            LandeskVSO.TFSWorkItemStoreConnector workItems = new LandeskVSO.TFSWorkItemStoreConnector();
            Microsoft.TeamFoundation.Client.TfsConfigurationServer tfsConfi = workItems.AuthenticateToTFSService();

                
            return View();
        }
    }
}