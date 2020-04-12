using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class AssetLoader_script : MonoBehaviour
{
    [System.Serializable]
    public class SpriteHolderClass
    {
        public string NameString;
        public Sprite LoadedSprite;
    }

    public List<SpriteHolderClass> AIProfileSprite;
    public bool AILoadingDone, PlayerdLoadingDone;
    public List<SpriteHolderClass> PlayerProfileSprite;

    public CharacterCreator_script CharacterCreator;

    public TextMeshProUGUI PathDebugText;

    public List<string> importFileNames;

    private List<string> characterFileNames = new List<string>() //temp file names, need to add imprting of names
    {
        "bg_Background0.png", "bg_Background1.png","bg_Background2.png", "bg_Background3.png", "bg_Background4.png",
        "bp_Body0.png", "bp_Body1.png", "bp_Body2.png", "bp_Body3.png", "bp_Body4.png",
        "hp_Head0.png", "hp_Head1.png", "hp_Head2.png", "hp_Head3.png", "hp_Head4.png",
        "ep_Eyes0.png", "ep_Eyes1.png", "ep_Eyes2.png", "ep_Eyes3.png", "ep_Eyes4.png",
        "np_Nose0.png", "np_Nose1.png", "np_Nose2.png", "np_Nose3.png", "np_Nose4.png",
        "mp_Mouth0.png", "mp_Mouth1.png", "mp_Mouth2.png", "mp_Mouth3.png", "mp_Mouth4.png",
    };

    private Texture2D _noImage;

    private CSVImporter_script _csvImporter;
    private UIManager_script _uiManager;

    void Awake()
    {
        _csvImporter = GetComponent<CSVImporter_script>();
        _uiManager = _csvImporter.UiManager;
        PathDebugText.text = "";
        //StartCoroutine("ImportAI");
        ImportCharacterGraphics();
    }

    public void ImportAIGraphics(List<string> filenames)
    {
        importFileNames = new List<string>(filenames);
        StartCoroutine("ImportAI");
    }

    IEnumerator ImportAI()
    {
        string folderPath = Application.streamingAssetsPath;
        
        folderPath = System.IO.Path.Combine(folderPath, "AI");
        //PathDebugText.text = folderPath + "\n";
        //DirectoryInfo di = new DirectoryInfo(folderPath);
        //PathDebugText.text += " 1\n";
        //FileInfo[] filesInfo = di.GetFiles(); //do not work on Android
        //PathDebugText.text += "2\n";

        AIProfileSprite = new List<SpriteHolderClass>();

        //PathDebugText.text += "Images found: " + filesInfo.Length +"\n";
        //PathDebugText.text += "Images found: " + importFileNames.Count +"\n";

        foreach (string s in importFileNames) //import filenames with given name
        {

            string filePath = System.IO.Path.Combine(folderPath, s);
            //PathDebugText.text += filePath + "\n";
            WWW www = new WWW(filePath);
            if (!www.isDone)
            {
                yield return null;
            }

            Texture2D texture = www.texture;
            Sprite aiSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


            SpriteHolderClass temp = new SpriteHolderClass();

            temp.NameString = s;
            temp.LoadedSprite = aiSprite;

            AIProfileSprite.Add(temp);
        }
        #region oldImportingUsingFileInfoClass
        //FileInof class do not work on android
        /* foreach (FileInfo fi in filesInfo)
         {
             if (fi.Name.Contains(".meta"))
             {
                 continue;
             }

             string filePath = System.IO.Path.Combine(folderPath, fi.Name);
             PathDebugText.text += filePath + "\n";
             WWW www = new WWW(filePath);
             if (!www.isDone)
             {
                 yield return null;
             }

             Texture2D texture = www.texture;
             Sprite aiSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));


             SpriteHolderClass temp = new SpriteHolderClass();

             temp.NameString = fi.Name;
             temp.LoadedSprite = aiSprite;

             AIProfileSprite.Add(temp);
         }*/
        #endregion
        AILoadingDone = true;
    }

    public Sprite GetAISprite(string nameRef)
    {
        foreach (SpriteHolderClass sh in AIProfileSprite)
        {
            if (sh.NameString == nameRef)
            {
                return sh.LoadedSprite;
            }
        }

        return null;
    }

    public void ImportCharacterGraphics()
    {
        StartCoroutine("DoImportCharacterGraphics");
    }

    IEnumerator DoImportCharacterGraphics()
    {
        string folderPath = Application.streamingAssetsPath;

        folderPath = System.IO.Path.Combine(folderPath, "CharacterParts");

        string filePathNo = System.IO.Path.Combine(folderPath, "NoFile");

        PlayerProfileSprite = new List<SpriteHolderClass>();
        
        WWW wwwNo = new WWW(filePathNo);

        if (!wwwNo.isDone)
        {
            yield return null;
        }

        _noImage = wwwNo.texture;

        foreach (string s in characterFileNames) //import filenames with given name
        {

            string filePath = System.IO.Path.Combine(folderPath, s);
            //PathDebugText.text += filePath + "\n";

            /*if (!File.Exists(filePath))
            {
                PathDebugText.text += s + " no exist\n";
                continue;
            }*/

            WWW www = new WWW(filePath);
            if (!www.isDone)
            {
                yield return null;
            }
            Texture2D texture = www.texture;

            if (texture.height == _noImage.height)                
            {
                //PathDebugText.text += s + " no exist\n";
                continue;
            }

            Sprite partSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            SpriteHolderClass temp = new SpriteHolderClass();

            temp.NameString = s;
            temp.LoadedSprite = partSprite;

            PlayerProfileSprite.Add(temp);
        }
        CharacterCreator.SetGraphicsParts(PlayerProfileSprite);
        PlayerdLoadingDone = true;

        StartCoroutine("CheckLoadingDone");
    }

    IEnumerator CheckLoadingDone()
    {
        if (!PlayerdLoadingDone || !_csvImporter.CSVLoadingDone) //wait for all AI images to load
        {
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        _uiManager.ToggleLoadingScreen(false);

    }
}
