using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem_script
{
    public static void SaveData(PlayerInfoManager_script player)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.skm";
        FileStream stream = new FileStream(path,FileMode.Create);
        
        PlayerData_script playerData = new PlayerData_script(player);

        bf.Serialize(stream, playerData);

        stream.Close();
    }

    public static PlayerData_script LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.skm";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData_script playerdata = bf.Deserialize(stream) as PlayerData_script;

            stream.Close();

            return playerdata;
        }
        else
        {
            Debug.Log("SaveSystem_script: LoadPlayer: No save data found at - " +path);
            return null;
        }
    }

    public static bool FileExist()
    {
        string path = Application.persistentDataPath + "/player.skm";
        return File.Exists(path);
    }

    public static void DeleteSaveFiles()
    {
        string path = Application.persistentDataPath + "/player.skm";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
