using System;

public static class GameEvents
{
    public static event Action<string> OnInteractionComplete;

    public static void TriggerInteractionComplete(string id)
    {
        UnityEngine.Debug.Log($"[GameEvents] ´¥·¢ID: {id}\n{new System.Diagnostics.StackTrace()}");
        OnInteractionComplete?.Invoke(id);
    }
}