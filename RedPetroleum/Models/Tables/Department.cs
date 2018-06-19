using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Tables
{
    public class Department
    {
        public Guid DepartmentId { get; set; }

        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Department Departments { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}