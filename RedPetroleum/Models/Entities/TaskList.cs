using System;
using System.Collections.Generic;

namespace RedPetroleum.Models.Entities
{
    public class TaskList
    {
        public Guid TaskListId { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }

        public string TaskName { get; set; }

        public string TaskDuration { get; set; }

        public DateTime? TaskDate { get; set; }

        public string CommentEmployees { get; set; }

        public double? SkillMark { get; set; }
        public double? EffectivenessMark { get; set; }
        public double? DisciplineMark { get; set; }
        public double? TimelinessMark { get; set; }
        public double? AverageMark { get; set; }
    }
}