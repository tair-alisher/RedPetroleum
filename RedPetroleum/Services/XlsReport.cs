﻿using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;

namespace RedPetroleum.Services
{
    public class XlsReport
    {
        UnitOfWork unit;
        Guid? departmentId;
        string reportType;
        DateTime? dt;
        public XlsReport(UnitOfWork unit, Guid? departmentId, string reportType, DateTime? dt)
        {
            this.unit = unit;
            this.departmentId = departmentId;
            this.reportType = reportType;
            this.dt = dt;
        }

        public ExcelPackage FormReport()
        {
            if (this.reportType == "ReportByDepartment")
                return ReportByDepartment();
            else if (this.reportType == "ReportByCompany")
                return ReportByCompany();
            else if (this.reportType == "ReportByDepartmentAverageMark")
                return ReportByDepartmentAverageMark();
            else if (this.reportType == "ReportByConsolidated")
                return ReportByConsolidated();
            else if (this.reportType == "ReportByInstructionsDG")
                return ReportByInstructionsDG();
            else
                return ReportByDepartment();
        }

        public ExcelPackage ReportByDepartment()
        {
            string ReportTitle = "ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА";

            ExcelPackage xlsPack = new ExcelPackage();

            ExcelWorksheet worksheet = xlsPack.Workbook.Worksheets.Add("Report");
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml
                .Style
                .ExcelHorizontalAlignment
                .Center;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml
                .Style
                .ExcelVerticalAlignment
                .Center;

            string departmentName = unit
                .Departments
                .GetDepartmentNameById((Guid)departmentId);

            worksheet.Column(1).Width = 5.69; //Width = 5.00 in excel
            worksheet.Column(2).Width = 31.00; //Width = 30.29 in excel
            worksheet.Column(3).Width = 25.69; //Width = 25.00 in excel
            worksheet.Column(4).Width = 11.00; //Width = 10.29 in excel
            worksheet.Column(5).Width = 14.69; //Width = 14.00 in excel

            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;

            worksheet.Cells["A1:E1"].Merge = true;

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, dt);
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;

            worksheet.Cells["A2:E2"].Merge = true;
            worksheet.Cells["A2"].Value = departmentName;
            worksheet.Cells["A2"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Style.Font.Size = 14;

            worksheet.Cells["A3"].Value = "№";
            worksheet.Cells["A3"].Style.Font.Bold = true;

            worksheet.Cells["B3"].Value = "Ф.И.О";
            worksheet.Cells["B3"].Style.Font.Bold = true;
            
            worksheet.Cells["C3"].Value = "Должность";
            worksheet.Cells["C3"].Style.Font.Bold = true;

            worksheet.Cells["D3"].Value = "Средний показатель";
            worksheet.Cells["D3"].Style.Font.Bold = true;

            worksheet.Cells["E3"].Value = "Подпись";
            worksheet.Cells["E3"].Style.Font.Bold = true;
          
            for (int k = 1; k <= 3; k++)
            {
                BorderLinesForReportByDepartment(worksheet,k);
            }
            
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByDepartmentId((Guid)departmentId,dt);
            int rowStart = 4;
            int i = 1;
            int j = 4;
            foreach (Employee employee in employees)
            {
                BorderLinesForReportByDepartment(worksheet,j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"C{rowStart}"].Value = employee.Position.Name;
                worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"D{rowStart}"].Value = employee.TaskLists.Select(t => t.AverageMark).Average() + "%";
                rowStart++;
                j++;
            }

            return xlsPack;
        }
        public void BorderLinesForReportByDepartment(ExcelWorksheet worksheet, int a)
        {          
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        public ExcelPackage ReportByCompany()
        {
            string ReportTitle = "ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА";

            ExcelPackage xlsPack = new ExcelPackage();

            ExcelWorksheet worksheet = xlsPack.Workbook.Worksheets.Add("Report");
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml
                .Style
                .ExcelHorizontalAlignment
                .Center;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml
                .Style
                .ExcelVerticalAlignment
                .Center;

            worksheet.Column(1).Width = 5.69; //Width = 5.00 in excel
            worksheet.Column(2).Width = 32.59; //Width = 31.86 in excel
            worksheet.Column(3).Width = 26.23; //Width = 25.57 in excel
            worksheet.Column(4).Width = 28.69; //Width = 28.00 in excel
            worksheet.Column(5).Width = 13.49; //Width = 12.71 in excel
            worksheet.Column(6).Width = 11.29; //Width = 10.57 in excel
            worksheet.Column(7).Width = 14.69; //Width = 14.00 in excel
            worksheet.Column(8).Width = 11.29; //Width = 10.57 in excel

            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;
            worksheet.Column(6).Style.WrapText = true;
            worksheet.Column(7).Style.WrapText = true;
            worksheet.Column(8).Style.WrapText = true;

            worksheet.Cells["A1:H1"].Merge = true;

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, dt);
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;

            worksheet.Cells["A2"].Value = "№";
            worksheet.Cells["A2"].Style.Font.Bold = true;

            worksheet.Cells["B2"].Value = "Ф.И.О";
            worksheet.Cells["B2"].Style.Font.Bold = true;

            worksheet.Cells["C2"].Value = "Отдел";
            worksheet.Cells["C2"].Style.Font.Bold = true;

            worksheet.Cells["D2"].Value = "Должность";
            worksheet.Cells["D2"].Style.Font.Bold = true;

            worksheet.Cells["E2"].Value = "Дата приема";
            worksheet.Cells["E2"].Style.Font.Bold = true;

            worksheet.Cells["F2"].Value = "Средний показатель";
            worksheet.Cells["F2"].Style.Font.Bold = true;

            worksheet.Cells["G2"].Value = "Посещаемость и опоздания";
            worksheet.Cells["G2"].Style.Font.Bold = true;

            worksheet.Cells["H2"].Value = "Сводный показатель";
            worksheet.Cells["H2"].Style.Font.Bold = true;

            for (int k = 1; k <= 2; k++)
            {
                BorderLinesForReportByCompany(worksheet, k);
            }

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dt);
            int rowStart = 3;
            int i = 1;
            int j = 3;
            foreach (Employee employee in employees)
            {
                BorderLinesForReportByCompany(worksheet, j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"C{rowStart}"].Value = employee.Department.Name;
                worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"D{rowStart}"].Value = employee.Position.Name;
                worksheet.Cells[$"D{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"E{rowStart}"].Value = employee.AdoptionDate;
                worksheet.Cells[$"E{rowStart}"].Style.Numberformat.Format = "dd.mm.yyyy";
                worksheet.Cells[$"F{rowStart}"].Value = employee.TaskLists.Select(t => t.AverageMark).Average() + "%";

                rowStart++;
                j++;
            }
            return xlsPack;
        }
        public void BorderLinesForReportByCompany(ExcelWorksheet worksheet, int a)
        {
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        public ExcelPackage ReportByDepartmentAverageMark()
        {
            string ReportTitle = "Отчет средних показателей по отделам";

            ExcelPackage xlsPack = new ExcelPackage();

            ExcelWorksheet worksheet = xlsPack.Workbook.Worksheets.Add("Report");
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml
                .Style
                .ExcelHorizontalAlignment
                .Center;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml
                .Style
                .ExcelVerticalAlignment
                .Center;

            worksheet.Column(1).Width = 5.69; //Width = 5.00 in excel
            worksheet.Column(2).Width = 53.00; //Width = 52.29 in excel
            worksheet.Column(3).Width = 8.84; //Width = 8.14 in excel
            worksheet.Column(4).Width = 20.74; //Width = 20.00 in excel

            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;

            worksheet.Cells["A1:D1"].Merge = true;

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, dt);
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;

            worksheet.Cells["A2"].Value = "№";
            worksheet.Cells["A2"].Style.Font.Bold = true;

            worksheet.Cells["B2"].Value = "Отдел";
            worksheet.Cells["B2"].Style.Font.Bold = true;

            worksheet.Cells["C2"].Value = "Наличие оценки";
            worksheet.Cells["C2"].Style.Font.Bold = true;

            worksheet.Cells["D2"].Value = "Средние показатели по отделу";
            worksheet.Cells["D2"].Style.Font.Bold = true;

            for (int k = 1; k <= 2; k++)
            {
                BorderLinesForReportByDepartmentAverageMark(worksheet, k);
            }

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dt);
            int rowStart = 3;
            int i = 1;
            int j = 3;
            foreach (Employee employee in employees)
            {
                BorderLinesForReportByDepartmentAverageMark(worksheet, j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.Department.Name;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"C{rowStart}"].Value = employee.EFullName;
                //worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"D{rowStart}"].Value = employee.Position.Name;
                //worksheet.Cells[$"D{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                rowStart++;
                j++;
            }
            return xlsPack;
        }
        public void BorderLinesForReportByDepartmentAverageMark(ExcelWorksheet worksheet, int a)
        {
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        public ExcelPackage ReportByConsolidated()
        {
            string ReportTitle = "Консолидированный анализ по оценке эффективности";

            ExcelPackage xlsPack = new ExcelPackage();

            ExcelWorksheet worksheet = xlsPack.Workbook.Worksheets.Add("Report");
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml
                .Style
                .ExcelHorizontalAlignment
                .Center;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml
                .Style
                .ExcelVerticalAlignment
                .Center;

            worksheet.Column(1).Width = 5.69; //Width = 5.00 in excel
            worksheet.Column(2).Width = 29.84; //Width = 29.14 in excel
            worksheet.Column(3).Width = 8.84; //Width = 8.14 in excel
            worksheet.Column(4).Width = 13.64; //Width = 12.86 in excel
            worksheet.Column(5).Width = 13.69; //Width = 13.00 in excel
            worksheet.Column(6).Width = 16.54; //Width = 15.86 in excel

            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;
            worksheet.Column(5).Style.WrapText = true;
            worksheet.Column(6).Style.WrapText = true;

            worksheet.Cells["A1:F1"].Merge = true;

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, dt);
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 14;

            worksheet.Cells["A2"].Value = "№";
            worksheet.Cells["A2"].Style.Font.Bold = true;

            worksheet.Cells["B2"].Value = "Отдел";
            worksheet.Cells["B2"].Style.Font.Bold = true;

            worksheet.Cells["C2"].Value = "Наличие оценки";
            worksheet.Cells["C2"].Style.Font.Bold = true;

            worksheet.Cells["D2"].Value = "Средние показатели по отделу";
            worksheet.Cells["D2"].Style.Font.Bold = true;

            worksheet.Cells["E2"].Value = "Показатели по Протоколу";
            worksheet.Cells["E2"].Style.Font.Bold = true;

            worksheet.Cells["F2"].Value = "Общие показатели в % соотношении по выполнению";
            worksheet.Cells["F2"].Style.Font.Bold = true;


            for (int k = 1; k <= 2; k++)
            {
                BorderLinesForReportByConsolidated(worksheet, k);
            }

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dt);
            int rowStart = 3;
            int i = 1;
            int j = 3;
            foreach (Employee employee in employees)
            {
                BorderLinesForReportByConsolidated(worksheet, j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                //worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                //worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"C{rowStart}"].Value = employee.Department.Name;
                //worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"D{rowStart}"].Value = employee.Position.Name;
                //worksheet.Cells[$"D{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"E{rowStart}"].Value = employee.AdoptionDate;
                //worksheet.Cells[$"E{rowStart}"].Style.Numberformat.Format = "dd.mm.yyyy";
                //worksheet.Cells[$"F{rowStart}"].Value = employee.TaskLists.Select(t => t.AverageMark).Average() + "%";

                rowStart++;
                j++;
            }
            return xlsPack;
        }
        public void BorderLinesForReportByConsolidated(ExcelWorksheet worksheet, int a)
        {
            worksheet.Cells[$"A{a}:F{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:F{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:F{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:F{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        public ExcelPackage ReportByInstructionsDG()
        {
            string ReportTitle = "Оценка выполнения поручений Генерального Директора по отчету о проделанной работе";

            ExcelPackage xlsPack = new ExcelPackage();

            ExcelWorksheet worksheet = xlsPack.Workbook.Worksheets.Add("Report");
            worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml
                .Style
                .ExcelHorizontalAlignment
                .Center;
            worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml
                .Style
                .ExcelVerticalAlignment
                .Center;

            string departmentName = unit
                .Departments
                .GetDepartmentNameById((Guid)departmentId);

            worksheet.Column(1).Width = 5.69; //Width = 5.00 in excel
            worksheet.Column(2).Width = 42.69; //Width = 42.00 in excel
            worksheet.Column(3).Width = 8.84; //Width = 8.14 in excel
            worksheet.Column(4).Width = 31.00; //Width = 30.29 in excel

            worksheet.Column(1).Style.WrapText = true;
            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;

            worksheet.Cells["A1:D1"].Merge = true;

            worksheet.Row(1).Height = 39.75;
            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, dt);
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 16;

            worksheet.Cells["A2:D2"].Merge = true;
            worksheet.Cells["A2"].Value = departmentName;
            worksheet.Cells["A2"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Style.Font.Size = 14;

            worksheet.Cells["A3:A4"].Merge = true;
            worksheet.Cells["A3"].Value = "№";
            worksheet.Cells["A3"].Style.Font.Bold = true;

            worksheet.Cells["B3:B4"].Merge = true;
            worksheet.Cells["B3"].Value = "Поручение";
            worksheet.Cells["B3"].Style.Font.Bold = true;

            worksheet.Cells["C3:D3"].Merge = true;
            worksheet.Cells["C3"].Value = "Выполнение";
            worksheet.Cells["C3"].Style.Font.Bold = true;

            worksheet.Cells["C4"].Value = "%";
            worksheet.Cells["C4"].Style.Font.Bold = true;

            worksheet.Cells["D4"].Value = "Комментарии";
            worksheet.Cells["D4"].Style.Font.Bold = true;

            for (int k = 1; k <= 4; k++)
            {
                BorderLinesForReportByInstructionsDG(worksheet, k);
            }

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dt);
           // IEnumerable<Employee> employees = unit.Employees.GetEmployeesByDepartmentId((Guid)departmentId, dt);
            int rowStart = 5;
            int i = 1;
            int j = 5;
            foreach (Employee employee in employees)
            {
                BorderLinesForReportByInstructionsDG(worksheet, j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                //worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                //worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"C{rowStart}"].Value = employee.Position.Name;
                //worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                //worksheet.Cells[$"D{rowStart}"].Value = employee.TaskLists.Select(t => t.AverageMark).Average() + "%";
                rowStart++;
                j++;
            }

            return xlsPack;
        }
        public void BorderLinesForReportByInstructionsDG(ExcelWorksheet worksheet, int a)
        {
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:D{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }
    }
}