using UnityEngine;

public static class DebugLogHelper
{
    public static void Log(string message, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        var className = new System.Diagnostics.StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name ?? "Unknown";
        Debug.Log($"[{className}.{methodName}] {message}");
    }
    
    public static void LogWarning(string message, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        var className = new System.Diagnostics.StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name ?? "Unknown";
        Debug.LogWarning($"[{className}.{methodName}] {message}");
    }
    
    public static void LogError(string message, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        var className = new System.Diagnostics.StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name ?? "Unknown";
        Debug.LogError($"[{className}.{methodName}] {message}");
    }
    
    
    
    // public static void Log(string message, object caller = null, string methodName = "")
    // {
    //     string callerName = caller?.GetType().Name ?? "UnknownCaller";
    //     string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";
    //
    //     if(caller != null)
    //         Debug.Log($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    //     else
    //         Debug.Log($"{message}");
    // }
    //
    // public static void LogWarning(string message, object caller = null, string methodName = "")
    // {
    //     string callerName = caller?.GetType().Name ?? "UnknownCaller";
    //     string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";
    //
    //     if(caller != null)
    //         Debug.LogWarning($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    //     else
    //         Debug.LogWarning($"{message}");
    // }
    //
    // public static void LogError(string message, object caller = null, string methodName = "")
    // {
    //     string callerName = caller?.GetType().Name ?? "UnknownCaller";
    //     string methodPart = string.IsNullOrEmpty(methodName) ? "" : $" -> {methodName}";
    //
    //     if(caller != null)
    //         Debug.LogError($"{message}\n\ncallstack\n\n{callerName}{methodPart}");
    //     else
    //         Debug.LogError($"{message}");
    // }
}