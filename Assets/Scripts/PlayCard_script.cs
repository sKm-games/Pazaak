using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard_script : MonoBehaviour
{
    public int PlayerID;
    public string InfoString;
    public int Value;
    public Transform _startingTransform;
    private Vector3 _startPos;
    private TextMeshProUGUI _valueText;
    public bool Placed;
    private TotalValueTracker_script _totalValueTracker;
    [SerializeField]private GameController_script _gameController;
    private Transform _discardPile;
    public float DiscardTime;
    private Image _backgroundImage;
    private Button _abilityButton;
    private TextMeshProUGUI _abilityButtonText;
    private List<Color> _cardColors;
    public string Ability;
    private bool _playerCard;
    private Image _cardBackImage;
    private int[] _flipValue;

    public void BounceBack()
    {
        Debug.Log("PlayCard_script: BounceBack: Start");
        this.transform.SetParent(_startingTransform);
        this.transform.localPosition = _startPos;
    }

    public void Config(int id, string s, Color[] c, GameController_script gc, bool hideCard = false)
    {
        _startingTransform = transform.parent;
        _startPos = this.transform.localPosition;
        _valueText = this.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        _backgroundImage = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
        _abilityButton = GetComponentInChildren<Button>();
        _abilityButtonText = _abilityButton.GetComponentInChildren<TextMeshProUGUI>();
        _cardBackImage = this.transform.GetChild(2).GetComponent<Image>();

        _cardBackImage.gameObject.SetActive(hideCard);

        _gameController = gc;
        if (_gameController.GameStage == 1)
        {
            _totalValueTracker = this.transform.GetChild(0).GetComponentInParent<TotalValueTracker_script>();
            if (_totalValueTracker != null)
            {
                _discardPile = _totalValueTracker.DiscardPile;
            }
        }

        InfoString = s;
        PlayerID = id;
        SetValuesAndAbility(s);
        _cardColors = new List<Color>(c);
        SetValueText();
        CheckAbilities(s);
    }

    void SetValuesAndAbility(string s)
    {
        //Debug.Log("PlayCard_script: SetValueAndAbility: string - " +s +" - parent " +this.transform.parent.name);
        string[] tempString = s.Split('|');
        if (!tempString[1].Contains("/"))
        {
            Value = int.Parse(tempString[1]);
        }
        switch (tempString[0])
        {
            case "N":
                Ability = "Normal";
                break;
            case "PM":
                Ability = "PlusMinus";
                break;
            case "D":
                Ability = "Double";
                break;
            case "T":
                Ability = "TieBreaker";
                break;
            case "F":
                Ability = "Flip";
                string [] flipValues = tempString[1].Split('/');
                _flipValue = new int[2];
                _flipValue[0] = int.Parse(flipValues[0]);
                _flipValue[1] = int.Parse(flipValues[1]);
                break;
            case "PM21":
                Ability = "PlusMinus21";
                break;
        }
    }

    void CheckAbilities(string s)
    {
        switch (Ability)
        {
            case "Normal":
                _abilityButton.gameObject.SetActive(false);
                break;
            case "PlusMinus":
                _abilityButton.gameObject.SetActive(true);
                _abilityButton.onClick.RemoveAllListeners();
                _abilityButton.onClick.AddListener(ToggleValue);
                _abilityButtonText.text = "+-";

                //just to randomize the starting cards graphics a bit
                float r = Random.Range(0f, 1f);
                if (r > 0.5f)
                {
                    ToggleValue();
                }
                break;
            case "Double":
                _abilityButton.gameObject.SetActive(false);
                Value = 0;
                _valueText.text = "2x";
                break;
            case "TieBreaker":
                _abilityButton.gameObject.SetActive(true);
                _abilityButton.onClick.RemoveAllListeners();
                _abilityButton.onClick.AddListener(ToggleValue);
                _abilityButtonText.text = "+-";
                Value = 1;
                SetValueText();

                //just to randomize the starting cards graphics a bit
                r = Random.Range(0f, 1f);
                if (r > 0.5f)
                {
                    ToggleValue();
                }
                break;

            case "Flip":
                _abilityButton.gameObject.SetActive(false);
                _abilityButton.onClick.RemoveAllListeners();
                Value = 0;
                _valueText.text = "+-" +_flipValue[0]+"/"+_flipValue[1];
                break;

            case "PlusMinus21":
                _abilityButton.gameObject.SetActive(true);
                _abilityButton.onClick.RemoveAllListeners();
                _abilityButton.onClick.AddListener(ToggleValue21);
                _abilityButtonText.text = "+-1/2";
                Value = 1;
                SetValueText();
                break;
        }
    }

    void SetValueText()
    {
        string s;
        if (Value < 0)
        {
            s = Value.ToString("F0");
            _backgroundImage.color = _cardColors[0];
        }
        else
        {
            s = "+" + Value;
            _backgroundImage.color = _cardColors[1];
        }
        if (Ability == "TieBreaker")
        {
            s += "T";
        }
        _valueText.text = s;
    }

    public void PlaceCard(Transform t, bool b = true, bool s = true)
    {
        this.transform.position = t.position;
        this.transform.SetParent(t);
        this.transform.localScale = new Vector3(1,1,1); //to fix scaling bug
        if (_gameController == null)
        {
            _gameController = FindObjectOfType<GameController_script>();
        }
        //Debug.Log("GC - " +_gameController.transform.name);
        if (_gameController.GameStage == 0) //placing card during deck building
        {
            return;
        }
        _playerCard = b;
        if (_totalValueTracker == null)
        {
            _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
            _discardPile = _totalValueTracker.DiscardPile;
        }
        if (s)
        {
            Invoke(Ability, 0f);
            Placed = true;
        }
    }

    //Card abilities, trigger with invoke
    public void ToggleValue()
    {
        Value = -Value;
        SetValueText();
    }

    public void ToggleValue21()
    {
        switch (Value)
        {
            case 1:
                Value = -1;
                break;
            case -1:
                Value = 2;
                break;
            case 2:
                Value = -2;
                break;
            case -2:
                Value = 1;
                break;
        }
        SetValueText();
    }

    void Normal()
    {
        //Add value as normal
        _totalValueTracker.IncreaceValue(this, _playerCard);
    }

    void PlusMinus()
    {
        //Add value as normal
        _totalValueTracker.IncreaceValue(this, _playerCard);
        //Hide button
        _abilityButton.gameObject.SetActive(false);
    }

    void Double()
    {
        //Double last played card
        PlayCard_script lastCard = _totalValueTracker.GetLastCard();
        _totalValueTracker.IncreaceValue(lastCard,_playerCard);
        lastCard.Value *= 2;
        lastCard.SetValueText();
    }

    void TieBreaker()
    {
        //Add value as normal
        _totalValueTracker.IncreaceValue(this, _playerCard);
        //Hide button
        _abilityButton.gameObject.SetActive(false);
        //Set tie breaker
        _totalValueTracker.TieBreaker = true;
    }

    void Flip()
    {
        _totalValueTracker.FlipPlayedCards(_flipValue[0],_flipValue[1]);
    }

    void PlusMinus21()
    {
        //Add value as normal
        _totalValueTracker.IncreaceValue(this, _playerCard);
        //Hide button
        _abilityButton.gameObject.SetActive(false);
    }

    //Card removal

    public void ToggleCardBack(bool b)
    {
        _cardBackImage.gameObject.SetActive(b);
    }

    public void RemoveCard()
    {
        StartCoroutine("DoRemoveCard");
    }

    IEnumerator DoRemoveCard() //move to discard pile
    {
        this.transform.DOMove(_discardPile.position, DiscardTime);
        float r = Random.Range(-20, 20);
        Vector3 newRot = new Vector3(0,0, r);
        //this.transform.DORotate(_discardPile.eulerAngles, DiscardTime);
        this.transform.DORotate(newRot, DiscardTime);
        yield return new WaitForSeconds(DiscardTime);
        this.transform.SetParent(_discardPile);
        //this.transform.localPosition = _discardPile.position;
        //Destroy(this.gameObject);
    }

    public void DestroyCard()
    {
        StartCoroutine("DoDestroyCard");
    }

    IEnumerator DoDestroyCard()
    {
        this.transform.DOScale(new Vector3(0, 0, 0), DiscardTime);
        yield return new WaitForSeconds(DiscardTime);
        Destroy(this.gameObject);
    }

    //Debug stuff
    public void ToggleCardBack()
    {
        _cardBackImage.gameObject.SetActive(!_cardBackImage.gameObject.activeInHierarchy);
    }
}
