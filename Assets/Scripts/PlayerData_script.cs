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

    public List<string> cardString;
    public List<int> cardAmount;

    public int Wins, Loses, Played;

    public PlayerData_script(PlayerInfoManager_script player)
    {
        Name = player.Name;
        AvatarIndex = player.AvatarIndex;
        Credits = player.Credits;
        //DeckInventroy = new List<string>(player.DeckInventroy);

        SplitPlayerDeck(out cardString,  out cardAmount, player);

        Wins = player.Wins;
        Loses = player.Loses;
        Played = player.Played;
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
