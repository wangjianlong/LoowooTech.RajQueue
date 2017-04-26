using System;
using System.Collections.Generic;
using System.Data.Entity;
using LoowooTech.RajQueue.Models;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Managers
{
    public class RajDbContext:DbContext
    {
        public RajDbContext() : base("name=DbConnection")
        {
            Database.SetInitializer<RajDbContext>(null);
        }

        public DbSet<UserEvaluation> UserEvaluations { get; set; }
    }
}