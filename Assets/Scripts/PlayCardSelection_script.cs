using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardSelection_script : MonoBehaviour
{
    public string CardInfo;
    private GameObject _defaultCard;
    private TextMeshProUGUI _valueText, _infoText, _abilityButtonText;
    private Button _abilityButton;
    private int _value;
    private string _ability;
    private Button _infoButton;
    private Image _cardBackground;

    void SetLinks()
    {
        _defaultCard = GetComponentInChildren<PlayCard_script>().gameObject;
        /*_valueText = _defaultCard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        _cardBackground = _defaultCard.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _abilityButton = _defaultCard.transform.GetChild(1).GetComponent<Button>();
        _abilityButtonText = _abilityButton.GetComponentInChildren<TextMeshProUGUI>();*/

        _infoText = this.transform.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _infoText.gameObject.SetActive(false);

        //Debug.Log("Set button start");
        Transform buttonHolder = this.transform.GetChild(2);
        _infoButton = buttonHolder.GetChild(2).GetComponent<Button>();
        //Debug.Log("Set button button got - " +_infoText.transform.name);
        _infoButton.onClick.RemoveAllListeners();
        _infoButton.onClick.AddListener(ToggleInfo);
        //Debug.Log("Set button done");
        //ConfigDefaultCard("PM|5",4);
    }

   public void ConfigDefaultCard(string s, int a)
    {
        SetLinks();
        CardInfo = s;
        //SetValuesAndAbility(s);
        //CheckAbilities();
        CreatNewCard(a);
    }

    #region old stuff
    /*
 void SetValuesAndAbility(string s)
 {
     //Debug.Log("PlayCardSelection_script: SetValueAndAbility: string - " + s);
     string[] tempString = s.Split('|');
     _value = int.Parse(tempString[1]);
     switch (tempString[0])
     {
         case "N":
             _ability = "Normal";
             break;
         case "PM":
             _ability = "PlusMinus";
             break;
         case "D":
             _ability = "Double";
             break;
         case "T":
             _ability = "TieBreaker";
             break;
         case "F":
             _ability = "Flip";
             break;
         case "PM21":
             _ability = "PlusMinus21";
             break;
     }
 }

 void CheckAbilities()
 {
     switch (_ability)
     {
         case "Normal":
             _abilityButton.gameObject.SetActive(false);
             SetValueText();
             break;
         case "PlusMinus":
             _abilityButton.gameObject.SetActive(true);
             _abilityButtonText.text = "+-";
             _abilityButton.onClick.AddListener(ToggleValue);
             SetValueText();
             break;
         case "Double":
             _abilityButton.gameObject.SetActive(false);
             _value = 0;
             _valueText.text = "2x";
             break;
         case "TieBreaker":
             _abilityButton.gameObject.SetActive(true);
             _abilityButtonText.text = "+-";
             _abilityButton.onClick.AddListener(ToggleValue);
             _value = 1;
             SetValueText();
             break;

         case "Flip":
             _abilityButton.gameObject.SetActive(false);
             _value = 0;
             _valueText.text = "+-2/4";
             break;

         case "PlusMinus21":
             _abilityButton.gameObject.SetActive(true);
             _abilityButtonText.text = "+-1/2";
             _abilityButton.onClick.AddListener(ToggleValue21);
             _value = 1;
             SetValueText();
             break;
     }
 }

 public void ToggleValue()
 {
     Debug.Log("PlayCardSelection_script: ToggleValue: Start");
     _value = -_value;
     SetValueText();
 }

 public void ToggleValue21()
 {
     Debug.Log("PlayCardSelection_script: ToggleValue21: Start");
     switch (_value)
     {
         case 1:
             _value = -1;
             break;
         case -1:
             _value = 2;
             break;
         case 2:
             _value = -2;
             break;
         case -2:
             _value = 1;
             break;
     }
     SetValueText();
 }

 void SetValueText()
 {
     Debug.Log("PlayCardSelection_script:SetValueText: Start");
     string s;
     if (_value < 0)
     {
         s = _value.ToString("F0");
         _cardBackground.color = _cardColors[0];
     }
     else
     {
         s = "+" + _value;
         _cardBackground.color = _cardColors[1];
     }
     if (_ability == "TieBreaker")
     {
         s += "T";
     }
     _valueText.text = s;
 }*/


    #endregion

    void CreatNewCard(int a)
    {
        if (a == 1) //already card
        {
            return;
        }

        Transform holder = _defaultCard.transform.parent;
        for (int i = 0; i < a - 1; i++)
        {
            GameObject card = Instantiate(_defaultCard, holder.position,Quaternion.identity,holder);
            float r = Random.Range(-10, 10);
            card.transform.eulerAngles = new Vector3(0,0,r);
        }
    }

    public void ToggleInfo()
    {
        Debug.Log("PlayCardSeletion_script:ToggleInfo: Start");
        if (_infoText.gameObject.activeInHierarchy)
        {
            _infoText.gameObject.SetActive(false);
            foreach (Transform t in _defaultCard.transform.parent)
            {
                t.gameObject.SetActive(true);
            }
            //_defaultCard.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            _infoText.text = "Insert card info, "; //get card info from centra list based on ability
            //_defaultCard.transform.parent.gameObject.SetActive(false);
            foreach (Transform t in _defaultCard.transform.parent)
            {
                t.gameObject.SetActive(false);
            }
            _infoText.gameObject.SetActive(true);
        }
    }
}
