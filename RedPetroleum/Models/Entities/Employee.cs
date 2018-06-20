using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId{ get; set; }

        public string EFullName { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid PositionId { get; set; }
        public Position Position { get; set; }

        public DateTime DateBorn { get; set; }

        public bool Dismissed { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }
    }
}