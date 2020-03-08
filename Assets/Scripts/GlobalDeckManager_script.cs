using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GlobalDeckManager_script : MonoBehaviour
{
    public GameObject GlobalPlayCardPrefab;
    public List<string> GlobalDeck;
    public int DeckSize;
    private List<string> _activeDeck;
    private Transform _leftMainCardBoard, _rigthMainCardBoard;
    private TotalValueTracker_script _leftBoard, _rigthBoard;
    private GameController_script _gameController;
    public Color[] CardColor;
    public Transform CardSpawnPoint;
    public float MoveTime;

    void Start()
    {
        _gameController = GetComponent<GameController_script>();
        _leftBoard = _gameController.LeftBoard;
        _rigthBoard = _gameController.RightBoard;
        _leftMainCardBoard = _leftBoard.transform.GetChild(0);
        _rigthMainCardBoard = _rigthBoard.transform.GetChild(0);
    }

    public void GenerateGlobalDeck()
    {
        _activeDeck = new List<string>();
        for (int i = 0; i < DeckSize; i++)
        {
            _activeDeck.AddRange(GlobalDeck);
        }
        _activeDeck.Shuffle();
    }

    public void PlaceGlobalCard(int p)
    {
        //Debug.Log("GlobalDeckManager_script:PlaceGlobalCard: Start - Value " +_activeDeck[0]);
        Transform spawn = FindSpawnLoc(p);
        if (spawn == null)
        {
            return;
        }
        if (_activeDeck.Count <= 0) //recreates active deck if deck runs out
        {
            GenerateGlobalDeck();
        }
        GameObject card = Instantiate(GlobalPlayCardPrefab, CardSpawnPoint.position, Quaternion.identity, CardSpawnPoint);
        PlayCard_script pc = card.GetComponent<PlayCard_script>();
        //int i = _activeDeck[0];
        string s = _activeDeck[0];
        pc.Config(_gameController.ActivePlayer,s, CardColor,_gameController);
        //Debug.Log("GlobalDeck: Add card to - " + _gameController.ActivePlayer + "- card value" + pc.Value);
        //pc.PlaceCard(spawn, false);
        StartCoroutine(MoveCard(card, pc, spawn));
        _activeDeck.RemoveAt(0);
    }

    private Transform FindSpawnLoc(int p)
    {
        Transform spawn = null;
        if (p == 0) //left
        {
            foreach (Transform t in _leftMainCardBoard)
            {
                if (t.childCount == 0)
                {
                    spawn = t;
                    return spawn;
                }
            }
            if (spawn == null)
            {
                _leftBoard.SetPlayerDone();
            }
        }
        else if (p == 1)
        {
            foreach (Transform t in _rigthMainCardBoard)
            {
                if (t.childCount == 0)
                {
                    spawn = t;
                    return spawn;
                }
            }
            if (spawn == null)
            {
                _rigthBoard.SetPlayerDone();
            }
        }
        //no valid found
        return null;
    }

    IEnumerator MoveCard(GameObject card, PlayCard_script pc, Transform spawn)
    {
        card.transform.DOMove(spawn.position, MoveTime);
        yield return new WaitForSeconds(MoveTime);
        pc.PlaceCard(spawn, false);
    }
}
