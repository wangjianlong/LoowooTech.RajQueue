using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Managers
{
    public static class OneContext
    {
        private static string _contextName { get; set; }
        public static RajDbContext Current
        {
            get { return HttpContext.Current.Items[_contextName] as RajDbContext; }
        }
        static OneContext()
        {
            _contextName = "_entityContext";
        }

        public static void Begin()
        {
            HttpContext.Current.Items[_contextName] = new RajDbContext();
        }

        public static void End()
        {
            var entity = HttpContext.Current.Items[_contextName] as RajDbContext;
            if (entity != null)
            {
                entity.Dispose();
            }
        }
    }
}