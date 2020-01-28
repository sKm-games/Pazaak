using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayCard_script : MonoBehaviour
{
    public int ID;
    public int Value;
    private Transform _startingTransform;
    private Vector3 _startPos;
    private TextMeshProUGUI _valueText;
    public bool Placed;
    private TotalValueTracker_script _totalValueTracker;
    private GameController_script _gameController;
    private Transform _discardPile;
    public float DiscardTime;
    private Image _backgroundImage;

    void Awake()
    {

    }

    public void BounceBack()
    {
        Debug.Log("PlayCard_script: BounceBack: Start");
        this.transform.SetParent(_startingTransform);
        this.transform.localPosition = _startPos;
    }

    public void Config(int id, int v, Color c, GameController_script gc)
    {
        _startingTransform = transform.parent;
        _startPos = this.transform.localPosition;
        _valueText = this.transform.GetComponentInChildren<TextMeshProUGUI>();
        _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
        _backgroundImage = this.transform.GetChild(0).GetComponent<Image>();
        _backgroundImage.color = c;
        if (_totalValueTracker != null)
        {
            _discardPile = _totalValueTracker.DiscardPile;
        }

        _gameController = gc;
        ID = id;
        Value = v;
        _valueText.text = GetText();
    }

    private string GetText()
    {
        string s;
        if (Value < 0)
        {
            s = Value.ToString("F0");
        }
        else
        {
            s = "+" + Value;
        }

        return s;
    }

    public void PlaceCard(Transform t, bool b = true, bool s = true)
    {

        this.transform.position = t.position;
        this.transform.SetParent(t);
        if (_totalValueTracker == null)
        {
            _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
            _discardPile = _totalValueTracker.DiscardPile;
        }
        if (s)
        {
            _totalValueTracker.IncreaceValue(Value, b);
            Placed = true;
        }
    }

    public void RemoveCard()
    {
        StartCoroutine("DoRemoveCard");
    }

    IEnumerator DoRemoveCard()
    {
        this.transform.DOMove(_discardPile.position, DiscardTime);
        this.transform.DORotate(_discardPile.eulerAngles, DiscardTime);
        yield return new WaitForSeconds(DiscardTime);
        Destroy(this.gameObject);
    }
}
