using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCardHolder_script : MonoBehaviour
{
    public int ID;
    private PlayCard_script _playCard;
    private TotalValueTracker_script _totalValueTracker;

    void Start()
    {
        _totalValueTracker = GetComponentInParent<TotalValueTracker_script>();
    }

    public bool CheckValible(PlayCard_script pc)
    {
        if (pc == null)
        {
            return false;
        }
        if (pc.ID != ID || _playCard != null) //wrong slot or slot occupide
        {
            return false;
        }
        
        return true;
    }

}
