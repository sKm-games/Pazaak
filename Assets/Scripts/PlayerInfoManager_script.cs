using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager_script : MonoBehaviour
{
    public string Name; //name of the player
    public Sprite Avatar; //avatar sprite
    public int Credits; //Amount of money
    public List<string> DeckInventroy; //All cards the player own
    public List<string> ActiveDeck; //All cards the player has selected to play with, max 12
    //Level? //used to unlock AI opponets?
    private int _wins, _loses, _gamesPlayed;
    private float _winRate; //wins/gamesPlayed
    
}
