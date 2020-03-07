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
    public Color[] _cardColors; //get from central list
    private int _value;
    private string _ability;
    private Button _infoButton;
    private Image _cardBackground;

    void Awake()
    {
        _defaultCard = this.transform.GetChild(2).GetChild(0).gameObject;
        _valueText = _defaultCard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        _cardBackground = _defaultCard.transform.GetChild(0).GetComponent<Image>();
        _abilityButton = _defaultCard.GetComponentInChildren<Button>();
        _abilityButton.interactable = false;
        _abilityButtonText = _abilityButton.GetComponentInChildren<TextMeshProUGUI>();

        _infoText = this.transform.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _infoText.gameObject.SetActive(false);

        Transform buttonHolder = this.transform.GetChild(3);
        _infoButton = buttonHolder.GetChild(2).GetComponent<Button>();
        _infoButton.onClick.AddListener(ToggleInfo);
        ConfigDefaultCard("PM|5",4);
    }

    public void ConfigDefaultCard(string s, int a, Color[] c = null)
    {
        CardInfo = s;
        SetValuesAndAbility(s);
        CheckAbilities();
        CreatNewCard(a);
    }

    void SetValuesAndAbility(string s)
    {
        Debug.Log("PlayCardSelection_script: SetValueAndAbility: string - " + s);
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
                _value = 1;
                SetValueText();
                break;
        }
    }

    void SetValueText()
    {
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
    }

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
        if (_infoText.gameObject.activeInHierarchy)
        {
            _infoText.gameObject.SetActive(false);
            _defaultCard.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            _infoText.text = "Insert card info, "; //get card info from centra list based on ability
            _defaultCard.transform.parent.gameObject.SetActive(false);
            _infoText.gameObject.SetActive(true);
        }
    }
}
