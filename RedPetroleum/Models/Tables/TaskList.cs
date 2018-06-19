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

        public Guid ETaskId { get; set; }
        public ETask ETask { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string CommentEmployer { get; set; }

        public string CommentEmployees { get; set; }

        public double Mark { get; set; }

        public bool Done { get; set; }     
    }
}