using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInfoManager_script : MonoBehaviour
{
    public string Name; //name of the player
    public int AvatarIndex;
    public Sprite Avatar; //avatar sprite
    public int Credits; //Amount of money

    [System.Serializable]
    public class DeckInventroyClass
    {
        public string CardInfo;
        public int CardAmount;
    }

    public List<DeckInventroyClass> PlayerDeck;

    public List<string> ActiveDeck; //All cards the player has selected to play with, max 12
    
    //Level? //used to unlock AI opponets?

    public int Wins, Loses, Played;
    private float _winRate; //wins/gamesPlayed

    public GenerateSelectionCards_script CardSelection;
    public UIManager_script UiManager;


    public void CalcWinRate()
    {
        _winRate = (float)Wins / (float)Played;
    }

    public void GameWon()
    {
        Wins++;
        Played++;
        CalcWinRate();
    }

    public void GameLost()
    {
        Loses++;
        Played++;
        CalcWinRate();
    }

    public void ModifyCredits(int credits)
    {
        Credits += credits;
        UiManager.UpdatePlayerInfoBar();
        SavePlayerInfo();
    }

    public void AddNewCard(string cardInfo, int price)
    {
        Credits -= price;
        int index = 999;
        for (int i = 0; i < PlayerDeck.Count; i++)
        {
            if (PlayerDeck[i].CardInfo == cardInfo)
            {
                index = i;
                break;
            }
        }
        if (index == 999)
        {
            DeckInventroyClass newCard = new DeckInventroyClass();
            newCard.CardAmount = 1;
            newCard.CardInfo = cardInfo;
            PlayerDeck.Add(newCard);
        }
        else
        {
            PlayerDeck[index].CardAmount++;
        }

        SavePlayerInfo();
    }

    //New game, save and load stuff
    public void LoadPlayerInfo()
    {
        PlayerData_script playerData = SaveSystem_script.LoadPlayer();
        Name = playerData.Name;
        AvatarIndex = playerData.AvatarIndex;
        Credits = playerData.Credits;
        
        CombinePlayerDeck(playerData.cardString, playerData.cardAmount);

        Wins = playerData.Wins;
        Loses = playerData.Loses;
        Played = playerData.Played;

        CalcWinRate();
    }

    void CombinePlayerDeck(List<string> cardInfo, List<int> cardAmount)
    {
        if (cardInfo == null)
        {
            return;
        }
        PlayerDeck = new List<DeckInventroyClass>();
        for (int i = 0; i < cardInfo.Count; i++)
        {
            DeckInventroyClass newCard = new DeckInventroyClass();
            newCard.CardInfo = cardInfo[i];
            newCard.CardAmount = cardAmount[i];

            PlayerDeck.Add(newCard);
        }
    }

    public void SavePlayerInfo()
    {
        SaveSystem_script.SaveData(this);
    }

    public void SetDefaultValues()
    {
        AvatarIndex = 0;
        Credits = 1000;
        Wins = 0;
        Loses = 0;
        Played = 0;
        
        SavePlayerInfo();
    }

    public void SetPlayerName(string s)
    {
        Name = s;
    }
}
