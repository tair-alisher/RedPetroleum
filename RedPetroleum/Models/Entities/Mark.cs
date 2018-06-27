using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Mark
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<TaskMark> TaskMarks { get; set; }
    }
}