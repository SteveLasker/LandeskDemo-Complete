using LandeskUMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LandeskUMP.Controllers
{
    public class VSOController : Controller
    {
        // GET: VSO
        public async Task<ActionResult> Index()
        {
            LandeskVSO.VSOnlineService service = new LandeskVSO.VSOnlineService();
            string query = "Select [System.Id] From WorkItems Where[System.WorkItemType] = 'Bug' order by [System.CreatedDate] desc";
            List<Models.VSOnline.WorkItem> workItems = await service.GetWorkItems<Models.VSOnline.WorkItem>(query);

                
            return View(workItems);
        }
    }
}