using RedPetroleum.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models
{
    public class TaskList
    {
        public Guid TaskListId { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string TaskName { get; set; }

        public string TaskDuration { get; set; }

        public string CommentEmployer { get; set; }

        public string CommentEmployees { get; set; }

        public double Mark { get; set; }   
    }
}