using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_script : MonoBehaviour
{
    public int MaxValue;
    public int ActivePlayer;
    public GameObject MainCanvas;
    [HideInInspector] public TotalValueTracker_script _leftBoard, _rigthBoard;
    private UIManager_script _uiManager;
    private GlobalDeckManager_script _globalDeckManager;
    
    void Awake()
    {
        _leftBoard = MainCanvas.transform.GetChild(1).GetChild(3).GetComponent<TotalValueTracker_script>();
        _rigthBoard = MainCanvas.transform.GetChild(1).GetChild(4).GetComponent<TotalValueTracker_script>();
        _uiManager = GetComponent<UIManager_script>();
        _globalDeckManager = GetComponent<GlobalDeckManager_script>();
        ActivePlayer = 1;
    }

    void Start()
    {
        StartGame(); //move to button
    }

    public void StartGame()
    {
        _globalDeckManager.MakeActiveDeck();
        SwitchPlayer();
    }

    public void SwitchPlayer()
    {
        if (_leftBoard.PlayerDone && !_rigthBoard.PlayerDone)
        {
            ActivePlayer = 1;
        }
        else if (_rigthBoard.PlayerDone && !_leftBoard.PlayerDone)
        {
            ActivePlayer = 0;
        }
        else if((_leftBoard.ActiveValue > MaxValue || _rigthBoard.ActiveValue > MaxValue) || (_leftBoard.PlayerDone && _rigthBoard.PlayerDone))
        {
            CompareScore();
            return;
        }
        else
        {
            ActivePlayer = (ActivePlayer == 0) ? 1 : 0;
        }
        if (ActivePlayer == 0)
        {
            _leftBoard.TogglePlayer(true);
            _rigthBoard.TogglePlayer(false);
        }
        else
        {
            _rigthBoard.TogglePlayer(true);
            _leftBoard.TogglePlayer(false);
        }
        _uiManager.SwitchPlayer(ActivePlayer);
        _globalDeckManager.PlaceGlobalCard(ActivePlayer);
    }

    void CompareScore()
    {
        Debug.Log("Compare score");
        string t;
        string s;
        if ((_leftBoard.ActiveValue > _rigthBoard.ActiveValue && _leftBoard.ActiveValue <= MaxValue) || (_leftBoard.ActiveValue < MaxValue && _rigthBoard.ActiveValue > MaxValue)) //left wins
        {
            _leftBoard.Wins++;
            _uiManager.UpdateLeftRoundCounter(_leftBoard.Wins);
            Debug.Log("GameController_script: CompareScore: Player 1(left) Wins");
            if (_leftBoard.Wins == 3)
            {
                t = "Game Over";
                s = "Player 1 Wins";
                _uiManager.ToggleEndScreen(true,false, t, s);
                return;
            }
            t = "Round Over";
            s = "Player 1 Wins";
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
        else if ((_rigthBoard.ActiveValue > _leftBoard.ActiveValue && _rigthBoard.ActiveValue <= MaxValue) || (_rigthBoard.ActiveValue < MaxValue && _leftBoard.ActiveValue > MaxValue)) //right wins
        {
            _rigthBoard.Wins++;
            _uiManager.UpdateRightRoundCounter(_rigthBoard.Wins);
            Debug.Log("GameController_script: CompareScore: Player 2(left) Wins");
            if (_rigthBoard.Wins == 3)
            {
                t = "Game Over";
                s = "Player 2 Wins";
                _uiManager.ToggleEndScreen(true,false, t, s);
                return;
            }
            t = "Round Over";
            s = "Player 2 Wins";
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
        else if(_leftBoard.ActiveValue == _rigthBoard.ActiveValue || (_leftBoard.ActiveValue > MaxValue && _rigthBoard.ActiveValue > MaxValue)) //draw
        {
            t = "Round Over";
            s = "Draw";
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
    }

    public void NewRound()
    {
        Debug.Log("New round");
        StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        List<PlayCard_script> pcs = new List<PlayCard_script>();
        pcs.AddRange(_leftBoard.transform.GetChild(0).GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_rigthBoard.transform.GetChild(0).GetComponentsInChildren<PlayCard_script>());
        foreach (PlayCard_script pc in pcs)
        {
            Destroy(pc.gameObject);
        }
        _leftBoard.ResetValues();
        _rigthBoard.ResetValues();
        yield return new WaitForEndOfFrame();
        //screen transition etc
        _uiManager.ToggleEndScreen(false, false);
        StartGame();
    }

    public void NewGame()
    {
        Debug.Log("New game");
        StartCoroutine(StartNewGame());
    }

    IEnumerator StartNewGame()
    {
        _leftBoard.ResetValues();
        _rigthBoard.ResetValues();
        //screen transition etc
        yield return new WaitForSeconds(2f);
        _uiManager.ToggleEndScreen(false, false);
        StartGame();
    }
    
}
