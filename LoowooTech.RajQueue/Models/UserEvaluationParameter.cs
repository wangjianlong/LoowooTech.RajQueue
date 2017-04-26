using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Models
{
    public class UserEvaluationParameter
    {
        public TimeType TimeType { get; set; }
        public StatictType Type { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public PageParameter Page { get; set; }
        public string[] Types { get; set; }
        public string[] IDs { get; set; }
        public string[] Departments { get; set; }
        public string UserID { get; set; }
        public MigrateType MigrateType { get; set; }

    }
}