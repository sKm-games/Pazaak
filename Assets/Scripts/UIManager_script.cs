using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_script : MonoBehaviour
{
    public PlayerInfoManager_script PlayerInfoManager;

    public GameObject EndGameScreen, CardSelectionScreen, GameBoardScreen, StartScreen, PreGameScreen, OpponentSelction, PlayerInfoBar, CardShop, BetScreen;
    private TextMeshProUGUI _endGameText;
    private TextMeshProUGUI _endGameTitleText;
    private Button _roundEndButton, _cardSeletionButton;
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
    private TextMeshProUGUI _leftPlayerName, _rightPlayerName;    
    public GameObject LoadingScreen;
    public GameObject PreGamePlayerGraphics;
    private Image[] _leftPlayerImages, _preGamePlayerImages;
    private Image _rightPlayerImage;

    private TextMeshProUGUI _playerInfoBarName;
    private TextMeshProUGUI _playerInfoBarCredits;

    private TMP_InputField _bettingInputfield;

    void Start()
    {
        _endGameText = EndGameScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _endGameTitleText = EndGameScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _roundEndButton = EndGameScreen.transform.GetChild(4).GetChild(0).GetComponent<Button>();
        _roundEndButtonText = _roundEndButton.GetComponentInChildren<TextMeshProUGUI>();
        _cardSeletionButton = EndGameScreen.transform.GetChild(4).GetChild(1).GetComponent<Button>();
        _gameController = GetComponent<GameController_script>();
        _playerIndicatorBackground = new Image[2];
        _playerIndicatorBackground[0] = PlayerIndicator[0].transform.parent.GetComponent<Image>();
        _playerIndicatorBackground[1] = PlayerIndicator[1].transform.parent.GetComponent<Image>();
        _endGameLeftPlayerText = EndGameScreen.transform.GetChild(3).GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();
        _endGameRightPlayerText = EndGameScreen.transform.GetChild(3).GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();

        _leftBoard = _gameController.LeftBoard;
        _rightBoard = _gameController.RightBoard;

        _leftPlayerName = PlayerIndicator[0].transform.parent.parent.GetChild(4).GetComponent<TextMeshProUGUI>();
        _leftPlayerImages = PlayerIndicator[0].transform.parent.parent.GetChild(5).GetComponentsInChildren<Image>();
        _preGamePlayerImages = PreGamePlayerGraphics.GetComponentsInChildren<Image>();


        _rightPlayerName = PlayerIndicator[1].transform.parent.parent.GetChild(4).GetComponent<TextMeshProUGUI>();
        _rightPlayerImage = PlayerIndicator[1].transform.parent.parent.GetChild(5).GetComponent<Image>();


        _playerInfoBarName = PlayerInfoBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _playerInfoBarCredits = PlayerInfoBar.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        _bettingInputfield = BetScreen.transform.GetChild(1).GetComponent<TMP_InputField>();

        ResetUI();
        GameBoardScreen.SetActive(false);
        CardSelectionScreen.SetActive(false);
        BetScreen.SetActive(false);
        StartScreen.SetActive(true);

        VersionText.text = "Work in progress v. " +Application.version;
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
            _cardSeletionButton.gameObject.SetActive(false);
            _roundEndButton.onClick.AddListener(_gameController.NewRound);
            _roundEndButtonText.text = "Next Round";
        }
        else
        {
            _roundEndButton.onClick.AddListener(_gameController.NewGame);
            _roundEndButtonText.text = "Replay";
            _cardSeletionButton.gameObject.SetActive(true);
        }
    }

    public void UpdateLeftRoundCounter()
    {
        int a = _leftBoard.Wins;
        LeftRoundCounter[a - 1].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void UpdateRightRoundCounter()
    {
        int a = _rightBoard.Wins;
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
        _leftBoard.PlayerName = PlayerInfoManager.Name;
        _leftPlayerName.text = _leftBoard.PlayerName;

        _rightPlayerName.text = _rightBoard.PlayerName;
        _rightPlayerImage.sprite = _gameController.AiMananger.AIStates.AISprite;
    }

    public void SetPlayerGraphics(List<Sprite> parts, List<Color> colors)
    {
        //List<Sprite> parts = new List<Sprite>(PlayerInfoManager.AvatarParts);
        //List<Color> colors = new List<Color>(PlayerInfoManager.PartColors);

        for (int i = 0; i < _leftPlayerImages.Length - 1; i++)
        {
            _leftPlayerImages[i].sprite = parts[i];
            _leftPlayerImages[i].color = colors[i];
            _preGamePlayerImages[i].sprite = parts[i];
            _preGamePlayerImages[i].color = colors[i];
        }
    }

    public void ToggleLoadingScreen(bool b)
    {
        LoadingScreen.SetActive(b);
        if (b)
        {
            
            LoadingScreen.GetComponent<CanvasGroup>().DOFade(1, 1f);
        }
        else
        {
            LoadingScreen.GetComponent<CanvasGroup>().DOFade(0, 1f);
        }
    }

    public void ToPreGameMenu()
    {
        _gameController.GameStage = 0;
        UpdatePlayerInfoBar();
        GameBoardScreen.SetActive(false);
        CardSelectionScreen.SetActive(false);
        PreGameScreen.SetActive(true);
        PreGameScreen.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void DeactiveGameUI()
    {
        CardSelectionScreen.SetActive(false);
        OpponentSelction.SetActive(false);
        GameBoardScreen.SetActive(false);
        EndGameScreen.SetActive(false);
        CardShop.SetActive(false);
    }

    public void UpdatePlayerInfoBar()
    {
        _playerInfoBarName.text = PlayerInfoManager.Name;
        _playerInfoBarCredits.text = "Credits: " + PlayerInfoManager.Credits;
    }

    public void SetBetAmount()
    {
        if (!int.TryParse(_bettingInputfield.text, out int bet))
        {
            return;
        }
        if (bet > PlayerInfoManager.Credits)
        {            
            _bettingInputfield.text = PlayerInfoManager.Credits.ToString("F0");
            bet = PlayerInfoManager.Credits;
        }
        PlayerInfoManager.BetAmount = bet;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator IEQuitGameAndSave()
    {
        PlayerInfoManager.SavePlayerInfo();
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
