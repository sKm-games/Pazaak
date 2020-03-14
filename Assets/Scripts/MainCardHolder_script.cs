using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCardHolder_script : MonoBehaviour
{
    public int ID;
    private PlayCard_script _playCard;
    private TotalValueTracker_script _totalValueTracker;
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _totalValueTracker = GetComponentInParent<TotalValueTracker_script>();
    }

    public void CheckEmpty() 
    {
        if (this.transform.childCount == 0)
        {
            _image.raycastTarget = false;
        }
        else
        {
            _image.raycastTarget = true;
        }
    }

    public bool CheckValible(PlayCard_script pc)
    {
        if (pc == null)
        {
            return false;
        }
        if (pc.PlayerID != ID || _playCard != null) //wrong slot or slot occupide
        {
            return false;
        }
        if (this.transform.childCount > 0)
        {
            return false;
        }
        
        return true;
    }

}
