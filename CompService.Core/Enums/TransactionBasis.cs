using System.ComponentModel.DataAnnotations;

namespace CompService.Core.Enums;

public enum TransactionBasis
{
    [Display(Name = "Заказ")] Order = 0,
    [Display(Name = "Возврат заказа")] OrderRefund = 1,
    [Display(Name = "Выплата ЗП")] SalaryPayment = 2,
    [Display(Name = "Оплата аренды")] Rent = 3,
}