using UnityEngine;

public class GlowBlink : MonoBehaviour
{
    [Header("发光设置")]
    public Color glowColor = Color.yellow;
    public float minIntensity = 0f;
    public float maxIntensity = 2f;
    public float blinkSpeed = 2f;

    private Material material;
    private bool isBlinking = false;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        // 开启发光
        material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (!isBlinking) return;

        // 用sin波让亮度平滑闪烁
        float intensity = Mathf.Lerp(minIntensity, maxIntensity,
            (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);

        material.SetColor("_EmissionColor", glowColor * intensity);
    }

    public void StartBlink()
    {
        isBlinking = true;
        Debug.Log($"[GlowBlink] 开始闪烁");
    }

    public void StopBlink()
    {
        isBlinking = false;
        material.SetColor("_EmissionColor", Color.black);
        Debug.Log($"[GlowBlink] 停止闪烁");
    }
}