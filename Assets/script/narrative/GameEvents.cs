using System;
using System.Collections.Generic;

public static class GameEvents
{
    public static event Action<string> OnInteractionComplete;
    private static Queue<string> pendingEvents = new Queue<string>();
    private static bool isListening = false;

    public static void TriggerInteractionComplete(string id)
    {
        UnityEngine.Debug.Log($"[GameEvents] ¥•∑¢ID: {id}");
        pendingEvents.Enqueue(id);
    }

    public static void StartListening()
    {
        isListening = true;
        ProcessPendingEvents();
    }

    public static void StopListening()
    {
        isListening = false;
    }

    public static void ProcessPendingEvents()
    {
        if (!isListening) return;

        while (pendingEvents.Count > 0)
        {
            string id = pendingEvents.Dequeue();
            UnityEngine.Debug.Log($"[GameEvents] ¥¶¿ÌID: {id}");
            OnInteractionComplete?.Invoke(id);
        }
    }
}