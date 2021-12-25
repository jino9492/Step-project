﻿using System.Collections;
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

    public GameObject pausePanel;
    public bool isPause = false;

    public Image gameOverPanel;

    public FloorData floor;
    public static bool isLoadedGame;

    float canRestartTimer = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        if (isLoadedGame)
        {
            LoadGame();
            isLoadedGame = false;
        }

        globalLight.SetActive(false);
        enemy = FindObjectOfType<Enemy>();
        gameOverPanel = GameObject.Find("GameOver").GetComponent<Image>();
        inter = FindObjectOfType<InteractManager>();
        floor = new FloorData();

        InitProperties();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E를 눌러 불 좀 켜줄래?
            globalLight.SetActive(!globalLight.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause == false)
            {
                pausePanel.gameObject.SetActive(true);
                isPause = true;
            }

            else
            {
                pausePanel.gameObject.SetActive(false);
                isPause = false;
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
        StartCoroutine("CoGameOver");
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string path = Application.persistentDataPath + "/SaveData.savedata";
            if (File.Exists(path))
            {
                isLoadedGame = true;
                SceneManager.LoadScene(floor.data[player.floor]);
            }
        }
    }

    public void SaveButton()
    {
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
