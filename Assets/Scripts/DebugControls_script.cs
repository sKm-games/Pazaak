using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugControls_script : MonoBehaviour
{
    TextMeshProUGUI _debugText;
    public Button ContinueButton;
    
    void Start()
    {
        _debugText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ToggleDebugText()
    {
        _debugText.gameObject.SetActive(!_debugText.gameObject.activeInHierarchy);
    }
    
    public void DeleteGameSave()
    {
        SaveSystem_script.DeleteSaveFiles();
        ContinueButton.interactable = false;
    }
}
