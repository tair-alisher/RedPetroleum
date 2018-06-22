using System;

namespace RedPetroleum.Models.Entities
{
    public class TaskList
    {
        public Guid TaskListId { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department Departments { get; set; }

        public string TaskName { get; set; }

        public string TaskDuration { get; set; }

        public DateTime? TaskDate { get; set; }

        public string CommentEmployees { get; set; }

        public double? Mark { get; set; }   
    }
}