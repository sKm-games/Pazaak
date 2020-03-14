using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISelecter_script : MonoBehaviour
{
    [System.Serializable]
    public class AIStatesClass
    {
        public string Difficulty;
        public string AIName;
        public List<string> DeckValues;
        public int EndOffset;
        public float RiskValue;
    }
    public List<AIStatesClass> AllAIs;

    private GameController_script _gameController;
    private AIMananger_script _aiMananger;

    void OnEnable()
    {
        CSVImporter_script.OnLoadingDone += GenerateAIs;
    }

    void OnDisable()
    {
        CSVImporter_script.OnLoadingDone += GenerateAIs;
    }

    void Start()
    {
        _aiMananger = GetComponent<AIMananger_script>();
        _gameController = _aiMananger.AIBoard.GameController;
    }

    public void GenerateAIs(CSVImporter_script.ImportedValues[] values)
    {

        foreach (CSVImporter_script.ImportedValues value in values)
        {
            AIStatesClass newAI = new AIStatesClass();
            newAI.Difficulty = value.Values[0];
            newAI.AIName = value.Values[1];
            string[] deckValues = value.Values[2].Split(',');
            newAI.DeckValues = new List<string>();
            foreach (string card in deckValues)
            {
                //int cardValue = int.Parse(card);
                //newAI.DeckValues.Add(cardValue);
                newAI.DeckValues.Add(card);
            }

            newAI.EndOffset = int.Parse(value.Values[3]);
            newAI.RiskValue = float.Parse(value.Values[4]);

            AllAIs.Add(newAI);
        }
        SelectDifficulty(_gameController.Difficulty);
    }

    public void SelectDifficulty(string difficulty)
    {
        List<AIStatesClass> tempAIList = new List<AIStatesClass>();
        if (difficulty == "" || difficulty == "all")
        {
            tempAIList = AllAIs;
        }
        else
        {
            foreach (AIStatesClass ai in AllAIs)
            {
                if (ai.Difficulty == difficulty)
                {
                    tempAIList.Add(ai);
                }
            }
        }

        tempAIList.Shuffle();

        _aiMananger.ConfigAI(tempAIList[0]);

        //_gameController.NewGame();
    }
}
