using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioEngineerLab.Core
{
    public static class ExtensionMethods
    {
        public static string GetEnumName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}