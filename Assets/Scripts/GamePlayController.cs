using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePlayController : SingletenMono<GamePlayController>
{
    public Transform centerTrans;
    public Transform rightTrans;

    /// <summary>
    /// 牌堆Trans
    /// </summary>
    public Transform cardPileTrans;

    /// <summary>
    /// 玩家位置
    /// </summary>
    public List<Transform> playerTransList;

    /// <summary>
    /// 牌堆数据
    /// </summary>
    private List<CardConfig> cardPile = new List<CardConfig>();

    /// <summary>
    /// 玩家
    /// </summary>
    private List<Player> playerList = new List<Player>();

    public Player currentPlayer;
    public CardComponent currentCard;

    public int nextTurnState;

    /// <summary>
    /// 玩家数量
    /// </summary>
    private int playerCount = 4;

    /// <summary>
    /// 初始化牌数
    /// </summary>
    private int initCardCount = 7;

    private Player myPlayer;
    private Transform myTrans;
    private HorizontalLayoutGroup layoutGroup;

    private int sendCount = 0;
    private bool isSend = false;
    private bool isReverse = false;
    Dictionary<string, CardConfig> cardConfigs;
    private IEnumerator Start()
    {
        layoutGroup = playerTransList[0].GetComponent<HorizontalLayoutGroup>();
        layoutGroup.enabled = false;
        // 初始化牌组
        cardConfigs = ConfigManager.Instance.LoadConfigs<CardConfig>();
        foreach (var cardConfig in cardConfigs.Values)
        {
            for (int i = 0; i < cardConfig.Modulus; i++)
            {
                cardPile.Add(cardConfig);
            }
        }
        // 初始化玩家
        for (int i = 0; i < playerCount; i++)
        {
            Player player = new Player();
            player.trans = playerTransList[i];
            playerList.Add(player);
        }

        // 发牌
        for (int i = 0; i < initCardCount; i++)
        {
            for (int j = 0; j < playerCount; j++)
            {
                var player = playerList[j];
                SendCard(player);
                yield return new WaitForSeconds(0.1f);
            }
        }
        myPlayer = playerList[0];

        yield return new WaitForSeconds(0.4f);
        layoutGroup.enabled = true;

        int cardCount = myPlayer.cardList.Count;
        SortCard(myPlayer);

        Timer.Register(1, OnTimerComplete, isLooped: true);
        cardPileTrans.SetParent(rightTrans);
        cardPileTrans.DOMove(rightTrans.position, 0.2f);

        StartCoroutine(NextTurn());
    }

    private void SortCard(Player player)
    {
        int cardCount = player.cardList.Count;

        CardComponent cardComponentTemp;
        bool isLoop = true;
        for (int i = 0; isLoop && i < cardCount - 1; i++)  //外层循环控制排序趟数
        {
            isLoop = false;
            for (int j = 0; j < cardCount - 1 - i; j++)  //内层循环控制每一趟排序多少次
            {
                if (int.Parse(player.cardList[j].config.Id) < int.Parse(player.cardList[j + 1].config.Id))
                {
                    cardComponentTemp = player.cardList[j];
                    player.cardList[j] = player.cardList[j + 1];
                    player.cardList[j + 1] = cardComponentTemp;
                    isLoop = true;
                }
            }
        }

        for (int i = 0; i < cardCount; i++)
        {
            player.cardList[i].transform.SetSiblingIndex(i);
            player.cardList[i].Show();
        }
    }

    private CardComponent SendCard(Player player)
    {
        var cardIndex = Random.Range(0, cardPile.Count);
        var cardConfig = cardPile[cardIndex];
        cardPile.RemoveAt(cardIndex);

        var sprite = ResourceManager.Instance.Load<Sprite>("card_back_alt");
        var cardTrans = ResourceManager.Instance.Load<Transform>("card");
        var cardImg = cardTrans.GetComponent<Image>();

        cardTrans.SetParent(player.trans);
        cardImg.sprite = sprite;
        cardTrans.localScale = Vector3.one;
        cardTrans.localEulerAngles = Vector3.zero;
        cardTrans.position = cardPileTrans.position;
        cardTrans.DOMove(player.trans.position, 0.5f);
        var cardComponent = cardTrans.GetComponent<CardComponent>();
        cardComponent.config = cardConfig;
        cardTrans.name = cardComponent.config.Id;
        player.AddCard(cardComponent);
        return cardComponent;
    }

    /// <summary>
    /// 游戏秒计时器
    /// </summary>
    private void OnTimerComplete()
    {
        //Debug.LogError("timer complete");
    }

    public IEnumerator NextTurn()
    {
        if (currentPlayer == null)
        {
            int firstPlayerIndex = Random.Range(0, playerList.Count);
            currentPlayer = playerList[firstPlayerIndex];

            if (playerList.IndexOf(currentPlayer) != 0)
            {
                var index = Random.Range(0, currentPlayer.cardList.Count);
                var pushCard = currentPlayer.cardList[index];
                TryPushCard(pushCard);
            }
        }
        else
        {
            int index = playerList.IndexOf(currentPlayer);

            if (isSend)
            {
                Debug.LogError(sendCount);
                sendCount += 1;
                for (int i = 0; i < sendCount; i++)
                {
                    SendCard(currentPlayer);
                    yield return new WaitForSeconds(0.1f);
                }
                sendCount = 0;

                if (currentCard.config.Type == "color_changer" || currentCard.config.Type == "pick_four")
                {
                    currentCard.config = cardConfigs["1"];
                }
                currentCard.config = cardConfigs["1"];

            }
            yield return new WaitForSeconds(0.4f);

            if (index == 0)
            {
                SortCard(currentPlayer);
            }

            if (isReverse)
            {
                index--;
            }
            else
            {
                index++;
            }

            if (index >= playerCount)
            {
                index = index - playerCount;
            }
            else if (index < 0)
            {
                index = playerCount + index;
            }
            Debug.LogError("当前玩家 ：" + index + " count: " + playerList.Count);
            currentPlayer = playerList[index];
            currentPlayer.isPush = false;

            if (index != 0)
            {
                foreach (var pushCard in currentPlayer.cardList)
                {
                    if (TryPushCard(pushCard))
                    {
                        break;
                    }
                }
                if (!currentPlayer.isPush)
                {
                    StartCoroutine(NextTurn());
                }
            }
        }
    }

    public bool TryPushCard(CardComponent card)
    {
        isSend = false;
        bool isPush = false;
        if (currentCard == null)
        {
            isPush = true;
        }
        else
        {
            if (currentCard.config.Type == "skip")
            {
                if (card.config.Type == "skip")
                {
                    isPush = true;
                }
                else
                {
                    isSend = true;
                }
            }
            else
            {
                if (currentCard.config.Type == "picker")
                {
                    if (card.config.Type == "picker")
                    {
                        sendCount += 2;
                        isPush = true;
                    }
                    else
                    {
                        isSend = true;
                    }
                }

                if (currentCard.config.Color == card.config.Color)
                {
                    if (card.config.Type == "picker")
                    {
                        sendCount += 2;
                    }
                    else if (card.config.Type == "reverse")
                    {
                        isReverse = !isReverse;
                        isSend = false;
                    }
                    isPush = true;
                }
                else if (currentCard.config.CardNumber != -1 && currentCard.config.CardNumber == card.config.CardNumber)
                {
                    isPush = true;
                }

                if (card.config.Type == "color_changer")
                {
                    if (currentCard.config.Type == "pick_four" || currentCard.config.Type == "picker")
                    {
                        isSend = true;
                    }
                    isPush = true;
                }
                else if (card.config.Type == "pick_four")
                {
                    sendCount += 4;
                    isSend = false;
                    isPush = true;
                }
            }
        }
        
        if (isPush)
        {
            if (card.config.Type != "reverse")
            {
                currentCard = card;
            }
            currentPlayer.PushCard(card);
            card.transform.SetParent(centerTrans);
            if (playerList.IndexOf(currentPlayer) != 0)
            {
                card.Show();
                var targetPos = new Vector3(Random.Range(0, 200), Random.Range(0, 200), 0);
                card.transform.DOMove(centerTrans.transform.TransformPoint(targetPos), 0.5f);
            }
            StartCoroutine(NextTurn());
        }
        else
        {
            isSend = true;
        }
        return isPush;
    }

    public void OnPassBtnClick()
    {
        isSend = true;
        StartCoroutine(NextTurn());
    }
}