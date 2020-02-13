using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputControls_script : MonoBehaviour
{
    private PlayCard_script _playCard;

    //---- Event data 
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
    //--

    private GameController_script _gameController;

    void Start()
    {
        _gameController = GetComponent<GameController_script>();
        m_Raycaster = FindObjectOfType<GraphicRaycaster>(); //finds the graphic raycaster
        m_EventSystem = FindObjectOfType<EventSystem>(); //finds the eventsystem
    }

    
    void Update()
    {
        if (_gameController.GameStage == 0)
        {
            
        }
        else if (_gameController.GameStage == 1)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GetCard();
            }

            if (Input.GetButton("Fire1"))
            {
                MoveCard();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                PlaceCard();
            }
        }
    }

    //Card Selection
    void SelectCard()
    {

    }


    //Main game controls
    void GetCard()
    {
        if (_playCard != null)
        {
            return;
            //already holding card
        }
        if (_playCard == null)
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            //Debug.Log("InputControls_script: GetCard: hits - " +results.Count);
            
            foreach (RaycastResult hit in results)
            {
                 _playCard = hit.gameObject.GetComponent<PlayCard_script>();
                 if (_playCard != null)
                 {
                     if (_playCard.ID != _gameController.ActivePlayer || _playCard.Placed)
                     {
                         _playCard = null;
                         Debug.Log("InputControls_script: GetCard: wrong players card");
                        //wrong player
                        return;
                     }
                    if ((_gameController.ActivePlayer == 0 && !_gameController._leftBoard.AllowMove) || (_gameController.ActivePlayer == 1 && !_gameController._rigthBoard.AllowMove))
                    {
                        _playCard = null;
                        Debug.Log("InputControls_script: GetCard: already placed card");
                        return;
                        
                    }
                    break;
                 }
            }
        }
    }
    
    void MoveCard()
    {
        if (_playCard == null)
        {
            return;
            //No card
        }
        
        _playCard.transform.position = Input.mousePosition;
    }

    void PlaceCard()
    {
        if (_playCard == null)
        {
            //Debug.Log("InputControls_script: PlaceCard: play card empty");
            return;
            //no card held
        }
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        Debug.Log("InputControls_script: GetCard: hits - " + results.Count);
        MainCardHolder_script cardHolder = null;

        foreach (RaycastResult hit in results)
        {
            cardHolder = hit.gameObject.GetComponent<MainCardHolder_script>();
            if (cardHolder != null)
            {
                break;
            }
        }
        if (cardHolder == null || !cardHolder.CheckValible(_playCard))
        {
            Debug.Log("InputControls_script: PlaceCard: wrong placement");
            //error sound
            _playCard.BounceBack();
            _playCard = null;
            return;
        }
        else
        {
            //success sound
            Debug.Log("InputControls_script: PlaceCard: Correct placement");
            //_gameController.SwitchPlayer();
            _playCard.PlaceCard(cardHolder.transform);
            _playCard = null;
        }
    }
}
