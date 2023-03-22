using System.ComponentModel.DataAnnotations;

namespace CompService.Core.Enums;

public enum RepairTypes
{
    [Display(Name = "Гарантийный ремонт")] Warranty = 0,
    [Display(Name = "Не гарантийный ремонт")] Default = 1,
}