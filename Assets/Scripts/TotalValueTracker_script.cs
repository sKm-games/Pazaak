using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TotalValueTracker_script : MonoBehaviour
{
    public int PlayerID;
    public string PlayerName;
    public int ActiveValue;
    public bool AllowMove;
    private TextMeshProUGUI _valueText;
    public Color[] TextColors;
    public bool PlayerDone;
    public int Wins;
    public GameController_script GameController;
    private Button _skipButton;
    private Button _stayButton;
    [HideInInspector] public Transform _mainCardBoard;
    [HideInInspector] public Transform _handCardBoard;
    private PlayerDeckMananger_script _playerDeckMananger;
    public Transform DiscardPile;
    private GameObject _playerDoneScreen;
    private Image _playerIndicator;
    private UIManager_script _uiManager;
    private List<PlayCard_script> _playedCards;
    public bool TieBreaker;

    void Awake()
    {
        _valueText = this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        _skipButton = this.transform.GetChild(3).GetChild(0).GetComponent<Button>();
        _stayButton = this.transform.GetChild(3).GetChild(1).GetComponent<Button>();
        _mainCardBoard = this.transform.GetChild(0);
        _handCardBoard = this.transform.GetChild(1);
        _playerDeckMananger = GetComponent<PlayerDeckMananger_script>();
        _playerDoneScreen = this.transform.GetChild(2).GetChild(3).gameObject;
        
        _uiManager = GameController.GetComponent<UIManager_script>();
        _playerIndicator = _uiManager.PlayerIndicator[PlayerID];
        ResetValues(true);
    }

    public void GenerateDeck()
    {
        _playerDeckMananger.GenereateDeck();
        _playedCards = new List<PlayCard_script>();
    }

    public void ResetValues(bool newGame)
    {
        _playedCards = new List<PlayCard_script>();
        ActiveValue = 0;
        _valueText.text = ActiveValue.ToString("F0");
        _valueText.color = TextColors[0];
        PlayerDone = false;
        _playerDoneScreen.SetActive(false);
        if (newGame)
        {
            Wins = 0;
        }
        //TogglePlayer(true);
        /*PlayCard_script[] pcs = _mainCardBoard.GetComponentsInChildren<PlayCard_script>();
        foreach (PlayCard_script pc in pcs)
        {
            Destroy(pc.gameObject);
        }*/
    }

    public void IncreaceValue(PlayCard_script pc, bool playerCard = true)
    {
        _playedCards.Add(pc);
        ActiveValue += pc.Value;
        _valueText.text = ActiveValue.ToString("F0");
        if (ActiveValue == GameController.MaxValue)
        {
            _valueText.color = TextColors[1];
            SetPlayerDone();
            return;
            //auto end turn
        }
        else if (ActiveValue > GameController.MaxValue)
        {
            _valueText.color = TextColors[2];
            //SetPlayerDone();
            //return;
            //Lost, over max
        }
        else if (ActiveValue < GameController.MaxValue)
        {
            _valueText.color = TextColors[0];
        }
        if (playerCard)
        {
            AllowMove = false;
            //GameController.SwitchPlayer();
        }
        //next player
    }

    public PlayCard_script GetLastCard()
    {
        return _playedCards[_playedCards.Count - 1];
    }

    public void FlipPlayedCards(int a, int b) //only flip specific cards
    {
        int newActiveValue = 0;
        foreach (PlayCard_script pc in _playedCards)
        {
            if (pc.Value == a || pc.Value == b)
            {
                pc.ToggleValue();
            }
            newActiveValue += pc.Value; //recalcualte the total value
        }
        ActiveValue = newActiveValue;
        _valueText.text = ActiveValue.ToString("F0");
    }

    public void SetPlayerDone()
    {
        if(PlayerID != GameController.ActivePlayer)
        {
            Debug.Log("TotalValueTracker_script: SetPlayerDone: Wrong Player " +PlayerID+"/"+GameController.ActivePlayer);
            return;
        }
        _uiManager.SetPlayerDone(PlayerID);
        _playerDoneScreen.SetActive(true);
        PlayerDone = true;
        TogglePlayer(false);
        Debug.Log("TotalValueTracker_script: SetPlayerDone: set player done ID - " + PlayerID);
        GameController.SwitchPlayer();
    }

    public void TogglePlayer(bool b)
    {
        _skipButton.interactable = b;
        _stayButton.interactable = b;
        AllowMove = b;
    }    
}
