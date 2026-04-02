using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private NarrativeEvent[] events;
    private int currentIndex = 0;

    void Awake() => Instance = this;

    void Start()
    {
        Debug.Log("[GameManager] 启动");
        PlayCurrentEvent();
    }

    void PlayCurrentEvent()
    {
        if (currentIndex >= events.Length)
        {
            Debug.Log("[GameManager] 游戏结束");
            return;
        }
        Debug.Log($"[GameManager] 播放第 {currentIndex} 个Event");
        events[currentIndex].Begin(OnEventComplete);
    }

    void OnEventComplete()
    {
        Debug.Log($"[GameManager] Event {currentIndex} 完成，切换下一个");
        currentIndex++;
        PlayCurrentEvent();
    }
}