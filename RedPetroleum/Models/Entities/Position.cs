using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Position
    {
        public Guid PositionId { get; set; }

        [Display(Name = "Наименование должности")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я -]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }        
    }
}