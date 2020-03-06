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
    }

    public void ResetValues(bool newGame)
    {
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

    public void IncreaceValue(int value, bool b = true)
    {
        ActiveValue += value;
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
        else if (b)
        {
            AllowMove = false;
            //GameController.SwitchPlayer();
        }
        //next player
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
