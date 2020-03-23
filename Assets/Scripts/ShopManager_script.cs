using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class ShopManager_script : MonoBehaviour
{
    private Transform _cardHolder;
    public GameObject DefaultCard;
    public int MinAmount, MaxAmount;

    public List<string> ShopMasterList;
    private List<string> _activeList;

    [System.Serializable]
    public class ShopCardInfo
    {
        public string CardInfo;
        public int CardPrice;
        public PlayCard_script PlayCard;
        public Button BuyButton;
    }

    public List<ShopCardInfo> CurrentShop;

    public GameController_script GameController;
    private CardAbilityInfoManager_script _cardAbilityInfo;
    public PlayerInfoManager_script PlayerInfoManager;
    private UIManager_script _uiManager;

    public Color[] CardColors;

    public bool NewShop;

    public float PriceIncrease;
    public int StartingPrice;
    private int _activePrice;

    private Button _refreshButton;
    private TextMeshProUGUI _refreshPriceText;

    void Start()
    {
        _cardAbilityInfo = GameController.GetComponent<CardAbilityInfoManager_script>();
        _uiManager = GameController.GetComponent<UIManager_script>();
        _refreshPriceText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _refreshButton = transform.GetChild(2).GetChild(0).GetComponent<Button>();

        _cardHolder = transform.GetChild(1);
        //DefaultCard = _cardHolder.GetChild(0).gameObject;
        NewShop = true; //new shop after each game?
        _activePrice = StartingPrice;
        _refreshPriceText.text = _activePrice.ToString("F0") + " Credits";
    }

    public void GenereteShop()
    {
        if (!NewShop)
        {
            return;
        }
        _activeList = new List<string>(ShopMasterList);
        int r = Random.Range(MinAmount, MaxAmount);

        foreach (Transform card in _cardHolder)
        {
            Destroy(card.gameObject);   
        }

        CurrentShop = new List<ShopCardInfo>();
        for (int i = 0; i < r; i++)
        {
            _activeList.Shuffle();
            string[] cardInfo = _activeList[r].Split(',');
            ShopCardInfo shopCard = new ShopCardInfo();
            
            shopCard.CardPrice = int.Parse(cardInfo[0]);
            shopCard.CardInfo = cardInfo[1];
            
            GameObject card = Instantiate(DefaultCard, _cardHolder);
            card.SetActive(true);
            
            PlayCardSelection_script pcs = card.GetComponentInChildren<PlayCardSelection_script>();
            PlayCard_script pc = card.GetComponentInChildren<PlayCard_script>();
            pc.Config(0, shopCard.CardInfo, CardColors, GameController);
            pcs.ConfigDefaultCard(shopCard.CardInfo, 1, GameController);

            shopCard.PlayCard = pc;
            
            Button buyButton = pcs.transform.GetChild(2).GetChild(1).GetComponent<Button>();
            int index = i;
            buyButton.onClick.AddListener(() => BuyCard(index));
            TextMeshProUGUI priceText = card.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            priceText.text = shopCard.CardPrice.ToString("F0") + "\nCredits";

            if (PlayerInfoManager.Credits < shopCard.CardPrice)
            {
                buyButton.interactable = false;
            }

            shopCard.BuyButton = buyButton;

            CurrentShop.Add(shopCard);
        }

        NewShop = false;

        _refreshButton.interactable = _activePrice < PlayerInfoManager.Credits;
    }

    public void BuyCard(int index)
    {
        PlayerInfoManager.AddNewCard(CurrentShop[index].CardInfo, CurrentShop[index].CardPrice);

        CurrentShop[index].PlayCard.gameObject.SetActive(false);
        CurrentShop[index].PlayCard.transform.parent.parent.GetChild(4).gameObject.SetActive(true);
        CurrentShop[index].BuyButton.interactable = false;
        _uiManager.UpdatePlayerInfoBar();
    }

    public void RefreshStore()
    {
        NewShop = true;
        PlayerInfoManager.ModifyCredits(-_activePrice);
        float tempPrice = _activePrice * PriceIncrease;
        _activePrice = Mathf.RoundToInt(tempPrice);

        _refreshPriceText.text = _activePrice.ToString("F0") + " Credits";

        _refreshButton.interactable = _activePrice < PlayerInfoManager.Credits;
        GenereteShop();
    }
}
