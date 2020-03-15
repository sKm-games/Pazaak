using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbilityInfoManager_script : MonoBehaviour
{
    [System.Serializable]
    public class CardInfo
    {
        public string AbilityID;
        [TextArea]
        public string AbilityInfo;
    }

    public CardInfo[] CardAbilitisInfo;

    public string GetAbilityInfo(string ability)
    {
        foreach (CardInfo ci in CardAbilitisInfo)
        {
            if (ability.Contains(ci.AbilityID))
            {
                return ci.AbilityInfo;
            }
        }

        return "Ability not found, no info, sry...";
    }
}
