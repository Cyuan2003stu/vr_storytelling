using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [Serializable]
    public class TriggerEvent
    {
        public float triggerTime;
        public UnityEngine.Events.UnityEvent action;
        [HideInInspector] public bool triggered = false;
    }

    public TriggerEvent[] triggers;
    private PlayableDirector director;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (director == null) return;
        if (director.state != PlayState.Playing) return;

        foreach (var t in triggers)
        {
            if (!t.triggered && director.time >= t.triggerTime)
            {
                t.triggered = true;
                Debug.Log($"[TimelineTrigger] 触发时间点: {t.triggerTime}s");
                t.action.Invoke();
            }
        }
    }

    public void ResetTriggers()
    {
        foreach (var t in triggers)
            t.triggered = false;
    }
}