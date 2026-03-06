using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public GameObject targetObject; // ← 改成 GameObject

    public void EnableObject()
    {
        targetObject.SetActive(true);
        Debug.Log("物体已启用");
    }

    public void DisableObject()
    {
        targetObject.SetActive(false);
        Debug.Log("物体已禁用");
    }
}