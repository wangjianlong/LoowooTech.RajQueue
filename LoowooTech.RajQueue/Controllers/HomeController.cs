using LoowooTech.RajQueue.Common;
using LoowooTech.RajQueue.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoowooTech.RajQueue.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Statistic(
            TimeType? timeType=null,StatictType? type=null,
            DateTime? startTime=null,DateTime? endTime=null,
            int page=1,int rows=20)
        {
            var parameter = new UserEvaluationParameter()
            {
                TimeType = timeType.HasValue ? timeType.Value : TimeType.Day,
                Type = type.HasValue ? type.Value : StatictType.Type,
                StartTime = startTime,
                EndTime = endTime
            };
            var dict = Core.UserEvaluationManager.Statistic(parameter);
            ViewBag.Dict = dict;
            ViewBag.Head = Extract<string>(dict);
            ViewBag.HeadDict = WinTypeHelper.Dict;
            ViewBag.Parameter = parameter;
            return View();
        }

        public ActionResult StatisticWait(
            TimeType? timeType = null, StatictType? type = null,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            var parameter = new UserEvaluationParameter()
            {
                TimeType = timeType.HasValue ? timeType.Value : TimeType.Day,
                Type = type.HasValue ? type.Value : StatictType.Type,
                StartTime = startTime,
                EndTime = endTime
            };
            var dict = Core.UserEvaluationManager.StatisticWait(parameter);
            ViewBag.Dict = dict;
            ViewBag.Head = Extract<string>(dict);
            ViewBag.Parameter = parameter;
            return View();
        }
        public ActionResult DownloadWait(
            TimeType? timeType = null, StatictType? type = null,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            var parameter = new UserEvaluationParameter()
            {
                TimeType = timeType.HasValue ? timeType.Value : TimeType.Day,
                Type = type.HasValue ? type.Value : StatictType.Type,
                StartTime = startTime,
                EndTime = endTime
            };
            var dict = Core.UserEvaluationManager.StatisticWait(parameter);
            IWorkbook workbook = ExcelManager.Save(dict, parameter.Type, Extract<string>(dict), WinTypeHelper.Dict);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "平均等待时间.xls");
        }
        public ActionResult DownloadIndex(
            TimeType? timeType = null, StatictType? type = null,
            DateTime? startTime = null, DateTime? endTime = null)
        {
            var parameter = new UserEvaluationParameter()
            {
                TimeType = timeType.HasValue ? timeType.Value : TimeType.Day,
                Type = type.HasValue ? type.Value : StatictType.Type,
                StartTime = startTime,
                EndTime = endTime
            };
            var dict = Core.UserEvaluationManager.Statistic(parameter);
            IWorkbook workbook = ExcelManager.Save(dict, parameter, Extract<string>(dict), WinTypeHelper.Dict);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "统计表.xls");

        }
        private List<T> Extract<T>(Dictionary<string, Dictionary<T, double>> dict)
        {
            var list = new List<T>();
            foreach (var value in dict.Values)
            {
                foreach (var key in value.Keys)
                {
                    if (!list.Contains(key))
                    {
                        list.Add(key);
                    }
                }
            }
            return list;
        }
        private List<T> Extract<T>(Dictionary<string,Dictionary<T,long>> dict)
        {
            var list = new List<T>();
            foreach(var value in dict.Values)
            {
                foreach(var key in value.Keys)
                {
                    if (!list.Contains(key))
                    {
                        list.Add(key);
                    }
                }
            }
            return list;
        }

        public ActionResult Search(
            DateTime?startTime=null,DateTime?endTime=null,
            string[] type=null,string[] id=null,
            string userId=null, string[] department=null,
            int page=1,int rows=20)
        {
            var parameter = new UserEvaluationParameter
            {
                StartTime=startTime,
                EndTime=endTime,
                Types=type,
                IDs=id,
                UserID=userId,
                Departments=department,
                MigrateType=MigrateType.To,
                Page = new PageParameter(page, rows)
            };
            ViewBag.List = Core.UserEvaluationManager.Search(parameter);
            ViewBag.Parameter = parameter;
            ViewBag.Types = Core.UserEvaluationManager.GetWindTypes();
            ViewBag.IDs = Core.UserEvaluationManager.GetWindIds();
            ViewBag.UserIDs = Core.UserEvaluationManager.GetUserIDs();
            ViewBag.Departments = Core.UserEvaluationManager.GetDepartments();
            return View();
        }

        public ActionResult DownloadSearch(
            DateTime? startTime = null, DateTime? endTime = null,
            string[] type = null, string[] id = null,
            string userId = null, string[] department = null)
        {
            var parameter = new UserEvaluationParameter
            {
                StartTime = startTime,
                EndTime = endTime,
                Types = type,
                IDs = id,
                UserID = userId,
                Departments = department,
                MigrateType = MigrateType.To
            };
            var list = Core.UserEvaluationManager.Search(parameter);
            IWorkbook workbook = ExcelManager.Save(list, WinTypeHelper.Dict);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "一般查询.xls");

        }

        public ActionResult Migrate(
            DateTime?startTime=null,DateTime? endTime=null,
            string userId=null,
            MigrateType MigrateType=MigrateType.To,
            int page=1,int rows=20)
        {
            var parameter = new UserEvaluationParameter
            {
                StartTime=startTime,
                EndTime=endTime,
                UserID=userId,
                MigrateType = MigrateType,
                Page = new PageParameter(page, rows)
            };
            ViewBag.List = Core.UserEvaluationManager.Search(parameter);
            ViewBag.UserIDs = Core.UserEvaluationManager.GetUserIDs();
            ViewBag.Parameter = parameter;
            return View();
        }
        public ActionResult DownloadMigrate(DateTime? startTime = null, DateTime? endTime = null,
            string userId = null,
            MigrateType MigrateType = MigrateType.To)
        {
            var parameter = new UserEvaluationParameter
            {
                StartTime = startTime,
                EndTime = endTime,
                UserID = userId,
                MigrateType = MigrateType
            };
            var list= Core.UserEvaluationManager.Search(parameter);
            IWorkbook workbook = ExcelManager.Save2(list, WinTypeHelper.Dict);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            byte[] fileContents = ms.ToArray();
            return File(fileContents, "application/ms-excel", "转移.xls");
        }

        public ActionResult Wait()
        {
            return View();
        }
    }
}