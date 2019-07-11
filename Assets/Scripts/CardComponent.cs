using UnityEngine;
using UnityEngine.EventSystems;

public class CardComponent : MonoBehaviour
{
    public int offsetY = 20;

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
        Debug.LogError("Double Click");
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
        transform.localPosition = originPos;
        isSelect = false;
    }

    public void OnPointDown()
    {
        if (!isSelect)
        {
            originPos = transform.localPosition;
        }
    }
}