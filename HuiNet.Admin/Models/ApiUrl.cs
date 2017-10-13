using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HuiNet.Admin.Models
{
    public class ApiUrl
    {
        public enum Controller1
        {
            [Description("/Controller1/Action1")]
            Action1,

            [Description("/Controller1/Action2")]
            Action2
        }

        public enum Controller2
        {
            [Description("/Controller2/Action1")]
            Action1,
        }
    }

    public static class EnumExtension
    {
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType(); MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

    }
}