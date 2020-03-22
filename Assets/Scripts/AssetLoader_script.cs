using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetLoader_script : MonoBehaviour
{
    [System.Serializable]
    public class SpriteHolderClass
    {
        public string NameString;
        public Sprite LoadedSprite;
    }
    public List<SpriteHolderClass> AIProfileSprite;
    public bool AILoadingDone;
    public List<SpriteHolderClass> PlayerProfileSprite;


    void Start()
    {
        StartCoroutine("ImportAI");
    }


    IEnumerator ImportAI()
    {
        string folderPath = Application.streamingAssetsPath;

        folderPath = System.IO.Path.Combine(folderPath, "AI");

        DirectoryInfo di = new DirectoryInfo(folderPath);
        FileInfo[] filesInfo = di.GetFiles();

        AIProfileSprite = new List<SpriteHolderClass>();

        foreach (FileInfo fi in filesInfo)
        {
            if (fi.Name.Contains(".meta"))
            {
                continue;
            }

            string filePath = System.IO.Path.Combine(folderPath, fi.Name);

            WWW www = new WWW(filePath);
            if (!www.isDone)
            {
                yield return null;
            }

            Texture2D texture = www.texture;
            Sprite aiSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Debug.Log(fi.Name +" sprite created");

            SpriteHolderClass temp = new SpriteHolderClass();

            temp.NameString = fi.Name;
            temp.LoadedSprite = aiSprite;

            AIProfileSprite.Add(temp);
        }

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

}
