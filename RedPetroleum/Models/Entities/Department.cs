using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Department
    {
        public Guid DepartmentId { get; set; }
        
        [StringLength(200, ErrorMessage = "Длина строки не должна превышать 200 символов")]
        [Display(Name = "Наименование отдела")]
        public string Name { get; set; }

        [Display(Name = "Наименование управляющего отдела")]
        public Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Department Departments { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<TaskList> TaskLists { get; set; }
    }
}