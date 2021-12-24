using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public string SceneToLoad;

    void Start()
    {
        string path = Application.persistentDataPath + "/SaveData.savedata";
        if (!File.Exists(path))
        {
            GameObject.Find("LoadGameButton").GetComponent<Image>().color = new Color(100, 100, 100);
        }
    }

    
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/SaveData.savedata";
        if (File.Exists(path))
        {

        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
