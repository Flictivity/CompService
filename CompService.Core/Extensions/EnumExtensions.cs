using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CompService.Core.Enums;

namespace CompService.Core.Extensions;

public static class EnumExtensions
{
    public static string? GetName(this Enum @enum)
    {
        return @enum.GetType().GetField(@enum.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name;
    }
}