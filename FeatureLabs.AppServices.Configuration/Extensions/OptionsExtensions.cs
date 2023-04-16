using System.Reflection;
using FeatureLabs.AppServices.Configuration.Attributes;

namespace FeatureLabs.AppServices.Configuration.Extensions
{
    public static class OptionsExtensions
    {
        public static string? TryGetPosition(this Type type)
        {
            var attribute = type.GetCustomAttribute<OptionsPositionAttribute>();
            return attribute?.Position;
        }
    }
}
