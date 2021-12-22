using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;


public class SaveSystem
{
    public static void Save(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.savedata";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/SaveData.savedata";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("세이브 데이터 없음"+path);
            return null;
        }
    }
}
