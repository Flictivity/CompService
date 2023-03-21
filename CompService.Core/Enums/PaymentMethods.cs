using System.ComponentModel.DataAnnotations;

namespace CompService.Core.Enums;

public enum PaymentMethods
{
    [Display(Name = "Наличные")] Cash = 0,
    [Display(Name = "Карта")] Card = 1
}