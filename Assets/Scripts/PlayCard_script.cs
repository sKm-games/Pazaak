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

    public void Config(int id, int v, Color[] c, GameController_script gc)
    {
        _startingTransform = transform.parent;
        _startPos = this.transform.localPosition;
        _valueText = this.transform.GetComponentInChildren<TextMeshProUGUI>();
        _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
        _backgroundImage = this.transform.GetChild(0).GetComponent<Image>();
        if (_totalValueTracker != null)
        {
            _discardPile = _totalValueTracker.DiscardPile;
        }

        _gameController = gc;
        ID = id;
        Value = v;

        string s;
        if (Value < 0)
        {
            s = Value.ToString("F0");
            _backgroundImage.color = c[0];
        }
        else
        {
            s = "+" + Value;
            _backgroundImage.color = c[1];
        }
        _valueText.text = s;
    }

    public void PlaceCard(Transform t, bool b = true, bool s = true)
    {

        this.transform.position = t.position;
        this.transform.SetParent(t);
        this.transform.localScale = new Vector3(1,1,1); //to fix scaling bug
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
}
