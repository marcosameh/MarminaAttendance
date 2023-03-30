using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Utilities
{
    public static class EnumUtilities
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue != null)
            {
                string displayName;

                displayName = enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .FirstOrDefault()
                    .GetCustomAttribute<DisplayAttribute>()?
                    .GetName();
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = enumValue.ToString();
                }

                return displayName;
            }
            return "";
        }
    }
}
