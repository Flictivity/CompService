using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CompService.Core.Enums;

namespace CompService.Core.Extensions;

public static class RolesExtensions
{
    public static string? GetName(this Role role)
    {
        return role.GetType().GetField(role.ToString()).GetCustomAttribute<DisplayAttribute>()?.Name;
    }
}