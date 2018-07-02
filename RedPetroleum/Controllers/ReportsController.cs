using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using RedPetroleum.Models;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using RedPetroleum.Services;

namespace RedPetroleum.Controllers
{
    public class ReportsController : Controller
    {
        UnitOfWork unit;
        public ReportsController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public ReportsController()
        {
            this.unit = new UnitOfWork();
        }

        public ActionResult ReportByDepartment(string Departments)
        {
            IEnumerable<Employee> emplist = unit.Employees.GetEmployeesWithRelations();
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            if (User.IsInRole("admin"))
            {

                ViewBag.Departments = new SelectList(
                         unit.Departments.GetAll(),
                         "DepartmentId",
                         "Name"
                     );
                if (Departments != null)
                {
                    var employeeList = new List<ReportByCompanyViewModel>();

                    ReportByCompanyViewModel model;
                    foreach (Employee employee in emplist)
                    {
                        model = new ReportByCompanyViewModel();
                        model.EmployeeId = employee.EmployeeId;
                        model.EmployeeName = employee.EFullName;
                        model.Position = employee.Position.Name;
                        model.AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average();

                        employeeList.Add(model);
                    }
                    return View(employeeList);
                }
                return View();
                
            }
            else
            {
                ViewBag.Departments = new SelectList(
                             unit.Departments.GetAvailableDepartments(User.Identity.GetUserId()),
                             "DepartmentId",
                             "Name"
                         );

                var employeeList = new List<ReportByCompanyViewModel>();

                ReportByCompanyViewModel model;
                foreach (Employee employee in emplist)
                {
                    model = new ReportByCompanyViewModel();
                    model.EmployeeId = employee.EmployeeId;
                    model.EmployeeName = employee.EFullName;
                    model.Position = employee.Position.Name;
                    model.AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average();

                    employeeList.Add(model);
                }
                return View(employeeList);
            }
         
          

            ////ViewBag.DepName = unit.Departments.GetDepartmentNameById(ViewBag.Departments);
            //return View();
        }

        public void ExportToExcel(string departmentId, string reportType)
        {
            Guid? department;

            if (departmentId == "*")
            {
               department = null;
            }
            else
            {
                department = Guid.Parse(departmentId);
            }
            XlsReport report = new XlsReport(unit, department, reportType);
            ExcelPackage package = report.FormReport();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=ExcelReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeesByDepartment(string departmentId)
        {
            if (departmentId == "")
            {
                return PartialView();
            }
            IEnumerable<Employee> employees = unit.Employees
                .GetEmployeesByDepartmentId(Guid.Parse(departmentId));
            var empList = new List<ReportByCompanyViewModel>();
            ReportByCompanyViewModel model;
            foreach (var item in employees)
            {
                model = new ReportByCompanyViewModel();
                model.EmployeeId = item.EmployeeId;
                model.EmployeeName = item.EFullName;
                model.Position = item.Position.Name;
                model.AverageMark = item.TaskLists.Select(t => t.AverageMark).Average();

                empList.Add(model);

            }
            return PartialView(empList);
        }

        public ActionResult ReportByCompany()
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(DateTime.Today);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel();
                model.EmployeeId = employee.EmployeeId;
                model.EmployeeName = employee.EFullName;
                model.Department = employee.Department.Name;
                model.Position = employee.Position.Name;
                model.AdoptionDate = employee.AdoptionDate;
                model.AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average();

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return View(employeeList);
        }
    }
}