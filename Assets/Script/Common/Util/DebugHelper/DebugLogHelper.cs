using UnityEngine;

public static class DebugLogHelper
{
    public static void Log(string message, object caller = null, string methodName = "")
    {
        string callerName = caller?.GetType().Name ?? "UnknownCaller";
        string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";

        Debug.Log($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    }

    public static void LogWarning(string message, object caller = null, string methodName = "")
    {
        string callerName = caller?.GetType().Name ?? "UnknownCaller";
        string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";

        Debug.LogWarning($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    }

    public static void LogError(string message, object caller = null, string methodName = "")
    {
        string callerName = caller?.GetType().Name ?? "UnknownCaller";
        string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";

        Debug.LogError($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    }
}