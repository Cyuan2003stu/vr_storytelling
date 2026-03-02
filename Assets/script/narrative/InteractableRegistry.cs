using System.Collections.Generic;
using UnityEngine;

public class InteractableRegistry : MonoBehaviour
{
    public static InteractableRegistry Instance;
    private Dictionary<string, GameObject> registry = new();

    void Awake() => Instance = this;

    public static void Register(string id, GameObject obj)
        => Instance.registry[id] = obj;

    public static void SetActive(string id, bool active)
    {
        if (Instance.registry.TryGetValue(id, out var obj))
            obj.SetActive(active);
    }
}