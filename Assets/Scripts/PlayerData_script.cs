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
    public int Wins, Loses, Played;

    public PlayerData_script(PlayerInfoManager_script player)
    {
        Name = player.Name;
        AvatarIndex = player.AvatarIndex;
        Credits = player.Credits;
        DeckInventroy = new List<string>(player.DeckInventroy);
        Wins = player.Wins;
        Loses = player.Loses;
        Played = player.Played;
    }
}
