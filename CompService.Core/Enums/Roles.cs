using System.ComponentModel.DataAnnotations;

namespace CompService.Core.Enums;

public enum Roles
{
    [Display(Name = "Администратор")] Administrator = 0,
    [Display(Name = "Мастер")] Master = 1,
    [Display(Name = "Оператор")] Operator = 2
}