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
        Guid departmentId;
        
        public XlsReport(UnitOfWork unit, Guid departmentId)
        {
            this.unit = unit;
            this.departmentId = departmentId;
        }

        public ExcelPackage FormReport()
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
                .GetDepartmentNameById(departmentId);

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

            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByDepartmentId(departmentId);
            int rowStart = 4;
            int i = 1;
            foreach (Employee employee in employees)
            {
                worksheet.Cells[$"C{i}"].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                worksheet.Cells[$"A{rowStart}"].Value = i++;
                worksheet.Cells[$"B{rowStart}"].Value = employee.EFullName;
                worksheet.Cells[$"B{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[$"C{rowStart}"].Value = employee.Position.Name;
                worksheet.Cells[$"C{rowStart}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                rowStart++;            
            }
            return xlsPack;
        }
    }
}