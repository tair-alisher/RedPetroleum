using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Tables
{
    public class Position
    {
        public Guid PositionId { get; set; }

        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
        
    }
}