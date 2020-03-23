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
        public int Credits;
        public string SpriteRef;
        public Sprite AISprite;
        public List<string> DeckValues;
        public int EndOffset;
        public float RiskValue;
    }
    
    //public List<AIStatesClass> AllAI;

    public List<AIStatesClass> EasyAI, MediumAI, HardAI;

    private GameController_script _gameController;
    private AIMananger_script _aiMananger;

    public AssetLoader_script AssetLoader;

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

    void GenerateAIs(CSVImporter_script.ImportedValues[] values)
    {
        StartCoroutine(DoGenerateAIs(values));
    }

    IEnumerator DoGenerateAIs(CSVImporter_script.ImportedValues[] values)
    {
        if (!AssetLoader.AILoadingDone) //wait for all AI images to load
        {
            yield return new WaitForSeconds(1f);
            yield return null;
        }

        foreach (CSVImporter_script.ImportedValues value in values)
        {
            if (value.Values[0] == "") //skip empty entries
            {
                continue;
            }
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

            newAI.SpriteRef = value.Values[5];
            newAI.AISprite = AssetLoader.GetAISprite(newAI.SpriteRef);

            newAI.Credits = int.Parse(value.Values[6]);

            if (newAI.Difficulty == "Easy")
            {
                EasyAI.Add(newAI);
            }
            else if (newAI.Difficulty == "Medium")
            {
                MediumAI.Add(newAI);
            }
            else if (newAI.Difficulty == "Hard")
            {
                HardAI.Add(newAI);
            }

            //AllAIs.Add(newAI);
        }
        //SelectDifficulty(_gameController.Difficulty);
    }

    public List<AIStatesClass> GetAIs()
    {
        List<AIStatesClass> aiList = new List<AIStatesClass>();
        EasyAI.Shuffle();
        MediumAI.Shuffle();
        HardAI.Shuffle();

        aiList.Add(EasyAI[0]);
        aiList.Add(MediumAI[0]);
        aiList.Add(HardAI[0]);

        return aiList;
    }

    /*public void SelectDifficulty(string difficulty)
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
    }*/
}
