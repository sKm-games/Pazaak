using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen_script : MonoBehaviour
{
    public PlayerInfoManager_script PlayerInfoManager;
    public TMP_InputField NameInput;
    public Button ContinueButton;
    public UIManager_script UiManager;

    void Awake()
    {
        bool file = SaveSystem_script.FileExist();
        ContinueButton.interactable = file;
#if UNITY_WEBGL
        ContinueButton.gameObject.SetActive(false);
#endif
    }

    public void OpenGameRules()
    {
        Application.OpenURL("https://starwars.fandom.com/wiki/Pazaak/Legends");
    }

    public void LoadGame()
    {
        PlayerInfoManager.LoadPlayerInfo();
        ToGame();
    }

    public void NewGame()
    {
        PlayerInfoManager.SetDefaultValues();
        ToGame();
    }

    void ToGame()
    {
        this.gameObject.SetActive(false);
        //UiManager.CardSelectionScreen.SetActive(true);
        UiManager.DeactiveGameUI();
        UiManager.PreGameScreen.SetActive(true);
        UiManager.UpdatePlayerInfoBar();
    }

    public void FocusPlayerName()
    {
        NameInput.ActivateInputField();
    }

    public void SetPlayerName()
    {
        PlayerInfoManager.SetPlayerName(NameInput.text);
    }
}
