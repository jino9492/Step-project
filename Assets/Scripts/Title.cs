using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public string SceneToLoad;
    public Button loadGameButton;

    public FloorData floor;

    void Start()
    {
        loadGameButton = GameObject.Find("LoadGameButton").GetComponent<Button>();
        string path = Application.persistentDataPath + "/SaveData.savedata";
        if (!File.Exists(path))
        {
            ColorBlock colors = loadGameButton.colors;
            colors.normalColor = new Color32(100, 100, 100, 255);
            colors.pressedColor = new Color32(100, 100, 100, 255);
            colors.selectedColor = new Color32(100, 100, 100, 255);
            colors.highlightedColor = new Color32(100, 100, 100, 255);
            loadGameButton.colors = colors;
        }

        floor = new FloorData();
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
            SaveData data = SaveSystem.Load();
            GameManager.isLoadedGame = true;
            SceneManager.LoadScene(floor.data[data.floor]);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
