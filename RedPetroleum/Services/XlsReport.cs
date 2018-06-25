using System;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;

namespace RedPetroleum.Services
{
    public class XlsReport
    {
        private readonly string ReportTitle = "ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА";

        UnitOfWork unit;
        Guid? departmentId;
        string reportType;
        public XlsReport(UnitOfWork unit, Guid? departmentId, string reportType)
        {
            this.unit = unit;
            this.departmentId = departmentId;
            this.reportType = reportType;
        }

        public ExcelPackage FormReport()
        {
            if (this.reportType == "ReportByDepartment")
                return FormFirstReport();
            else if (this.reportType == "ReportByCompany")
                return FormSecondReport();
            else
                return FormFirstReport();
        }

        public ExcelPackage FormFirstReport()
        {
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

            worksheet.Column(1).Width = 5.69;
            worksheet.Column(2).Width = 31.00;
            worksheet.Column(3).Width = 25.69;
            worksheet.Column(4).Width = 11.00;
            worksheet.Column(5).Width = 14.69;

            worksheet.Column(2).Style.WrapText = true;
            worksheet.Column(3).Style.WrapText = true;
            worksheet.Column(4).Style.WrapText = true;

            worksheet.Cells["A1:E1"].Merge = true;

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, DateTimeOffset.Now);
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
                BorderLines(worksheet,k);
            }
            

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByDepartmentId((Guid)departmentId);
            int rowStart = 4;
            int i = 1;
            int j = 4;
            foreach (Employee employee in employees)
            {
                BorderLines(worksheet,j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"C{rowStart}"].Value = employee.Position.Name;
                worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                rowStart++;
                j++;
            }

            return xlsPack;
        }
        public void BorderLines(ExcelWorksheet worksheet, int a)
        {          
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:E{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        public ExcelPackage FormSecondReport()
        {
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

            worksheet.Cells["A1"].Value = String.Format("{0} - {1:MMMM yyyy} г.", ReportTitle, DateTimeOffset.Now);
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
                BorderLines2(worksheet, k);
            }


            IEnumerable<Employee> employees = unit.Employees.GetDepartment();
            int rowStart = 3;
            int i = 1;
            int j = 3;
            foreach (Employee employee in employees)
            {
                BorderLines2(worksheet, j);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"C{rowStart}"].Value = employee.Department.Name;
                worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"D{rowStart}"].Value = employee.Position.Name;
                worksheet.Cells[$"D{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"E{rowStart}"].Value = employee.AdoptionDate;
                worksheet.Cells[$"E{rowStart}"].Style.Numberformat.Format = "dd.mm.yyyy";
                rowStart++;
                j++;
            }
            return xlsPack;
        }
        public void BorderLines2(ExcelWorksheet worksheet, int a)
        {
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[$"A{a}:H{a}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }
    }
}