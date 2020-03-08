using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSelectionCards_script : MonoBehaviour
{
    public GameController_script GameController;
    public GameObject SelectCardPrefab;
    public string[] AllCards;
    public string[] CurrentDeck;
    public Transform AllCardsHolder;
    public Transform SelectedCardHolder;
    public int MaxDeckSize;
    public Color[] CardColors;

    void Start()
    {
        GenerateAllCards();
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
}
