using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<Card> cardList = new List<Card>();
    public void AddCard(Card card)
    {
        cardList.Add(card);
    }

    public void PushCard(Card card)
    {

    }
}
