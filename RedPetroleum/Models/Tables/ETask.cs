using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Tables
{
    public class ETask
    {
        public Guid ETaskId { get; set; }

        public string Name { get; set; }

        public ICollection<TaskList> TaskLists { get; set; }
    }
}