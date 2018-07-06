using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models
{
    public class ReportByCompanyViewModel
    {
        public Guid EmployeeId;
        public string EmployeeName;
        public string Department;
        public string Position;
        public DateTime AdoptionDate;
        public double? AverageMark;
    }

    public class ReportByInstructionsDGViewModel
    {
        public string Department;
        public string TaskName;
        public double? AverageMark;
        public string CommentEmployees;
    }

    public class DepartmentsWithChildren
    {
        public Department Department;
        public List<CustomDepartment> Children;
    }

    public class CustomDepartment
    {
        public Guid DepartmentId;
        public string Name;
        public Guid? ParentId;
        public double? AverageMark;
    }
}