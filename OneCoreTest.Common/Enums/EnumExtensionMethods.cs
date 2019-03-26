using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Common.Enums
{
    public static class EnumExtensionMethods
    {
        public static string ToName(this Enum enumValue)
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0
                ? descriptionAttributes[0].Description
                : enumValue.ToString();
        }
    }
}
