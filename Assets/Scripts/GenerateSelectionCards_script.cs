using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerateSelectionCards_script : MonoBehaviour
{
    public GameController_script GameController;
    public GameObject SelectCardPrefab;
    public string[] AllCards;
    public List<string> CurrentDeck;
    public Transform AllCardsHolder;
    public Transform SelectedCardHolder;
    public Color[] CardColors;
    private MainCardHolder_script[] _selectedCardHolders;
    public Button StartButton;
    private List<Transform> _cardPages;
    private int _cardPageIndex;
    private int _activePages;
    public GameObject CardPageButtonHolder;
    private TextMeshProUGUI _pageText;

    public PlayerDeckMananger_script PlayersDeck;
    public PlayerInfoManager_script PlayerInfo;

    void Start()
    {
        _selectedCardHolders = SelectedCardHolder.GetComponentsInChildren<MainCardHolder_script>();
        _pageText = CardPageButtonHolder.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        SetCardPages();
        GenerateAllCards();
        StartButton.interactable = false;
    }

    private void GenerateAllCards()
    {
        foreach (string cardInfo in PlayerInfo.DeckInventroy)
        {
            Transform parent = GetCardPage();
            GameObject card = Instantiate(SelectCardPrefab, parent.position, Quaternion.identity,
                parent);
            PlayCardSelection_script pcs = card.GetComponentInChildren<PlayCardSelection_script>();
            PlayCard_script pc = card.GetComponentInChildren<PlayCard_script>();
            string[] cardString = cardInfo.Split(',');
            int cardAmount = int.Parse(cardString[0]);
            pc.Config(0,cardString[1],CardColors, GameController);
            pcs.ConfigDefaultCard(cardString[1], cardAmount, GameController);
        }

        foreach (Transform c in _cardPages)
        {
            c.gameObject.SetActive(false);
            if (c.childCount > 0)
            {
                _activePages++;
            }
        }

        _activePages--; //reduce one to get array index
        _cardPages[0].gameObject.SetActive(true);

        if (_activePages <= 0) //one page? skip page setup
        {
            CardPageButtonHolder.SetActive(false);
            return;
        }

        _pageText.text = "1 / " +(_activePages + 1);
        
        _cardPageIndex = _cardPages.IndexOf(_cardPages[0]);
    }

    private void SetCardPages()
    {
        _cardPages = new List<Transform>();
        for (int i = 1; i < AllCardsHolder.transform.childCount; i++)
        {
            Transform child = AllCardsHolder.GetChild(i);
            if (child.tag == "CardPage")
            {
                _cardPages.Add(child);
            }
        }
    }

    private Transform GetCardPage()
    {
        foreach (Transform t in _cardPages)
        {
            if (t.childCount < 10)
            {
                return t;
            }
        }
        Debug.Log("GenerateSelectionCard_script: GetCardPage: no valid card page found, use last");
        return _cardPages[_cardPages.Count];
    }

    public void SetPlayerDeck()
    {
        if (CheckValidSelection())
        {
            CurrentDeck = new List<string>();
            PlayCard_script[] pcs = SelectedCardHolder.GetComponentsInChildren<PlayCard_script>();
            foreach (PlayCard_script pc in pcs)
            {
                CurrentDeck.Add(pc.InfoString);
            }
            StartButton.interactable = true;
            PlayersDeck.SetPlayerDeck(CurrentDeck);
            PlayerInfo.ActiveDeck = new List<string>(CurrentDeck);
        }
        else
        {
            StartButton.interactable = false;
        }
    }

    public bool CheckValidSelection()
    {
        foreach (MainCardHolder_script mc in _selectedCardHolders)
        {
            if (mc.transform.childCount == 0)
            {
                //Debug.Log("Slot missing card. error");
                return false;
            }
        }

        return true;
    }

    public void FillSlots()
    {
        List<PlayCard_script> pcs = new List<PlayCard_script>(AllCardsHolder.GetComponentsInChildren<PlayCard_script>());
        pcs.Shuffle();
        foreach (MainCardHolder_script mc in _selectedCardHolders)
        {
            if (mc.transform.childCount == 0)
            {
                pcs[0].PlaceCard(mc.transform);
                pcs.Remove(pcs[0]);
            }
        }
        SetPlayerDeck();
    }

    public void ClearSelection()
    {
        List<PlayCard_script> pcs = new List<PlayCard_script>(SelectedCardHolder.GetComponentsInChildren<PlayCard_script>());
        foreach (PlayCard_script pc in pcs)
        {
            pc.BounceBack();
        }
        CurrentDeck = new List<string>();

        StartButton.interactable = false;
    }

    public void SwitchPage(int i)
    {
        _cardPages[_cardPageIndex].gameObject.SetActive(false);
        _cardPageIndex += i;
        if (_cardPageIndex > _activePages)
        {
            _cardPageIndex = 0;
        }
        else if(_cardPageIndex < 0)
        {
            _cardPageIndex = _activePages;
        }
        _cardPages[_cardPageIndex].gameObject.SetActive(true);
        _pageText.text = (_cardPageIndex + 1) +" / " + (_activePages + 1); //offset by 1 to show nicer
    }
}
