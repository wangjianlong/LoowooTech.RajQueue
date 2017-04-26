using LoowooTech.RajQueue.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoowooTech.RajQueue.Common
{
    public static class ExcelManager
    {
        private static string _modelExcelFilePath { get; set; }
        static ExcelManager()
        {
            _modelExcelFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excels", System.Configuration.ConfigurationManager.AppSettings["Excel"] ?? "Table1.xls");
        }
        private static IWorkbook OpenExcel(this string filePath)
        {
            IWorkbook workbook = null;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
            }
            return workbook;
        }
        private static ICell GetCell(IRow row,int line,ICell modelCell = null)
        {
            ICell cell = row.GetCell(line);
            if (cell == null)
            {
                if (modelCell != null)
                {
                    cell = row.CreateCell(line, modelCell.CellType);
                    cell.CellStyle = modelCell.CellStyle;
                }
                else
                {
                    cell = row.CreateCell(line);
                }
            }
            return cell;
        }

        public static IWorkbook Save2(List<UserEvaluation> list,Dictionary<string,string> dict)
        {
            IWorkbook workbook = _modelExcelFilePath.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var row = sheet.GetRow(0);
                    var modelCell = row.GetCell(0);
                    GetCell(row, 0, modelCell).SetCellValue("用户ID");
                    GetCell(row, 1, modelCell).SetCellValue("来自用户ID");
                    GetCell(row, 2, modelCell).SetCellValue("窗口号");
                    GetCell(row, 3, modelCell).SetCellValue("业务类型");
                    GetCell(row, 4, modelCell).SetCellValue("部门");
                    GetCell(row, 5, modelCell).SetCellValue("取号时间");
                    GetCell(row, 6, modelCell).SetCellValue("叫号时间");
                    var i = 1;
                    foreach(var item in list)
                    {
                        row = sheet.GetRow(i);
                        if (row == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        i++;
                        var cell = GetCell(row, 0, modelCell);
                        cell.SetCellValue(item.UserID);
                        GetCell(row, 1, modelCell).SetCellValue(item.FromUserID);
                        GetCell(row, 2, modelCell).SetCellValue(item.WinID);
                        GetCell(row, 3, modelCell).SetCellValue(dict.ContainsKey(item.WinType)?dict[item.WinType]:item.WinType);
                        GetCell(row, 4, modelCell).SetCellValue(item.Department);
                        GetCell(row, 5, modelCell).SetCellValue(item.GetNumberTime);
                        GetCell(row, 6, modelCell).SetCellValue(item.CallTime.HasValue?item.CallTime.Value.ToString():"");
                    }
                }
            }
            return workbook;
        }
        public static IWorkbook Save(List<UserEvaluation> list,Dictionary<string,string> dict)
        {
            IWorkbook workbook = _modelExcelFilePath.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var row = sheet.GetRow(0);
                    var modelCell = row.GetCell(0);
                    GetCell(row, 0, modelCell).SetCellValue("用户ID");
                    GetCell(row, 1, modelCell).SetCellValue("窗口类型");
                    GetCell(row, 2, modelCell).SetCellValue("业务类型");
                    GetCell(row, 3, modelCell).SetCellValue("部门");
                    GetCell(row, 4, modelCell).SetCellValue("取号时间");
                    GetCell(row, 5, modelCell).SetCellValue("叫号时间");
                    var i = 1;
                    foreach(var item in list)
                    {
                        row = sheet.GetRow(i);
                        if (row == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        i++;
                        var cell = GetCell(row, 0, modelCell);
                        cell.SetCellValue(item.UserID);
                        GetCell(row, 1, modelCell).SetCellValue(item.WinID);
                        GetCell(row, 2, modelCell).SetCellValue(dict.ContainsKey(item.WinType)?dict[item.WinType]:item.WinType);
                        GetCell(row, 3, modelCell).SetCellValue(item.Department);
                        GetCell(row, 4, modelCell).SetCellValue(item.GetNumberTime);
                        GetCell(row, 5, modelCell).SetCellValue(item.CallTime.HasValue?item.CallTime.Value.ToString():"");
                    }
                }
            }
            return workbook;   
        }

        public static IWorkbook Save(Dictionary<string,Dictionary<string,long>> dict,StatictType type,List<string> heads, Dictionary<string, string> headDict)
        {
            IWorkbook workbook = _modelExcelFilePath.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var row = sheet.GetRow(0);
                    var modelCell = row.GetCell(0);
                    modelCell.SetCellValue(string.Format("时间\\{0}", type.GetDescription()));
                    var i = 1;
                    foreach(var head in heads)
                    {
                        var cell = GetCell(row, i++, modelCell);
                        if (headDict.ContainsKey(head))
                        {
                            cell.SetCellValue(headDict[head]);
                        }
                        else
                        {
                            cell.SetCellValue(head);
                        }
                    }
                    i = 1;
                    foreach (var entry in dict.OrderByDescending(e => e.Key))
                    {
                        row = sheet.GetRow(i);
                        if (row == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        i++;
                        var cell = GetCell(row, 0, modelCell);
                        cell.SetCellValue(entry.Key);
                        var j = 1;
                        foreach (var head in heads)
                        {
                            cell = GetCell(row, j++, modelCell);
                            if (entry.Value.ContainsKey(head))
                            {
                                cell.SetCellValue(entry.Value[head]+"s");
                            }
                            else
                            {
                                cell.SetCellValue("/");
                            }
                        }
                    }
                }
            }
            return workbook;
        }

        public static IWorkbook Save(
            Dictionary<string,Dictionary<string,long>> dict,
            UserEvaluationParameter paramter,
            List<string> heads,
            Dictionary<string,string> headDict)
        {
            if (!System.IO.File.Exists(_modelExcelFilePath))
            {
                return null;
            }

            IWorkbook workbook = _modelExcelFilePath.OpenExcel();
            if (workbook != null)
            {
                var sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    var modelrow = sheet.GetRow(0);
                    var modelcell = modelrow.GetCell(0);
                    modelcell.SetCellValue(string.Format("时间\\{0}", paramter.Type.GetDescription()));
                    var i = 1;
                    foreach(var head in heads)
                    {
                        var cell = GetCell(modelrow, i++, modelcell);
                        if (headDict.ContainsKey(head))
                        {
                            cell.SetCellValue(headDict[head]);
                        }
                        else
                        {
                            cell.SetCellValue(head);
                        }
                    }
                    i = 1;
                    foreach(var entry in dict.OrderByDescending(e => e.Key))
                    {
                        var row = sheet.GetRow(i);
                        if (row == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        i++;
                        var cell = GetCell(row, 0, modelcell);
                        cell.SetCellValue(entry.Key);
                        var j = 1;
                        foreach(var head in heads)
                        {
                            cell = GetCell(row, j++, modelcell);
                            if (entry.Value.ContainsKey(head))
                            {
                                cell.SetCellValue(entry.Value[head]);
                            }
                            else
                            {
                                cell.SetCellValue("/");
                            }
                        }
                    }
                }
            }
            return workbook;
        }


    }
}