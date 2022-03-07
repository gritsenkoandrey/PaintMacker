using UnityEngine;

namespace Utils
{
    public static class CustomDebug
    {
        public static void Log(object value)
        {
            Debug.Log(value);
        }

        public static void LogWarning(object value)
        {
            Debug.LogWarning(value);
        }

        public static void LogError(object value)
        {
            Debug.LogError(value);
        }
    }
}