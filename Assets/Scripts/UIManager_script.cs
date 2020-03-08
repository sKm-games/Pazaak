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
    private TotalValueTracker_script _leftBoard, _rightBoard;
    public Color[] PlayerIndicatorColors;
    public TextMeshProUGUI VersionText;
    private TextMeshProUGUI[] _endGameLeftPlayerText;
    private TextMeshProUGUI[] _endGameRightPlayerText;
    private TextMeshProUGUI _leftPlayerName;
    private TextMeshProUGUI _rightPlayerName;
    public GameObject LoadingScreen;

    void Start()
    {
        _endGameText = EndGameScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _endGameTitleText = EndGameScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _roundEndButton = EndGameScreen.transform.GetChild(4).GetChild(0).GetComponent<Button>();
        _roundEndButtonText = _roundEndButton.GetComponentInChildren<TextMeshProUGUI>();
        _gameController = GetComponent<GameController_script>();
        _playerIndicatorBackground = new Image[2];
        _playerIndicatorBackground[0] = PlayerIndicator[0].transform.parent.GetComponent<Image>();
        _playerIndicatorBackground[1] = PlayerIndicator[1].transform.parent.GetComponent<Image>();
        _endGameLeftPlayerText = EndGameScreen.transform.GetChild(3).GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();
        _endGameRightPlayerText = EndGameScreen.transform.GetChild(3).GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

        _leftBoard = _gameController.LeftBoard;
        _rightBoard = _gameController.RightBoard;

        _leftPlayerName = PlayerIndicator[0].transform.parent.parent.GetChild(4).GetComponent<TextMeshProUGUI>();
        _rightPlayerName = PlayerIndicator[1].transform.parent.parent.GetChild(4).GetComponent<TextMeshProUGUI>();

        ResetUI();

        VersionText.text = Application.version;
    }

    public void ToggleEndScreen(bool b, bool r, string t = null, string s = null, string leftScore = null, string rightScore = null)
    {
        EndGameScreen.SetActive(b);
        if (!b) //ui off skip rest
        {
            RestPlayerIndicators();
            return;
        }
        _endGameTitleText.text = t;
        _endGameText.text = s;

        _endGameLeftPlayerText[0].text = _leftBoard.PlayerName;
        _endGameLeftPlayerText[1].text = "Score\n" + _leftBoard.ActiveValue + " of " + _gameController.MaxValue;
        _endGameLeftPlayerText[2].text = "Rounds\n" + _leftBoard.Wins + " of 3";

        _endGameRightPlayerText[0].text = _rightBoard.PlayerName;
        _endGameRightPlayerText[1].text = "Score\n" + _rightBoard.ActiveValue + " of " + _gameController.MaxValue;
        _endGameRightPlayerText[2].text = "Rounds\n" + _rightBoard.Wins + " of 3";

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

    public void SetPlayerNames()
    {
        _leftPlayerName.text = _leftBoard.PlayerName;
        _rightPlayerName.text = _rightBoard.PlayerName;
    }

    public void ToggleLoadingScreen(bool b)
    {
        LoadingScreen.SetActive(b);
    }

}
