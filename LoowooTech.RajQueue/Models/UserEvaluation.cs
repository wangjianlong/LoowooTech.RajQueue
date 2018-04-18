using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Models
{
    [Table("UserEvaluation")]
    public class UserEvaluation
    {
        [Key]
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public string WinID { get; set; }
        public string WinType { get; set; }
        public string Department { get; set; }
        public string FromWinID { get; set; }
        public string FromUserID { get; set; }
        public DateTime? CallTime { get; set; }

        public DateTime GetNumberTime { get; set; }
        [NotMapped]
        public long Seconds
        {
            get
            {
                TimeSpan time = CallTime.Value - GetNumberTime;
                return time.Seconds + time.Minutes * 60 + time.Hours * 3600 + time.Days * 24 * 3600;
                //return time.TotalSeconds;
            }
        }
    }

    public enum TimeType
    {
        [Description("日统计")]
        Day,
        [Description("月统计")]
        Month
    }

    public enum StatictType
    {
        [Description("窗口")]
        Win,
        [Description("用户")]
        User,
        [Description("业务")]
        Type,
        [Description("部门")]
        Department,
    }

    public enum MigrateType
    {
        [Description("转移到")]
        To,
        [Description("转移自")]
        From,
    }
}