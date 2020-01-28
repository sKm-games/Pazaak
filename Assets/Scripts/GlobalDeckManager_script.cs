﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDeckManager_script : MonoBehaviour
{
    public GameObject GlobalPlayCardPrefab;
    public List<int> GlobalDeck;
    public int DeckSize;
    private List<int> _activeDeck;
    private Transform _leftMainCardBoard, _rigthMainCardBoard;
    private TotalValueTracker_script _leftBoard, _rigthBoard;
    private GameController_script _gameController;

    void Start()
    {
        _gameController = GetComponent<GameController_script>();
        _leftBoard = _gameController._leftBoard;
        _rigthBoard = _gameController._rigthBoard;
        _leftMainCardBoard = _leftBoard.transform.GetChild(0);
        _rigthMainCardBoard = _rigthBoard.transform.GetChild(0);
    }

    public void MakeActiveDeck()
    {
        _activeDeck = new List<int>();
        for (int i = 0; i < DeckSize; i++)
        {
            _activeDeck.AddRange(GlobalDeck);
        }
        _activeDeck.Shuffle();
    }

    public void PlaceGlobalCard(int p)
    {
        Transform spawn = FindSpawnLoc(p);
        if (spawn == null)
        {
            return;
        }
        GameObject card = Instantiate(GlobalPlayCardPrefab);
        PlayCard_script pc = card.GetComponent<PlayCard_script>();
        int i = _activeDeck[0];
        pc.Config(3,i);
        pc.PlaceCard(spawn,false);
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
}
