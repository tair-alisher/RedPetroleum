using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using RedPetroleum.Models;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using RedPetroleum.Services;

namespace RedPetroleum.Controllers
{
    [Authorize]
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

        [HttpGet]
        public ActionResult ReportByDepartment(string departmentId, DateTime? dateValue)
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
                if (departmentId != null)
                {
                    var employeeList = new List<ReportByCompanyViewModel>();

                    ReportByCompanyViewModel model;
                    foreach (Employee employee in emplist)
                    {
                        model = new ReportByCompanyViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeName = employee.EFullName,
                            Position = employee.Position.Name,
                            AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                        };

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
                if (departmentId != null)
                {
                    var employeeList = new List<ReportByCompanyViewModel>();

                    ReportByCompanyViewModel model;
                    foreach (Employee employee in emplist)
                    {
                        model = new ReportByCompanyViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeName = employee.EFullName,
                            Position = employee.Position.Name,
                            AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                        };

                        employeeList.Add(model);
                    }
                    return View(employeeList);
                }
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByDepartment(string departmentId, DateTime? dateValue)
        {
            if (departmentId == "")
            {
                return PartialView();
            }
            IEnumerable<Employee> employees = unit.Employees
                .GetEmployeesByDepartmentId(Guid.Parse(departmentId), dateValue);
            var empList = new List<ReportByCompanyViewModel>();
            ReportByCompanyViewModel model;
            foreach (var item in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = item.EmployeeId,
                    EmployeeName = item.EFullName,
                    Position = item.Position.Name,
                    AverageMark = item.TaskLists.Select(t => t.AverageMark).Average()
                };

                empList.Add(model);
            }
            return PartialView(empList);
        }

        [HttpGet]
        public ActionResult ReportByCompany(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return View(employeeList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByCompany(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return PartialView(employeeList);
        }

        [HttpGet]
        public ActionResult ReportByDepartmentAverageMark()
        {
            var firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDate.AddMonths(1).AddDays(-1);
            DateTime[] dateValues = new DateTime[] {
                firstDate,
                lastDayOfMonth,
            };
            List<CustomDepartment> DepartmentsWithoutParentAndChildren = new List<CustomDepartment>();
            List<Department> DepsWithoutParentAndChildren = unit.Departments.GetDepartmentsWithoutParentAndChildren().ToList();
            foreach (var department in DepsWithoutParentAndChildren)
            {
                DepartmentsWithoutParentAndChildren.Add(
                    new CustomDepartment
                    {
                        DepartmentId = department.DepartmentId,
                        Name = department.Name,
                        AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(department.DepartmentId, dateValues)
                    }
                    );
            }
            ViewBag.DepartmentsWithoutParentAndChildren = DepartmentsWithoutParentAndChildren;

            List<Department> DepartmentsWithoutParentWithChildren = unit.Departments.GetDepartmentsWithoutParentWithChildren().ToList();
            List<DepartmentsWithChildren> result = new List<DepartmentsWithChildren>();
            DepartmentsWithChildren item = null;
            List<Department> children = null;

            foreach (Department parent in DepartmentsWithoutParentWithChildren)
            {
                item = new DepartmentsWithChildren
                {
                    Department = parent,
                    Children = new List<CustomDepartment>(),
                    AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(parent.DepartmentId, dateValues)
                };
                children = unit.Departments.GetDepartmentsByParentId(parent.DepartmentId).ToList();
                foreach (Department child in children)
                {
                    item.Children.Add(
                        new CustomDepartment
                        {
                            DepartmentId = child.DepartmentId,
                            Name = child.Name,
                            ParentId = child.ParentId,
                            AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(child.DepartmentId, dateValues)
                        }
                        );
                }
                result.Add(item);
            }
            ViewBag.Marks = result;
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByDepartmentAverageMark(string[] dateValue)
        {
            ViewBag.Today = DateTime.Now.Month;
            DateTime[] dateValues = new DateTime[] {
                DateTime.ParseExact(dateValue[0], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(dateValue[1], "MM/dd/yyyy", CultureInfo.InvariantCulture)
           };
            List<CustomDepartment> DepartmentsWithoutParentAndChildren = new List<CustomDepartment>();
            List<Department> DepsWithoutParentAndChildren = unit.Departments.GetDepartmentsWithoutParentAndChildren().ToList();
            foreach (var department in DepsWithoutParentAndChildren)
            {
                DepartmentsWithoutParentAndChildren.Add(
                    new CustomDepartment
                    {
                        DepartmentId = department.DepartmentId,
                        Name = department.Name,
                        AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(department.DepartmentId, dateValues)
                    }
                    );
            }
            ViewBag.DepartmentsWithoutParentAndChildren = DepartmentsWithoutParentAndChildren;

            List<Department> DepartmentsWithoutParentWithChildren = unit.Departments.GetDepartmentsWithoutParentWithChildren().ToList();
            List<DepartmentsWithChildren> result = new List<DepartmentsWithChildren>();
            DepartmentsWithChildren item = null;
            List<Department> children = null;

            foreach (Department parent in DepartmentsWithoutParentWithChildren)
            {
                item = new DepartmentsWithChildren
                {
                    Department = parent,
                    Children = new List<CustomDepartment>(),
                       AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(parent.DepartmentId, dateValues)
                };
                children = unit.Departments.GetDepartmentsByParentId(parent.DepartmentId).ToList();
                foreach (Department child in children)
                {
                    item.Children.Add(
                        new CustomDepartment
                        {
                            DepartmentId = child.DepartmentId,
                            Name = child.Name,
                            ParentId = child.ParentId,
                            AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndTwoDate(child.DepartmentId, dateValues)
                        }
                        );
                }
                result.Add(item);
            }
            ViewBag.Marks = result;
            return PartialView(result);
        }

        [HttpGet]
        public ActionResult ReportByConsolidated(DateTime? dateValue)
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            List<CustomDepartment> DepartmentsWithoutParentAndChildren = new List<CustomDepartment>();
            List<Department> DepsWithoutParentAndChildren = unit.Departments.GetDepartmentsWithoutParentAndChildren().ToList();
            foreach (var department in DepsWithoutParentAndChildren)
            {
                DepartmentsWithoutParentAndChildren.Add(
                    new CustomDepartment
                    {
                        DepartmentId = department.DepartmentId,
                        Name = department.Name,
                        AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(department.DepartmentId, dateValue),
                        AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(department.DepartmentId, dateValue)
                    }
                    );
            }
            ViewBag.DepartmentsWithoutParentAndChildren = DepartmentsWithoutParentAndChildren;

            List<Department> DepartmentsWithoutParentWithChildren = unit.Departments.GetDepartmentsWithoutParentWithChildren().ToList();
            List<DepartmentsWithChildren> result = new List<DepartmentsWithChildren>();
            DepartmentsWithChildren item = null;
            List<Department> children = null;

            foreach (Department parent in DepartmentsWithoutParentWithChildren)
            {
                item = new DepartmentsWithChildren
                {
                    Department = parent,
                    Children = new List<CustomDepartment>(),
                    AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(parent.DepartmentId, dateValue),
                     AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(parent.DepartmentId, dateValue)
                };
                children = unit.Departments.GetDepartmentsByParentId(parent.DepartmentId).ToList();
                foreach (Department child in children)
                {
                    item.Children.Add(
                        new CustomDepartment
                        {
                            DepartmentId = child.DepartmentId,
                            Name = child.Name,
                            ParentId = child.ParentId,
                            AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(child.DepartmentId, dateValue),
                            AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(child.DepartmentId, dateValue)
                        }
                        );
                }
                result.Add(item);
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByConsolidated(DateTime? dateValue)
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            List<CustomDepartment> DepartmentsWithoutParentAndChildren = new List<CustomDepartment>();
            List<Department> DepsWithoutParentAndChildren = unit.Departments.GetDepartmentsWithoutParentAndChildren().ToList();
            foreach (var department in DepsWithoutParentAndChildren)
            {
                DepartmentsWithoutParentAndChildren.Add(
                    new CustomDepartment
                    {
                        DepartmentId = department.DepartmentId,
                        Name = department.Name,
                        AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(department.DepartmentId, dateValue),
                        AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(department.DepartmentId, dateValue)

                    }
                    );
            }
            ViewBag.DepartmentsWithoutParentAndChildren = DepartmentsWithoutParentAndChildren;

            List<Department> DepartmentsWithoutParentWithChildren = unit.Departments.GetDepartmentsWithoutParentWithChildren().ToList();
            List<DepartmentsWithChildren> result = new List<DepartmentsWithChildren>();
            DepartmentsWithChildren item = null;
            List<Department> children = null;

            foreach (Department parent in DepartmentsWithoutParentWithChildren)
            {
                item = new DepartmentsWithChildren
                {
                    Department = parent,
                    Children = new List<CustomDepartment>(),
                    AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(parent.DepartmentId, dateValue),
                    AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(parent.DepartmentId, dateValue)
                };
                children = unit.Departments.GetDepartmentsByParentId(parent.DepartmentId).ToList();
                foreach (Department child in children)
                {
                    item.Children.Add(
                        new CustomDepartment
                        {
                            DepartmentId = child.DepartmentId,
                            Name = child.Name,
                            ParentId = child.ParentId,
                            AverageMark = unit
                                .Employees
                                .GetEmployeesAverageMarkByDepartmentIdAndDate(child.DepartmentId, dateValue),
                            AverageMarkDepartment = unit
                                .Employees
                                .GetDepartmentsAverageMarkByDepartmentIdAndDate(child.DepartmentId, dateValue)
                        }
                        );
                }
                result.Add(item);
            }
            return PartialView(result);
        }

        [HttpGet]
        public ActionResult ReportByInstructionsDG(string departmentId, DateTime? dateValue)
        {
            IEnumerable<TaskList> taskLists = unit.TaskLists.GetTaskListsWithRelations();
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            if (User.IsInRole("admin"))
            {
                ViewBag.Departments = new SelectList(
                         unit.Departments.GetAll(),
                         "DepartmentId",
                         "Name"
                     );
                if (departmentId != null)
                {
                    var taskList = new List<ReportByInstructionsDGViewModel>();

                    ReportByInstructionsDGViewModel model;
                    foreach (TaskList item in taskLists)
                    {
                        model = new ReportByInstructionsDGViewModel
                        {
                            TaskName = item.TaskName,
                            AverageMark = item.AverageMark,
                            CommentEmployees = item.CommentEmployees
                        };

                        taskList.Add(model);
                    }
                    return View(taskList);
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
                if (departmentId != null)
                {
                    var taskList = new List<ReportByInstructionsDGViewModel>();

                    ReportByInstructionsDGViewModel model;
                    foreach (TaskList item in taskLists)
                    {
                        model = new ReportByInstructionsDGViewModel
                        {
                            TaskName = item.TaskName,
                            AverageMark = item.AverageMark,
                            CommentEmployees = item.CommentEmployees
                        };

                        taskList.Add(model);
                    }
                    return View(taskList);
                }
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByInstructionsDG(string departmentId, DateTime? dateValue)
        {
            if (departmentId == "")
            {
                return PartialView();
            }
            IEnumerable<TaskList> taskLists = unit.TaskLists
                .GetTaskListsByDepartmentId(Guid.Parse(departmentId), dateValue);
            var taskList = new List<ReportByInstructionsDGViewModel>();
            ReportByInstructionsDGViewModel model;
            foreach (var item in taskLists)
            {
                model = new ReportByInstructionsDGViewModel
                {
                    TaskName = item.TaskName,
                    AverageMark = item.AverageMark,
                    CommentEmployees = item.CommentEmployees
                };

                taskList.Add(model);
            }
            return PartialView(taskList);
        }

        public void ExportToExcel(string departmentId, string reportType, DateTime dateValue, Guid? parentId)
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
            XlsReport report = new XlsReport(unit, department, reportType, dateValue, parentId);
            ExcelPackage package = report.FormReport();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=ExcelReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }

        public void ExportToExcelDepartmentAverageMark(string departmentId, string reportType, string dateValue, Guid? parentId)
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
            var dates = dateValue.Split(',').ToArray();
            DateTime[] dateValues = new DateTime[] {
                DateTime.ParseExact(dates[0], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(dates[1], "MM/dd/yyyy", CultureInfo.InvariantCulture)
           };

            XlsReport report = new XlsReport(unit, department, reportType, dateValues, parentId);
            ExcelPackage package = report.FormReport();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=ExcelReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }
    }
}