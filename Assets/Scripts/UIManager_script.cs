using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_script : MonoBehaviour
{
    public GameObject EndGameScreen;
    private TextMeshProUGUI _endGameText;
    private TextMeshProUGUI _endGameTitleText;
    private Button _roundEndButton;
    private TextMeshProUGUI _roundEndButtonText;
    public Image[] LeftRoundCounter;
    public Image[] RightRoundCounter;
    public Image[] PlayerIndicator;
    private Image[] _playerIndicatorBackground;
    private GameController_script _gameController;
    public Color[] PlayerIndicatorColors;

    void Start()
    {
        _endGameText = EndGameScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _endGameTitleText = EndGameScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _roundEndButton = EndGameScreen.transform.GetChild(3).GetChild(0).GetComponent<Button>();
        _roundEndButtonText = _roundEndButton.GetComponentInChildren<TextMeshProUGUI>();
        _gameController = GetComponent<GameController_script>();
        _playerIndicatorBackground = new Image[2];
        _playerIndicatorBackground[0] = PlayerIndicator[0].transform.parent.GetComponent<Image>();
        _playerIndicatorBackground[1] = PlayerIndicator[1].transform.parent.GetComponent<Image>();

        ResetUI();
    }

    public void ToggleEndScreen(bool b, bool r, string t = null, string s = null)
    {
        EndGameScreen.SetActive(b);
        if (!b) //ui off skip rest
        {
            RestPlayerIndicators();
            return;
        }
        _endGameTitleText.text = t;
        _endGameText.text = s;

        _roundEndButton.onClick.RemoveAllListeners();

        if (r)
        {
            _roundEndButton.onClick.AddListener(_gameController.NewRound);
            _roundEndButtonText.text = "Next Round";
        }
        else
        {
            _roundEndButton.onClick.AddListener(_gameController.NewGame);
            _roundEndButtonText.text = "New Game";
        }
    }

    public void UpdateLeftRoundCounter(int a)
    {
        LeftRoundCounter[a - 1].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void UpdateRightRoundCounter(int a)
    {
        RightRoundCounter[a - 1].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResetUI()
    {
        foreach (Image li in LeftRoundCounter)
        {
            li.transform.GetChild(0).gameObject.SetActive(false);
        }
        foreach (Image ri in RightRoundCounter)
        {
            ri.transform.GetChild(0).gameObject.SetActive(false);
        }

        ToggleEndScreen(false,false);
    }

    public void SwitchPlayer(int i)
    {
        PlayerIndicator[0].enabled = false;
        PlayerIndicator[1].enabled = false;

        PlayerIndicator[i].enabled = true;
    }

    public void SetPlayerDone(int id)
    {
        PlayerIndicator[id].enabled = false;
        _playerIndicatorBackground[id].color = PlayerIndicatorColors[1];
    }

    void RestPlayerIndicators()
    {
        _playerIndicatorBackground[0].color = PlayerIndicatorColors[0];
        _playerIndicatorBackground[1].color = PlayerIndicatorColors[0];
    }
}
