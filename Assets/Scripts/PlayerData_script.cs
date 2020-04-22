using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData_script
{
    public string Name; //name of the player
    public int AvatarIndex; //avatar sprite index
    public int Credits; //Amount of money
    public List<string> DeckInventroy; //All cards the player own

    public List<string> CardString;
    public List<int> CardAmount;

    public int Wins, Loses, Played;

    public List<int> PartIndex;
    public List<int> ColorIndex;

    public float MusicVolume;
    public float SFXVolume;

    public PlayerData_script(PlayerInfoManager_script player)
    {
        Name = player.Name;
        Credits = player.Credits;
        //DeckInventroy = new List<string>(player.DeckInventroy);

        SplitPlayerDeck(out CardString,  out CardAmount, player);

        PartIndex = new List<int>(player.PartIndex);

        foreach (int p in PartIndex)
        {
            Debug.Log("part index: " + p);
        }

        ColorIndex = new List<int>(player.ColorIndex);

        foreach (int c in ColorIndex)
        {
            Debug.Log("color index: " + c);
        }

        Wins = player.Wins;
        Loses = player.Loses;
        Played = player.Played;

        MusicVolume = player.MusicVolume;
        SFXVolume = player.SFXVolume;
    }

    private void SplitPlayerDeck(out List<string> cardInfo, out List<int> cardAmount, PlayerInfoManager_script player)
    {
        cardInfo = new List<string>();
        cardAmount = new List<int>();
        foreach (PlayerInfoManager_script.DeckInventroyClass card in player.PlayerDeck)
        {
             cardInfo.Add(card.CardInfo);
             cardAmount.Add(card.CardAmount);
        }
    }
}
