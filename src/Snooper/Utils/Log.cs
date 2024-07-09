namespace Snooper.Utils
{
    using UnityEngine;

    public class Log
    {
        public const string TAG = "[Snooper] ";
        public static void Msg(string msg) => Debug.Log(TAG + msg);
        public static void Warn(string msg) => Debug.LogWarning(TAG + msg);
        public static void Err(string msg) => Debug.LogError(TAG + msg);
    }
}
