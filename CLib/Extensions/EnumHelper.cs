using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class EnumHelper
{
    public static T ToEnum<T>(this string s) where T : struct
    {
        return Enum.TryParse(s, out T newValue) ? newValue : default;
    }

    public static int ToEnumInt<T>(this string s) where T : struct
    {
        return Convert.ToInt32(Enum.TryParse(s, out T newValue) ? newValue : default);
    }

    public static bool ToEnumBool<T>(this string s) where T : struct
    {
        return Convert.ToInt32(Enum.TryParse(s, out T newValue) ? newValue : default) == 1;
    }

    public static T ToEnum<T>(this bool b) where T : Enum
    {
        return (T)(object)(b ? 1 : 0);
    }

    //public static bool ToBool(this Enums.Logic logic)
    //{
    //    return logic.Equals(Logic.B);
    //}

}

