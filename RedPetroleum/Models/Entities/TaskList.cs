using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RedPetroleum.Models.Entities
{
    public class TaskList
    {
        public Guid TaskListId { get; set; }

        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }

        [Display(Name = "Наименование задачи")]
        public string TaskName { get; set; }

        [Display(Name = "Продолжительность задачи")]
        public string TaskDuration { get; set; }

        [Display(Name = "Дата задачи")]
        public DateTime? TaskDate { get; set; }

        [Display(Name = "Комментарий сотрудника")]
        public string CommentEmployees { get; set; }

        public double? SkillMark { get; set; }
        public double? EffectivenessMark { get; set; }
        public double? DisciplineMark { get; set; }
        public double? TimelinessMark { get; set; }
        public double? AverageMark { get; set; }
    }
}