using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Main : MonoBehaviour
{
    /// <summary>
    /// 屏幕中心点，发牌点
    /// </summary>
    public Transform centerTrans;

    /// <summary>
    /// 玩家位置
    /// </summary>
    public List<Transform> playerTrans;

    /// <summary>
    /// 牌堆
    /// </summary>
    List<Card> cardPile = new List<Card>();

    /// <summary>
    /// 玩家
    /// </summary>
    List<Player> players = new List<Player>();

    /// <summary>
    /// 玩家数量
    /// </summary>
    int playerCount = 4;

    /// <summary>
    /// 初始化牌数
    /// </summary>
    int initCardCount = 7;

    Player myPlayer;
    Transform myTrans;
    private void Awake()
    {
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<ConfigManager>();
    }

    IEnumerator Start()
    {
        var layoutGroup = playerTrans[0].GetComponent<HorizontalLayoutGroup>();
        layoutGroup.enabled = false;
        // 初始化牌组
        var cardConfigs = ConfigManager.Instance.LoadConfigs<CardConfig>();
        foreach (var cardConfig in cardConfigs.Values)
        {
            for (int i = 0; i < cardConfig.Modulus; i++)
            {
                Card card = new Card();
                card.config = cardConfig;
                cardPile.Add(card);
            }
        }
        // 初始化玩家
        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player());
        }

        // 发牌
        for (int i = 0; i < initCardCount; i++)
        {
            for (int j = 0; j < playerCount; j++)
            {
                var cardIndex = Random.Range(0, cardPile.Count);
                players[j].AddCard(cardPile[cardIndex]);
                cardPile.RemoveAt(cardIndex);

                var sprite = ResourceManager.Instance.Load<Sprite>("card_back_alt");
                var cardTrans = ResourceManager.Instance.Load<Transform>("card");
                var cardImg = cardTrans.GetComponent<Image>();

                var playerTransTemp = playerTrans[j];
                cardTrans.SetParent(playerTransTemp);
                cardImg.sprite = sprite;
                cardTrans.localScale = Vector3.one;
                cardTrans.localEulerAngles = Vector3.zero;
                cardTrans.position = centerTrans.position;

                cardTrans.DOMove(playerTransTemp.position, 0.5f);
                //yield return new WaitForSeconds(0.2f);
            }    
        }

        myPlayer = players[0];
        myTrans = playerTrans[0];

        myPlayer.cardList.Sort((a, b) => int.Parse(a.config.Id) - int.Parse(b.config.Id));
        //yield return new WaitForSeconds(0.2f);
        layoutGroup.enabled = true;
        for (int i = 0; i < myTrans.childCount; i++)
        {
            myTrans.GetChild(i).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(myPlayer.cardList[i].config.ResName);
        }

        Timer.Register(1, OnTimerComplete, isLooped:true);
        yield return 0;
    }

    private void OnTimerComplete()
    {
        //Debug.LogError("timer complete");
    }
}
