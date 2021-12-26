using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject globalLight;
    public Enemy enemy;
    public Player player;
    public InteractManager inter;

    public AudioSource BGM;
    public AudioClip bgmClip;

    public GameObject pausePanel;
    public bool isPause = false;
    public bool init = false;

    public Image gameOverPanel;

    public FloorData floor;
    public static bool isLoadedGame;

    float canRestartTimer = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        globalLight.SetActive(false);
        enemy = FindObjectOfType<Enemy>();
        gameOverPanel = GameObject.Find("GameOver").GetComponent<Image>();
        inter = FindObjectOfType<InteractManager>();
        floor = new FloorData();
        BGM = FindObjectOfType<GameManager>().GetComponent<AudioSource>();

        if (isLoadedGame)
        {
            LoadGame();
            isLoadedGame = false;
        }

        InitProperties();
    }

    private void Update()
    {
        if (!init) 
        {
            InitProperties();
            init = true;
        }
        if (isPause)
        {
            enemy.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
            {
                pausePanel.gameObject.SetActive(true);
                isPause = true;
            }

            else
            {
                ResumeButton();
            }
        }

        if (player.isDead)
        {
            canRestartTimer += Time.deltaTime;
            if (canRestartTimer > 2)
                RestartGame();
        }
        else
            gameOverPanel.gameObject.SetActive(false);
    }

    public void InitProperties()
    {
        pausePanel.SetActive(false);
        inter.mapImg.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        BGM.PlayOneShot(bgmClip, 0.05f);
        gameObject.GetComponent<AudioListener>().enabled = true;
        StartCoroutine("CoGameOver");
    }

    public void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string path = Application.persistentDataPath + "/SaveData.savedata";
            if (File.Exists(path))
            {
                isLoadedGame = true;
                SceneManager.LoadScene(floor.data[player.floor]);
            }
        }
    }

    public void ResumeButton()
    {
        pausePanel.gameObject.SetActive(false);
        isPause = false;
        enemy.gameObject.SetActive(true);
        enemy.RePathFinding();
    }

    public void SaveButton()
    {
        ResumeButton();
        SaveSystem.Save(player);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    
    public void LoadGame()
    {
        SaveData data = SaveSystem.Load();

        player.onTutorial = data.onTutorial;
        player.key = data.key;
        player.inRoom = data.inRoom;
        player.isGameStarted = data.isGameStarted;
        player.isGameCleared = data.isGameCleared;
        player.floor = data.floor;
        player.hasMap = data.hasMap;
        player.interactManager.audioListenerPlayer.enabled = data.isActivePlayerAudioListener;
        GameObject.Find(data.doorAudioListener).GetComponent<AudioListener>().enabled = !data.isActivePlayerAudioListener;

        Vector2 position;
        position.x = data.playerPosition[0];
        position.y = data.playerPosition[1];

        player.transform.position = position;
    }

    IEnumerator CoGameOver()
    {
        enemy.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        inter.enabled = false;

        gameOverPanel.gameObject.SetActive(true);
        foreach(TextMeshProUGUI text in gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>())
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                text.color = new Color(255, 255, 255, i);
                yield return null;
            }
        }
    }
}
