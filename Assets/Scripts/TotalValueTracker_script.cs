using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalValueTracker_script : MonoBehaviour
{
    public int PlayerID;
    public int ActiveValue;
    private TextMeshProUGUI _valueText;
    public Color[] TextColors;
    public bool PlayerDone;
    public int Wins;
    public GameController_script GameController;
    private Button _skipButton;
    private Button _stayButton;
    private Transform _mainCardBoard;

    void Awake()
    {
        _valueText = this.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        _skipButton = this.transform.GetChild(3).GetChild(0).GetComponent<Button>();
        _stayButton = this.transform.GetChild(3).GetChild(1).GetComponent<Button>();
        _mainCardBoard = this.transform.GetChild(0);
        ResetValues();
    }

    public void ResetValues()
    {
        ActiveValue = 0;
        _valueText.text = ActiveValue.ToString("F0");
        _valueText.color = TextColors[0];
        PlayerDone = false;
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
            SetPlayerDone();
            return;
            //Lost, over max
        }
        else if (b)
        {
            GameController.SwitchPlayer();
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
        PlayerDone = true;
        TogglePlayer(false);
        GameController.SwitchPlayer();
    }

    public void TogglePlayer(bool b)
    {
        _skipButton.interactable = b;
        _stayButton.interactable = b;
    }
}
