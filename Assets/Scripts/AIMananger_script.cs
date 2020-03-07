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
    public int MoveTime;
    private GameObject _buttonHolder;
    private Coroutine _determin;
    private Coroutine _moveCard;
    
    public AISelecter_script.AIStatesClass AIStates;

    private int debugTrack;

    void Start()
    {
        //ConfigAI();
    }

    public void ConfigAI(AISelecter_script.AIStatesClass aiStates) //when choosing AI
    {
        AIStates = new AISelecter_script.AIStatesClass();
        AIStates = aiStates;

        _aiDeck = AIBoard.GetComponent<PlayerDeckMananger_script>();
        _gameController = AIBoard.GameController;
        _buttonHolder = AIBoard.transform.GetChild(3).gameObject;
        AIBoard.PlayerName = AIStates.AIName;
        _aiDeck.SetPlayerDeck(AIStates.DeckValues, true);
        _buttonHolder.SetActive(false);

    }

    public void DeterminPlay()
    {
        //_determin = StartCoroutine(DoDeterminPlay());
        _determin = StartCoroutine(DoDeterminePlay());
    }

    IEnumerator DoDeterminePlay() //check for play
    {
        float rWait = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(rWait);
        if (AIBoard.PlayerDone)
        {
            yield break;
        }
        PlayCard_script tempCard = null;
        PlayCard_script drawCard = null;

        if (AIBoard.ActiveValue > _gameController.MaxValue) //Active value over max, check for minus card
        {
            tempCard = CheckOverMax();
            if (tempCard == null) //No card to get lower the max
            {
                AIBoard.SetPlayerDone();
            }
            else //found card play it
            {
                Debug.Log("AIMananger_script: DoDeterminePlay: Play card");
                _moveCard = StartCoroutine(MoveCard(tempCard));
                yield break;
            }
        }

        //player done check higher and less 20
        if (PlayerBoard.PlayerDone)
        {
            Debug.Log("AIMananger_script: DoDeterminePlay: Player done check");
            if (AIBoard.ActiveValue > PlayerBoard.ActiveValue) //random card won the game
            {
                Debug.Log("AIMananger_script: DoDeterminePlay: Random card fun");
                AIBoard.SetPlayerDone();
                yield break;
            }
            CheckPlayerDone(out tempCard, out drawCard);
        }

        if (tempCard != null)
        {
            Debug.Log("AIMananger_script: DoDeterminePlay: Play card");
            _moveCard = StartCoroutine(MoveCard(tempCard));
            yield break;
        }

        //check able to get 20
        Debug.Log("AIMananger_script: DoDeterminePlay: check for max");
        tempCard = CheckMaxValue();

        if (tempCard != null)
        {
            Debug.Log("AIMananger_script: DoDeterminePlay: Play card");
            _moveCard = StartCoroutine(MoveCard(tempCard));
            yield break;
        }

        //check close to max
        Debug.Log("AIMananger_script: DoDeterminePlay: check close to max");
        tempCard = CheckCloseMaxValue();

        if (tempCard != null)
        {
            Debug.Log("AIMananger_script: DoDeterminePlay: Play card");
            _moveCard = StartCoroutine(MoveCard(tempCard));
            yield break;
        }

        if (PlayerBoard.PlayerDone && (drawCard != null || tempCard != null)) //not possible to win try for a draw
        {
            int risk = Random.Range(0, 100);
            if (risk > AIStates.RiskValue) //AI will not risk a new card, goes for draw
            {
                Debug.Log("AIMananger_script: DoDeterminePlay: Play card");
                _moveCard = StartCoroutine(MoveCard(tempCard));
                yield break;
            }
        }

        //need to check max

        Debug.Log("AIMananger_script: DoDeterminePlay: no valid card found, skip");
        //Sound effect or something
        _gameController.SwitchPlayer();
    }

    void CheckPlayerDone(out PlayCard_script tempCard, out PlayCard_script drawCard) //check higher if player done and for draw
    {
        tempCard = null;
        drawCard = null;

        int risk = Random.Range(0, 100);
        if (risk < AIStates.RiskValue || (AIBoard.ActiveValue + (10 - AIStates.EndOffset)) > _gameController.MaxValue) //will risk a random card
        {
            return;
        }

        foreach (PlayCard_script card in _aiDeck.ActiveCards)
        {
            if (PlayerBoard.PlayerDone)
            {
                int diffValue = (card.Value + AIBoard.ActiveValue) - PlayerBoard.ActiveValue;

                if (card.Ability == "Normal")
                {
                    diffValue = (card.Value + AIBoard.ActiveValue) - PlayerBoard.ActiveValue;
                }
                else if (card.Ability == "PlusMinus")
                {
                    if (card.Value < 0)
                    {
                        card.ToggleValue();
                    }
                    diffValue = (card.Value + AIBoard.ActiveValue) - PlayerBoard.ActiveValue;
                }
                else if (card.Ability == "Double")
                {
                    PlayCard_script pc = AIBoard.GetLastCard();
                    diffValue = ((pc.Value * 2) + AIBoard.ActiveValue) - PlayerBoard.ActiveValue;
                }
                if (diffValue <= 0) //avoid draw
                {
                    drawCard = card;
                    continue;
                }
                /*if (diffValue == 0)
                {
                    drawCard = card;
                }*/
                if (tempCard == null)
                {
                    tempCard = card;
                    continue;
                }
                //find closes to player
                int tempDiffValue = (tempCard.Value + AIBoard.ActiveValue) - PlayerBoard.ActiveValue;
                if (diffValue < tempDiffValue)
                {
                    tempCard = card;
                    continue;
                }
            }
        }
    }

    PlayCard_script CheckMaxValue() //find max
    {
        PlayCard_script tempCard = null;

        foreach (PlayCard_script card in _aiDeck.ActiveCards)
        {
            int totalValue = 0;
            if (card.Ability == "Normal")
            {
                totalValue = card.Value + AIBoard.ActiveValue;
            }
            else if (card.Ability == "PlusMinus")
            {
                if (card.Value < 0)
                {
                    card.ToggleValue();
                }
                totalValue = card.Value + AIBoard.ActiveValue;
            }
            else if (card.Ability == "Double")
            {
                PlayCard_script pc = AIBoard.GetLastCard();
                totalValue = (pc.Value * 2) + AIBoard.ActiveValue;
            }
            if (totalValue == _gameController.MaxValue)
            {
                tempCard = card;
                break;
            }
        }

        return tempCard;
    }

    PlayCard_script CheckCloseMaxValue() //find closes to max
    {
        PlayCard_script tempCard = null;

        foreach (PlayCard_script card in _aiDeck.ActiveCards)
        {
            int totalValue = 0;
            int tempMax = _gameController.MaxValue - AIStates.EndOffset;
            if (card.Ability == "Normal")
            {
                totalValue = card.Value + AIBoard.ActiveValue;
            }
            else if (card.Ability == "PlusMinus")
            {
                if (card.Value < 0)
                {
                    card.ToggleValue();
                }
                totalValue = card.Value + AIBoard.ActiveValue;
            }
            else if (card.Ability == "Double")
            {
                PlayCard_script pc = AIBoard.GetLastCard();
                totalValue = (pc.Value * 2) + AIBoard.ActiveValue;
            }
            if (totalValue >= tempMax && totalValue < _gameController.MaxValue && totalValue > PlayerBoard.ActiveValue)
            {
                if (tempCard == null)
                {
                    tempCard = card;
                    continue;
                }
                else if (card.Value > tempCard.Value)
                {
                    tempCard = card;
                    continue;
                }
            }
        }

        return tempCard;
    }

    PlayCard_script CheckOverMax()
    {
        PlayCard_script tempCard = null;

        foreach (PlayCard_script card in _aiDeck.ActiveCards)
        {
            int totalValue = 0;
            if (card.Ability == "Normal")
            {
                totalValue = AIBoard.ActiveValue + card.Value;
            }
            else if (card.Ability == "PlusMinus")
            {
                if (card.Value > 0) //toggle value if card is PlusMinus card
                {
                    card.ToggleValue();
                }
                totalValue = AIBoard.ActiveValue + card.Value;
            }
            if (totalValue <= _gameController.MaxValue)
            {
                tempCard = card;
                break;
            }
        }
        return tempCard;
    }

    IEnumerator DoDeterminPlay() //old, broken.
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
            
            if (playTotal <= AIStates.EndOffset || (playTotal > PlayerBoard.ActiveValue && PlayerBoard.PlayerDone))
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

    IEnumerator MoveCard(PlayCard_script pc) //look for valid slot and move card
    {
        Debug.Log("AIMananger_script: MoveCard: Start");
        if (_gameController.RoundDone)
        {
            //round over
            //StopCoroutine(_moveCard); 
            yield break;
        }
        Transform slot = null;
        Debug.Log("AIMananger_script: MoveCard: find slot");
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
            Debug.Log("AIMananger_script: MoveCard: no valid slot");
            //no valid slot left
            AIBoard.SetPlayerDone();
            // StopCoroutine(_moveCard);
            yield break;
        }
        Debug.Log("AIMananger_script: MoveCard: move card");
        pc.ToggleCardBack(false);
        pc.transform.DOMove(slot.position, MoveTime);
        _aiDeck.RemoveCard(pc);
        yield return new WaitForSeconds(MoveTime);
        pc.PlaceCard(slot);
        /*if (AIBoard.ActiveValue == _gameController.MaxValue - EndOffset) //close to max, end player
        {
            AIBoard.SetPlayerDone();
            StopCoroutine(_moveCard);
        }*/
        //_gameController.SwitchPlayer();
        if (AIBoard.ActiveValue != _gameController.MaxValue)
        {
            AIBoard.SetPlayerDone();
        }
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
