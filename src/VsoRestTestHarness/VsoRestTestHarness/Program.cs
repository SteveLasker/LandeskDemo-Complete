using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoRestTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            VSOnlineService service = new VSOnlineService();
            string query = "Select [System.Id] From WorkItems Where[System.WorkItemType] = 'Bug' order by [System.CreatedDate] desc";
            List<Models.VSOnline.WorkItem> workItems = service.GetWorkItems<Models.VSOnline.WorkItem>(query).Result;
        }
    }
}
