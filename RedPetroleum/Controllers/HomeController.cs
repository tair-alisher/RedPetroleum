using RedPetroleum.Models;
using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OfficeOpenXml;
using System.Drawing;

namespace RedPetroleum.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            IEnumerable<Employee> emplist = db.Employees;
            return View(emplist);
        }
        public void ExportToExcel()
        {
            IEnumerable<Employee> emplist = db.Employees;

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:E1"].Merge = true;
            ws.Cells["A1"].Value = "ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА - " + string.Format("{0:MMMM yyyy} г.",DateTimeOffset.Now);
            ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["A1"].Style.Font.Bold = true;
            ws.Cells["A1"].Style.Font.Size = 16;

            ws.Cells["A2:E2"].Merge = true;
            ws.Cells["A2"].Value = "Отдел";
            ws.Cells["A2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["A2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["A2"].Style.Font.Bold = true;
            ws.Cells["A2"].Style.Font.Size = 14;

            ws.Cells["A3"].Value = "№";
            ws.Cells["A3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["A3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["A3"].Style.Font.Bold = true;
            ws.Column(1).Width = 5.69;

            ws.Cells["B3"].Value = "Ф.И.О";
            ws.Cells["B3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["B3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["B3"].Style.Font.Bold = true;
            ws.Column(2).Width = 30.29;
            ws.Column(2).Style.WrapText = true;

            ws.Cells["C3"].Value = "Должность";
            ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["C3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["C3"].Style.Font.Bold = true;
            ws.Column(3).Width = 25.00;
            ws.Column(3).Style.WrapText = true;

            ws.Cells["D3"].Value = "Средний показатель";
            ws.Cells["D3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["D3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["D3"].Style.Font.Bold = true;
            ws.Column(4).Width = 10.29;
            ws.Column(4).Style.WrapText = true;

            ws.Cells["E3"].Value = "Подпись";
            ws.Cells["E3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells["E3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            ws.Cells["E3"].Style.Font.Bold = true;
            ws.Column(5).Width = 14.00;

            int rowStart = 4;
            int i = 1;
            foreach (var item in emplist)
            {
                ws.Row(rowStart).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                ws.Cells[string.Format("A{0}", rowStart)].Value = i++;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EFullName;
                rowStart++;
            }         
         
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition","attachment: filename="+"ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }   
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}