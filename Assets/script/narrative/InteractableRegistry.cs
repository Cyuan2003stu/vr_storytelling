using System.Collections.Generic;
using UnityEngine;

public class InteractableRegistry : MonoBehaviour
{
    private static InteractableRegistry _instance;
    private Dictionary<string, GameObject> registry;

    public static InteractableRegistry Instance
    {
        get
        {
            // 如果没有实例，自动在场景里创建一个
            if (_instance == null)
            {
                GameObject obj = new GameObject("InteractableRegistry");
                _instance = obj.AddComponent<InteractableRegistry>();
                _instance.registry = new Dictionary<string, GameObject>();
                Debug.Log("InteractableRegistry 自动创建");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            registry = new Dictionary<string, GameObject>();
        }
    }

    public static void Register(string id, GameObject obj)
    {
        Debug.Log($"注册: {id}");
        Instance.registry[id] = obj;
    }

    public static void SetActive(string id, bool active)
    {
        if (Instance.registry.TryGetValue(id, out var obj))
            obj.SetActive(active);
        else
            Debug.LogWarning($"找不到ID: {id}");
    }
}