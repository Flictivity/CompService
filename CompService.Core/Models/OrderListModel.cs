﻿namespace CompService.Core.Models;

public class OrderListModel<T> where T : class
{
    public T Item { get; set; } = null!;
    public int ItemCount {get;set;}
    public double Discount { get; set; }
    public double Sum { get; set; }
}
