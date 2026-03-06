using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    private GameObject cube;

    void Start()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(3, 1, 8);
        cube.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Debug.Log("立方体已生成");
    }

    void OnDisable()
    {
        if (cube != null)
        {
            cube.SetActive(false);
            Debug.Log("立方体已隐藏");
        }
    }

    void OnEnable()
    {
        if (cube != null)
        {
            cube.SetActive(true);
            Debug.Log("立方体已显示");
        }
    }
}