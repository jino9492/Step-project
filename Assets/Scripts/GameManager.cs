using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject globalLight;
    public Enemy enemy;
    public Player player;

    public GameObject pausePanel;
    public bool isPause = false;

    private void Start()
    {
        globalLight.SetActive(false);
        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
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
            
    }

    public void GameOver()
    {
        enemy.gameObject.SetActive(false);
        player.gameObject.SetActive(false);

        RestartGame(); // 임시
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        player.floor = data.floor;

        Vector2 position;
        position.x = data.playerPosition[0];
        position.y = data.playerPosition[1];

        player.transform.position = position;
    }
}
