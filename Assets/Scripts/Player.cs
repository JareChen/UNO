using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public bool isPush;
    public Transform trans;
    public List<CardComponent> cardList = new List<CardComponent>();
    public void AddCard(CardComponent cardComponent)
    {
        cardList.Add(cardComponent);
    }

    public void PushCard(CardComponent cardComponent)
    {
        isPush = true;
        cardList.Remove(cardComponent);
    }
}
