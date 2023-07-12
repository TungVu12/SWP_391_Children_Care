using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ASP_NET_MVC_Ver1.Common
{
    public static class CommonFuntion
    {
        public static string GetDisplayName(this Object value)
        {
            if (value == null)
                return string.Empty;

            var enumType = value.GetType();
            if (!enumType.IsEnum)
                throw new ArgumentException("Value must be of Enum type.");

            var displayAttribute = enumType
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetName() ?? value.ToString();
        }
    }
}
