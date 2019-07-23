using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour
{
    public int offsetY = 20;
    public CardConfig config;
    private bool isSelect;
    private Vector3 originPos;
    private EventTriggerListener listener;

    public void Start()
    {
        listener = EventTriggerListener.Get(gameObject);
        listener.onClick = OnClick;
        listener.onDoubleClick = OnDoubleClick;
        listener.onLongClick = OnLongClick;
        listener.onPointDown = OnPointDown;
        listener.onDrag = OnDrag;
        listener.onEndDrag = OnEndDrag;
    }

    public void OnClick()
    {
        if (isSelect)
        {
            isSelect = false;
            transform.localPosition = originPos;
        }
        else
        {
            isSelect = true;
            transform.localPosition = new Vector3(originPos.x, originPos.y + offsetY, originPos.z);
        }
        Debug.LogError("Click");
    }

    public void OnDoubleClick()
    {
        bool pushResult = GamePlayController.Instance.TryPushCard(this);
        if (pushResult)
        {

        }
    }

    public void OnLongClick()
    {
        Debug.LogError("Long Click");
    }

    public void OnDrag(PointerEventData eventData)
    {
        var drawGo = eventData.pointerDrag;
        drawGo.transform.position = Tools.Instance.ScreenToWorldPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool pushResult = GamePlayController.Instance.TryPushCard(this);
        if (pushResult)
        {
            
        }
        else
        {
            transform.localPosition = originPos;
            isSelect = false;
        }
    }

    public void OnPointDown()
    {
        if (!isSelect)
        {
            originPos = transform.localPosition;
        }
    }

    public void Show()
    {
        GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(config.ResName);
    }
}