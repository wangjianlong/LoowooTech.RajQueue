using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Managers
{
    public class ManagerBase
    {

        protected RajDbContext DB { get { return OneContext.Current; } }
    }
}