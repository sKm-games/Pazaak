using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayCard_script : MonoBehaviour
{
    public int ID;
    public int Value;
    private Transform _startingTransform;
    private Vector3 _startPos;
    private TextMeshProUGUI _valueText;
    public bool Placed;
    private TotalValueTracker_script _totalValueTracker;

    void Awake()
    {
        _startingTransform = transform.parent;
        _startPos = this.transform.position;
        _valueText = this.transform.GetComponentInChildren<TextMeshProUGUI>();
        _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
        _valueText.text = GetText();
    }

    public void BounceBack()
    {
        Debug.Log("PlayCard_script: BounceBack: Start");
        this.transform.SetParent(_startingTransform);
        this.transform.position = _startPos;
    }

    public void Config(int id, int v)
    {
        _valueText = this.transform.GetComponentInChildren<TextMeshProUGUI>();
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

    public void PlaceCard(Transform t, bool b = true)
    {
        
        this.transform.position = t.position;
        this.transform.SetParent(t);
        if (_totalValueTracker == null)
        {
            _totalValueTracker = this.transform.GetComponentInParent<TotalValueTracker_script>();
        }
        _totalValueTracker.IncreaceValue(Value, b);
        Placed = true;
    }
}
