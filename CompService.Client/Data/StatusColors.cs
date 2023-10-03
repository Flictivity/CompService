using CompService.Core.Enums;

namespace CompService.Client.Data;

public static class StatusColors
{
    public static Dictionary<OrdersStatuses, string> StatusColor = new()
    {
        {OrdersStatuses.Closed, "#919191"},
        {OrdersStatuses.ClosedWithProblems, "#000000"},
        {OrdersStatuses.Finished, "#FF5D5D"},
        {OrdersStatuses.InWork, "#0E9924"},
        {OrdersStatuses.InAgreement, "#FBB123"},
        {OrdersStatuses.WaitSparePart, "#FBB123"},
        {OrdersStatuses.NewOrder, "#4AB7F5"},
    };
}