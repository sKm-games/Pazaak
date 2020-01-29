using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AIMananger_script : MonoBehaviour
{
    public TotalValueTracker_script AIBoard;
    public TotalValueTracker_script PlayerBoard;
    private PlayerDeckMananger_script _aiDeck;
    private GameController_script _gameController;
    public float MoveTime;
    public int EndOffset;
    private GameObject _buttonHolder;
    private Coroutine _determin;
    private Coroutine _moveCard;

    private int debugTrack;
    void Start()
    {
        ConfigAI();
    }

    public void ConfigAI() //when choosing AI
    {
        _aiDeck = AIBoard.GetComponent<PlayerDeckMananger_script>();
        _gameController = AIBoard.GameController;
        _buttonHolder = AIBoard.transform.GetChild(3).gameObject;
        _buttonHolder.SetActive(false);
    }

    public void DeterminPlay()
    {
        _determin = StartCoroutine(DoDeterminPlay());
    }

    IEnumerator DoDeterminPlay()
    {
        debugTrack++;
        Debug.Log("AI Determin PLay - " +debugTrack);
        if (_gameController.ActivePlayer != AIBoard.PlayerID || _gameController.RoundDone || AIBoard.ActiveValue >= _gameController.MaxValue)
        {
            //not AI's turn or round over
            yield break;
        }
        //thinking animations
        float t = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(t);
        if ((AIBoard.ActiveValue > PlayerBoard.ActiveValue && AIBoard.ActiveValue < _gameController.MaxValue &&
            PlayerBoard.PlayerDone)) 
        {
            Debug.Log("More then player");
            AIBoard.SetPlayerDone();
            yield break;
            //StopCoroutine(_determin);
        }
        PlayCard_script[] pcs = AIBoard._handCardBoard.GetComponentsInChildren<PlayCard_script>();
        int playTotal;
        PlayCard_script pc = null;
        foreach (PlayCard_script fpc in pcs)
        {
            playTotal = AIBoard.ActiveValue + fpc.Value;
            if (playTotal > _gameController.MaxValue) //need minus
            {
                if (fpc.Value < 0)
                {
                    pc = fpc;
                    break;
                }
            }
            //check close to max
            
            if (playTotal <= EndOffset || (playTotal > PlayerBoard.ActiveValue && PlayerBoard.PlayerDone))
            {
                pc = fpc;
                break;
            }
        }
        if (pc != null)
        {
            _moveCard = StartCoroutine(MoveCard(pc));
            yield break;
        }
        else if (pc == null && AIBoard.ActiveValue > _gameController.MaxValue)
        {
            AIBoard.SetPlayerDone();
            yield break;
        }
        //no valid moves, skip
        Debug.Log("AI Switch Player");
        _gameController.SwitchPlayer();
    }

    IEnumerator MoveCard(PlayCard_script pc)
    {
        if (_gameController.RoundDone)
        {
            //round over
            //StopCoroutine(_moveCard); 
            yield break;
        }
        Transform slot = null;
        foreach (Transform t in AIBoard._mainCardBoard)
        {
            if (t.childCount == 0)
            {
                slot = t;
                break;
            }
        }
        if (slot == null)
        {
            //no valid slot left
            AIBoard.SetPlayerDone();
            // StopCoroutine(_moveCard);
            yield break;
        }

        pc.transform.DOMove(slot.position, MoveTime);
        yield return new WaitForSeconds(MoveTime);
        pc.PlaceCard(slot);
        /*if (AIBoard.ActiveValue == _gameController.MaxValue - EndOffset) //close to max, end player
        {
            AIBoard.SetPlayerDone();
            StopCoroutine(_moveCard);
        }*/
        //_gameController.SwitchPlayer();
        AIBoard.SetPlayerDone();
    }

    public void RoundOver()
    {
        Debug.Log("Stop AI");
        if (_moveCard != null)
        {
            StopCoroutine(_moveCard);
        }
        if (_determin != null)
        {
            StopCoroutine(_determin);
        }
    }
}
