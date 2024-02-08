using System.ComponentModel;

namespace WebAPI.Helpers
{
    public static class Enumerators
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
                return "";

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])value.GetType().GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
