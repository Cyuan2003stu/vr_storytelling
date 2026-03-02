using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private NarrativeEvent[] events; // Inspector里按顺序拖入
    private int currentIndex = 0;

    void Awake() => Instance = this;

    void Start() => PlayCurrentEvent();

    void PlayCurrentEvent()
    {
        if (currentIndex >= events.Length)
        {
            Debug.Log("游戏结束");
            return;
        }
        events[currentIndex].Begin(OnEventComplete);
    }

    void OnEventComplete()
    {
        currentIndex++;
        PlayCurrentEvent();
    }
}