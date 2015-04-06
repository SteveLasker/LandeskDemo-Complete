using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandeskUMP.Models
{
    public enum WorkItemState
    {
        New,
        Approved,
        Committed,
        Done,
        Removed,
        None
    }
}