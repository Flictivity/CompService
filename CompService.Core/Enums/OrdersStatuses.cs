using System.ComponentModel.DataAnnotations;

namespace CompService.Core.Enums;

public enum OrdersStatuses
{
    [Display(Name = "Новый")] NewOrder = 0,
    [Display(Name = "В работе")] InWork = 1,
    [Display(Name = "Согласование")] InAgreement = 2,
    [Display(Name = "Ждет запчасть")] WaitSparePart = 3,
    [Display(Name = "Готов")] Finished = 4,
    [Display(Name = "Закрыт")] Closed = 5,
    [Display(Name = "Закрыт не успешно")] ClosedWithProblems = 6,
}