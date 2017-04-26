using LoowooTech.RajQueue.Common;
using LoowooTech.RajQueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Managers
{
    public class UserEvaluationManager:ManagerBase
    {
        /// <summary>
        /// 作用：获取所有的业务类型种类
        /// 作者：汪建龙
        /// 编写时间：2017年4月13日11:13:09
        /// </summary>
        /// <returns></returns>
        public List<string> GetWindTypes()
        {
            return DB.UserEvaluations.GroupBy(e => e.WinType).Select(e => e.Key).ToList()??new List<string>();
        }

        /// <summary>
        /// 作用：获取所有的窗口类型种类
        /// 作者：汪建龙
        /// 编写时间：2017年4月13日11:13:40
        /// </summary>
        /// <returns></returns>
        public List<string> GetWindIds()
        {
            return DB.UserEvaluations.GroupBy(e => e.WinID).Select(e => e.Key).ToList()??new List<string>();
        }


        public List<string> GetUserIDs()
        {
            return DB.UserEvaluations.GroupBy(e => string.IsNullOrEmpty(e.UserID)?"空":e.UserID).Select(e => e.Key).ToList()??new List<string>();
        }

        public List<string> GetDepartments()
        {
            return DB.UserEvaluations.GroupBy(e => string.IsNullOrEmpty(e.Department) ? "空" : e.Department).Select(e => e.Key).ToList()??new List<string>();
        }

        public List<UserEvaluation> Search(UserEvaluationParameter parameter)
        {
            var query = DB.UserEvaluations.AsQueryable();
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CallTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CallTime <= parameter.EndTime.Value);
            }
            if (!string.IsNullOrEmpty(parameter.UserID))
            {
                switch (parameter.MigrateType)
                {
                    case MigrateType.To:
                        if (parameter.UserID == "空")
                        {
                            query = query.Where(e => string.IsNullOrEmpty(e.UserID));
                        }
                        else
                        {
                            query = query.Where(e => e.UserID == parameter.UserID);
                        }
                        break;
                    case MigrateType.From:
                        if (parameter.UserID == "空")
                        {
                            query = query.Where(e => string.IsNullOrEmpty(e.FromUserID));
                        }
                        else
                        {
                            query = query.Where(e => e.FromUserID == parameter.UserID);
                        }
                        break;
                }

                
            }

 
            if (parameter.Types != null && parameter.Types.Length > 0)
            {
                query = query.Where(e => parameter.Types.Contains(e.WinType));
            }

            if (parameter.IDs != null && parameter.IDs.Length > 0)
            {
                if (parameter.IDs.Contains("空"))
                {
                    query = query.Where(e => parameter.IDs.Contains(e.WinID) || string.IsNullOrEmpty(e.WinID));
                }
                else
                {
                    query = query.Where(e => parameter.IDs.Contains(e.WinID));
                }
                
            }

            if (parameter.Departments != null && parameter.Departments.Length > 0)
            {
                if (parameter.Departments.Contains("空"))
                {
                    query = query.Where(e => parameter.Departments.Contains(e.Department) || string.IsNullOrEmpty(e.Department));
                }
                else
                {
                    query = query.Where(e => parameter.Departments.Contains(e.Department));
                }
            }

            query = query.OrderByDescending(e => e.ID).SetPage(parameter.Page);
            return query.ToList();
        }
        public Dictionary<string,Dictionary<string,long>> StatisticWait(UserEvaluationParameter parameter)
        {
            var dict = new Dictionary<string, Dictionary<string, long>>();
            var query = DB.UserEvaluations.Where(e=>e.CallTime.HasValue).AsQueryable();
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CallTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CallTime <= parameter.EndTime.Value);
            }

            var list = query.ToList();
            Dictionary<string, List<UserEvaluation>> temp = null;
            switch (parameter.TimeType)
            {
                case TimeType.Day:
                    temp = list.GroupBy(e => e.GetNumberTime.ToString("yyyy-MM-dd")).ToDictionary(e => e.Key, e => e.ToList());
                    break;
                case TimeType.Month:
                    temp = list.GroupBy(e => e.GetNumberTime.ToString("yyyy-MM")).ToDictionary(e => e.Key, e => e.ToList());
                    break;
            }
            switch (parameter.Type)
            {
                case StatictType.Type:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.WinType) ? "NULL" : k.WinType).ToDictionary(k => k.Key, k => k.Sum(j => j.Seconds) / k.LongCount()));

                    break;
                case StatictType.User:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.UserID) ? "NULL" : k.UserID).ToDictionary(k => k.Key, k => k.Sum(j => j.Seconds) / k.LongCount()));
                    break;
                case StatictType.Win:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.WinID) ? "NULL" : k.WinID).ToDictionary(k => k.Key, k => k.Sum(j=>j.Seconds)/k.LongCount()));
                    break;
                case StatictType.Department:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.Department) ? "NULL" : k.Department).ToDictionary(k => k.Key, k =>k.Sum(j=>j.Seconds)/ k.LongCount()));
                    break;
            }
            return dict;
        }

        public Dictionary<string,Dictionary<string,long>> Statistic(UserEvaluationParameter parameter)
        {
            var dict = new Dictionary<string, Dictionary<string, long>>();
            var query = DB.UserEvaluations.AsQueryable();
            if (parameter.StartTime.HasValue)
            {
                query = query.Where(e => e.CallTime >= parameter.StartTime.Value);
            }
            if (parameter.EndTime.HasValue)
            {
                query = query.Where(e => e.CallTime <= parameter.EndTime.Value);
            }
            var list = query.ToList();
            Dictionary<string, List<UserEvaluation>> temp = null;
            switch (parameter.TimeType)
            {
                case TimeType.Day:
                    temp = list.GroupBy(e => e.GetNumberTime.ToString("yyyy-MM-dd")).ToDictionary(e => e.Key, e => e.ToList());
                    break;
                case TimeType.Month:
                    temp = list.GroupBy(e => e.GetNumberTime.ToString("yyyy-MM")).ToDictionary(e => e.Key, e => e.ToList());
                    break;
            }
            switch (parameter.Type)
            {
                case StatictType.Type:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.WinType)?"NULL":k.WinType).ToDictionary(k => k.Key, k => k.LongCount()));
                    break;
                case StatictType.User:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.UserID)?"NULL":k.UserID).ToDictionary(k => k.Key, k => k.LongCount()));
                    break;
                case StatictType.Win:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.WinID)?"NULL":k.WinID).ToDictionary(k => k.Key, k => k.LongCount()));
                    break;
                case StatictType.Department:
                    dict = temp.ToDictionary(e => e.Key, e => e.Value.GroupBy(k => string.IsNullOrEmpty(k.Department) ? "NULL" : k.Department).ToDictionary(k => k.Key, k => k.LongCount()));
                    break;
            }
            return dict;
        }
    }
}