using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CSVImporter_script : MonoBehaviour
{
    //This script controls the importing of level values from a given csv file
    public string SubFolder; //holds the name of the folber the file is in, only used if file is not in the Reasource folder

    public string FileName; //Holds the file name, must include the file type
    public List<string> AllImportedStrings; //holds all the lines in the imported file
    private int _activeLevelRef;

    [System.Serializable]
    public class ImportedValues //class to hold all the different values of all the active problems
    {
        public string[] Values;
    }

    public ImportedValues[] AllImportedValues; //used to acces the different problems.

    private IEnumerator loadFile;

    public UIManager_script UiManager;

    public delegate void LoadingDone(ImportedValues[] values);

    public static event LoadingDone OnLoadingDone;

    AssetLoader_script _assetsLoader;

    public bool CSVLoadingDone;

    void Awake()
    {
        _assetsLoader = GetComponent<AssetLoader_script>();
        UiManager.LoadingScreen.SetActive(true);
    }

    void Start()
    {
        LoadCSV();
    }

    public void LoadCSV() //Clear values and sets up and starts corutine
    {
        _activeLevelRef = 0;
        AllImportedStrings.Clear(); //clears the list, just for debuging
        AllImportedValues = null; //clears the all level class
        loadFile = loadStreamingAsset();
        StopCoroutine(loadFile);
        StartCoroutine(loadFile); //starts the file importing
    }

    IEnumerator loadStreamingAsset() //based on Germáns json import script, but had to tweak how the file is being read since its a csv and I needed it to spilt on lines.
    {
        //Debug.Log("LoadCSVCartDiver_script: loadStreamingAsset: Start import");
        //just creates the file path.
        string filePath = Application.streamingAssetsPath;
        //System.IO.Path.Combine(Application.streamingAssetsPath, "Resources");
        if (SubFolder != "")
        {
            filePath = System.IO.Path.Combine(filePath, SubFolder);
        }

        filePath = System.IO.Path.Combine(filePath, FileName);

        //filePath = testPath; //for testing with web file

        string result = ""; //string to hold the result of the downlaoded file.
        if (filePath.Contains("://") || filePath.Contains(":///")) //check if the file is on a webpage
        {
            yield return new WaitForSeconds(1f); //Safty wait
                                                 //WWW www = new WWW(filePath);
                                                 //yield return www;

            UnityWebRequest www = UnityWebRequest.Get(filePath); //Creates the link to the file
            yield return www.SendWebRequest(); //downloads the file
                                               //result = System.Text.Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);
            if (www.isNetworkError || www.isHttpError) //chekc for erros
            {
                Debug.Log(www.error);
            }
            else
            {
                //byte[] resultByte = www.downloadHandler.data;
                //result = System.Text.Encoding.UTF8.GetString(resultByte, 3, resultByte.Length - 3);
                result = www.downloadHandler.text; //creates a string out of all the text in the file
                ReadCSVFile(result); //sendts that string to the ReadCSVFile functions
                                     //Debug.Log("Import Result: " + result);
            }
            /*result = www.text;
            ReadCSVFile(result);*/

        }
        else
        {
            //result = System.IO.File.ReadAllText(filePath);
            /*string[] temp = System.IO.File.ReadAllLines(filePath);
            foreach (string s in temp)
            {
                mainImportedStrings.Add(s);
            }*/
            //mainImportedStrings.Add(result);
            //yield return new WaitForSeconds(0.1f); //need a wait here or the game created some referance errors. 2 sec might be way to long, will tweak this in the future.
            yield return new WaitForEndOfFrame(); //seems to be enought to ensure every is linked up and working
            result = System.IO.File.ReadAllText(filePath); //reads the local csv file.
                                                           //Debug.Log("Import Result: " +result);
            ReadCSVFile(result); //send the string to the ReadCSVFile functions
        }
    }

    public void ReadCSVFile(string s) //splits the csv file strings into lines, runs some MatrixBuilder specific code if the game mode selecteiong screen is not in use.
    {
        if (s == "") //if s if blank, the csv was not imported correctly, try again
        {
            Debug.LogError("CSVImporter: ReadCSVFile: Failed to load CSV or CSV empty");
            return;
        }

        AllImportedStrings = new List<string>(); //creates a new list of strings
        StringReader sr = new StringReader(s);
        string line;
        while ((line = sr.ReadLine()) != null) //reads each line of the downloaded string, if the string is not null add the line to the main string list
        {
           AllImportedStrings.Add(line);
        }

        AllImportedStrings.RemoveAt(0); //Removed the first info line from the file, this line usely holdes info about each column
        SplitStrings(); //split the values on ;
    }

    private void SplitStrings() //splits all the values into seperate values
    {
        AllImportedValues = new ImportedValues[AllImportedStrings.Count];
        for (int i = 0; i < AllImportedStrings.Count; i++)
        {
            AllImportedValues[i] = new ImportedValues();
            AllImportedValues[i].Values = AllImportedStrings[i].Split(';');
        }
        LoadAIImages();
        
    }

    private void LoadAIImages()
    {
        List<string> aiFileName = new List<string>();
        foreach (ImportedValues iv in AllImportedValues)
        {
            if (string.IsNullOrEmpty(iv.Values[5])) //skip blanks
            {
                continue;
            }
            aiFileName.Add(iv.Values[5]);
        }

        _assetsLoader.ImportAIGraphics(aiFileName);
        CheckLoadingDone();
    }

    public void CheckLoadingDone() //add some sort of loading done check
    {
        //LoadingScreen.SetActive(false);        
        OnLoadingDone(AllImportedValues);
        CSVLoadingDone = true;
        //UiManager.ToggleLoadingScreen(false);
    }
}

