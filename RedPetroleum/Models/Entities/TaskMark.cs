using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class TaskMark
    {
        public Guid Id { get; set; }
        public Guid MarkId { get; set; }
        public Guid TaskListId { get; set; }   
        public double MarkValue { get; set; }

        [ForeignKey("Id")]
        public Mark Mark { get; set; }

        [ForeignKey("TaskListId")]
        public TaskList TaskList { get; set; }
    }
}