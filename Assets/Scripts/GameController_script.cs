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
    public AIMananger_script AiMananger;
    public float SwitchDelay;
    private Coroutine _switchPlayer;
    public bool RoundDone;
    
    void Awake()
    {
        _leftBoard = MainCanvas.transform.GetChild(1).GetChild(4).GetComponent<TotalValueTracker_script>();
        _rigthBoard = MainCanvas.transform.GetChild(1).GetChild(5).GetComponent<TotalValueTracker_script>();
        _uiManager = GetComponent<UIManager_script>();
        _globalDeckManager = GetComponent<GlobalDeckManager_script>();
        ActivePlayer = 1;
    }

    void Start()
    {
        _leftBoard.TogglePlayer(false);
        _rigthBoard.TogglePlayer(false);
        Invoke("GenerateDecks", 1f);
        Invoke("StartGame", 2f);
    }

    public void GenerateDecks()
    {
        _leftBoard.GenerateDeck();
        _rigthBoard.GenerateDeck();
        _globalDeckManager.GenerateGlobalDeck();
    }

    public void StartGame()
    {
        RoundDone = false;
        SwitchPlayer();
    }

    public void SwitchPlayer()
    {
        _switchPlayer = StartCoroutine(DoSwitchPlayer());
        Debug.Log("Switch Player: ActivePlayer - " +ActivePlayer);
    }

    IEnumerator DoSwitchPlayer()
    {
        if (RoundDone)
        {
            Debug.Log("Round Over");
            //StopCoroutine(_switchPlayer);
            yield break;
        }
        _leftBoard.TogglePlayer(false);
        _rigthBoard.TogglePlayer(false);
        yield return new WaitForSeconds(SwitchDelay);
        Debug.Log("Do Switch Player");
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
            RoundDone = true;
            CompareScore();
            //StopCoroutine(_switchPlayer);
            yield break;
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
        if (ActivePlayer == AiMananger.AIBoard.PlayerID)
        {
            AiMananger.DeterminPlay();
        }
    }

    void CompareScore()
    {
        Debug.Log("Compare score");
        string t;
        string s;
        RoundDone = true;
        AiMananger.RoundOver();
        if ((_leftBoard.ActiveValue > _rigthBoard.ActiveValue && _leftBoard.ActiveValue <= MaxValue) || (_leftBoard.ActiveValue <= MaxValue && _rigthBoard.ActiveValue > MaxValue)) //left wins
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
            ActivePlayer = 1;
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
        else if ((_rigthBoard.ActiveValue > _leftBoard.ActiveValue && _rigthBoard.ActiveValue <= MaxValue) || (_rigthBoard.ActiveValue <= MaxValue && _leftBoard.ActiveValue > MaxValue)) //right wins
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
            ActivePlayer = 0;
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
        else if(_leftBoard.ActiveValue == _rigthBoard.ActiveValue || (_leftBoard.ActiveValue > MaxValue && _rigthBoard.ActiveValue > MaxValue)) //draw
        {
            t = "Round Over";
            s = "Draw";
            _uiManager.ToggleEndScreen(true,true, t, s);
        }
    }

    public void NewRound() //ui button
    {
        Debug.Log("New round");
        StopAllCoroutines();
        StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        _uiManager.ToggleEndScreen(false, false);
        List<PlayCard_script> pcs = new List<PlayCard_script>();
        pcs.AddRange(_leftBoard.transform.GetChild(0).GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_rigthBoard.transform.GetChild(0).GetComponentsInChildren<PlayCard_script>());
        //sound effect
        foreach (PlayCard_script pc in pcs)
        {
           pc.RemoveCard();
        }
        _leftBoard.ResetValues();
        _rigthBoard.ResetValues();
        yield return new WaitForSeconds(1.1f);
        //screen transition etc
        StartGame();
    }

    public void NewGame() //ui button
    {
        Debug.Log("New game");
        StopAllCoroutines();
        StartCoroutine(StartNewGame());
    }

    IEnumerator StartNewGame()
    {
        _leftBoard.ResetValues();
        _rigthBoard.ResetValues();
        List<PlayCard_script> pcs = new List<PlayCard_script>();
        pcs.AddRange(_leftBoard.DiscardPile.GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_leftBoard._mainCardBoard.GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_leftBoard._handCardBoard.GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_rigthBoard._mainCardBoard.GetComponentsInChildren<PlayCard_script>());
        pcs.AddRange(_rigthBoard._handCardBoard.GetComponentsInChildren<PlayCard_script>());
        foreach (PlayCard_script pc in pcs)
        {
            pc.DestroyCard();
        }
        //screen transition etc
        yield return new WaitForSeconds(1f);
        GenerateDecks();
        yield return new WaitForSeconds(1f);
        _uiManager.ToggleEndScreen(false, false);
        _uiManager.ResetUI();
        StartGame();
    }
    
}
