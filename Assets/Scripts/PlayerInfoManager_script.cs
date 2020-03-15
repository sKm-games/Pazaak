using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager_script : MonoBehaviour
{
    public string Name; //name of the player
    public int AvatarIndex;
    public Sprite Avatar; //avatar sprite
    public int Credits; //Amount of money
    public List<string> DeckInventroy; //All cards the player own
    public List<string> ActiveDeck; //All cards the player has selected to play with, max 12
                                    //Level? //used to unlock AI opponets?
    public int Wins, Loses, Played;
    private float _winRate; //wins/gamesPlayed

    public GenerateSelectionCards_script CardSelection;


    public void CalcWinRate()
    {
        _winRate = (float)Wins / (float)Played;
    }

    public void GameWon()
    {
        Wins++;
        Played++;
        CalcWinRate();
        SavePlayerInfo();
    }

    public void GameLost()
    {
        Loses++;
        Played++;
        CalcWinRate();
        SavePlayerInfo();
    }


    //New game, save and load stuff
    public void LoadPlayerInfo()
    {
        PlayerData_script playerData = SaveSystem_script.LoadPlayer();
        Name = playerData.Name;
        AvatarIndex = playerData.AvatarIndex;
        Credits = playerData.Credits;
        DeckInventroy = playerData.DeckInventroy;
        Wins = playerData.Wins;
        Loses = playerData.Loses;
        Played = playerData.Played;
        CalcWinRate();
    }

    public void SavePlayerInfo()
    {
        SaveSystem_script.SaveData(this);
    }

    public void SetDefaultValues()
    {
        AvatarIndex = 0;
        Credits = 1000;
        DeckInventroy = new List<string>();
        Wins = 0;
        Loses = 0;
        Played = 0;
        DeckInventroy = new List<string>(CardSelection.AllCards);
        SavePlayerInfo();
        
    }

    public void SetPlayerName(string s)
    {
        Name = s;
    }
}
