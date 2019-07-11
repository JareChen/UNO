using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public Action onClick;
    public Action onDoubleClick;
    public Action onLongClick;
    public Action onPointDown;
    public Action<PointerEventData> onDrag;
    public Action<PointerEventData> onEndDrag;

    public float interval = 0.3f;
    public float timer = 0.0f;
    public float durationThreshold = 1.0f;

    private float timePressStarted;
    private bool first = false;
    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private bool dragTriggered = false;

    private void Update()
    {
        if (first)
        {
            timer += Time.deltaTime;
            if (timer > interval && !longPressTriggered && !dragTriggered)
            {
                onClick?.Invoke();
                timer = 0;
                first = false;
            }
        }
        if (!longPressTriggered && isPointerDown && (Time.time - timePressStarted) > durationThreshold && !dragTriggered)
        {
            longPressTriggered = true;
            onLongClick?.Invoke();
        }
    }

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (first)
        {
            onDoubleClick?.Invoke();
            first = false;
            timer = 0;
            return;
        }
        if (!longPressTriggered && !dragTriggered)
        {
            first = true;
        }
        else
        {
            first = false;
            timer = 0;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        onPointDown?.Invoke();
        timePressStarted = Time.time;
        isPointerDown = true;
        dragTriggered = false;
        longPressTriggered = false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        dragTriggered = true;
        onDrag?.Invoke(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke(eventData);
    }
}