using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckMananger_script : MonoBehaviour
{
    public GameObject CardPrefab;
    public List<string> PlayerDeck;
    private List<string> _activeDeck;
    private Transform _cardSlots;
    private TotalValueTracker_script _totalValueTracker;
    public Color[] CardColors;
    public List<PlayCard_script> ActiveCards;
    private bool isAI;
    
    void Start()
    {
        _totalValueTracker = GetComponent<TotalValueTracker_script>();
        _cardSlots = this.transform.GetChild(1);
    }

    public void SetPlayerDeck(List<string> deckValues, bool b = false)
    {
        PlayerDeck = new List<string>(deckValues);
        isAI = b;
    }

    public void GenereateDeck()
    {
        _activeDeck = PlayerDeck;
        _activeDeck.Shuffle();
        foreach (Transform t in _cardSlots)
        {
            //int v = _activeDeck[0];
            string s = _activeDeck[0];
            _activeDeck.RemoveAt(0);
            GameObject go = Instantiate(CardPrefab);
            PlayCard_script pc = go.GetComponent<PlayCard_script>();
            pc.PlaceCard(t, false, false);
            pc.Config(_totalValueTracker.PlayerID, s, CardColors, _totalValueTracker.GameController,isAI);
            ActiveCards.Add(pc);
        }
    }

    public void RemoveCard(PlayCard_script pc)
    {
        if (ActiveCards.Contains(pc))
        {
            ActiveCards.Remove(pc);
        }
    }

    public int FindHighstCard()
    {
        int i = 0;
        foreach (PlayCard_script pc in ActiveCards)
        {
            if (pc.Value > i)
            {
                i = pc.Value;
            }
        }

        return i;
    }

}
