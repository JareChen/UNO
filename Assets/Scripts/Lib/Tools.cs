using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : SingletenMono<Tools>
{
    public Canvas canvas;
    
    public Vector3 ScreenToWorldPosition(Vector2 screenPos)
    {
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out worldPos);
        return worldPos;
    }
}
