using System;

namespace Core.Utils
{
    public static class ExtensionMethods
    {
        public static string GetEnumName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}