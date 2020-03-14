using System.Collections;
using System.Collections.Generic;
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
    public int MaxDeckSize;
    public Color[] CardColors;
    private MainCardHolder_script[] _selectedCardHolders;
    public Button StartButton;

    public PlayerDeckMananger_script PlayersDeck;

    void Start()
    {
        _selectedCardHolders = SelectedCardHolder.GetComponentsInChildren<MainCardHolder_script>();
        GenerateAllCards();
        StartButton.interactable = false;
    }

    private void GenerateAllCards()
    {
        foreach (string cardInfo in AllCards)
        {
            GameObject card = Instantiate(SelectCardPrefab, AllCardsHolder.position, Quaternion.identity,
                AllCardsHolder);
            PlayCardSelection_script pcs = card.GetComponentInChildren<PlayCardSelection_script>();
            PlayCard_script pc = card.GetComponentInChildren<PlayCard_script>();
            string[] cardString = cardInfo.Split(',');
            int cardAmount = int.Parse(cardString[0]);
            pc.Config(0,cardString[1],CardColors, GameController);
            pcs.ConfigDefaultCard(cardString[1], cardAmount);

        }
    }

    public void SaveSelectedCards()
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
        }
        else
        {
            StartButton.interactable = false;
        }
    }

    private bool CheckValidSelection()
    {
        foreach (MainCardHolder_script mc in _selectedCardHolders)
        {
            if (mc.transform.childCount == 0)
            {
                Debug.Log("Slot missing card. error");
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
    }

    public void ClearSelection()
    {
        List<PlayCard_script> pcs = new List<PlayCard_script>(SelectedCardHolder.GetComponentsInChildren<PlayCard_script>());
        foreach (PlayCard_script pc in pcs)
        {
            pc.BounceBack();
        }
    }
}
