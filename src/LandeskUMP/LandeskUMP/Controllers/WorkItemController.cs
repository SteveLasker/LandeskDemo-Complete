using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using LandeskUMP.VSOnline;
using LandeskUMP.Models.VSOnline;
namespace LandeskUMP.Controllers
{
    [Authorize]
    public class WorkItemController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string query = "Select [System.Id] From WorkItems Where[System.WorkItemType] = 'Bug' order by [System.CreatedDate] desc";
            List<WorkItem> workItems = await new VSOnlineService().GetWorkItems<WorkItem>(query);

            return View(workItems);
        }
    }
}