using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpponentSelctionManager_script : MonoBehaviour
{
    public AISelecter_script AiSelecter;
    private AIMananger_script _aiMananger;
    
    private List<AISelecter_script.AIStatesClass> _activeAIs;

    private Transform _aiHolder;

    private List<Image> _aiImage;
    private List<TextMeshProUGUI> _aiText;

    private GameController_script _gameController;
    private UIManager_script _uiManager;
    GenerateSelectionCards_script _generateSelectionCards;

    void Awake()
    {
        _aiMananger = AiSelecter.GetComponent<AIMananger_script>();
        _gameController = _aiMananger.AIBoard.GameController;
        _uiManager = _gameController.GetComponent<UIManager_script>();


        _aiHolder = transform.GetChild(1);

        _aiImage = new List<Image>();
        _aiImage.Add(_aiHolder.GetChild(0).GetChild(0).GetComponent<Image>());
        _aiImage.Add(_aiHolder.GetChild(1).GetChild(0).GetComponent<Image>());
        _aiImage.Add(_aiHolder.GetChild(2).GetChild(0).GetComponent<Image>());

        _aiText = new List<TextMeshProUGUI>();
        _aiText.Add(_aiHolder.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>());
        _aiText.Add(_aiHolder.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>());
        _aiText.Add(_aiHolder.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>());

        _uiManager.OpponentSelction.SetActive(false);

        _generateSelectionCards = _uiManager.CardSelectionScreen.GetComponent<GenerateSelectionCards_script>();

    }
    
    public void GetAIs()
    {
        _activeAIs = new List<AISelecter_script.AIStatesClass>();
        _activeAIs.AddRange(AiSelecter.GetAIs());

        for (int i = 0; i < _activeAIs.Count; i++)
        {
            if (_activeAIs[i].AISprite != null)
            {
                _aiImage[i].sprite = _activeAIs[i].AISprite;

            }
            else
            {
                Debug.Log("AI image null");
            }

            string s;
            s = "<b>" + _activeAIs[i].AIName + "</b>\n";
            s += _activeAIs[i].Difficulty +"\n";
            s += "Bet \n";
            s += _activeAIs[i].Credits;

            _aiText[i].text = s;
        }
    }

    public void SelectAI(Transform t)
    {        
        _aiMananger.ConfigAI(_activeAIs[t.GetSiblingIndex()]);
        _uiManager.CardSelectionScreen.SetActive(true);
        _generateSelectionCards.SetUpCardSelection();
        _uiManager.OpponentSelction.SetActive(false);

    }
}
