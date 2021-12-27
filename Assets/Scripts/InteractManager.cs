using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public ObjectId objectIdScript;
    public Animator anim;
    public TalkTextManager talkTextManager;
    public GameObject flashlight;
    public GameManager gm;

    public TextMeshProUGUI talkingText;
    public TextMeshProUGUI talkingTitle;
    public bool showPanel = false;
    public int page = 0;
    public bool showMap = false;

    public AudioClip doorOpenSFX;
    public AudioClip doorLockSFX;
    public AudioClip documentSFX;
    public AudioClip leverSFX;
    public AudioClip elevatorSFX;
    public AudioSource audioSource;

    public AudioListener audioListener;
    public AudioListener audioListenerPlayer;
    AudioLowPassFilter audioFilterEnemy;

    public int objectId;
    public int objectNumber;

    public enum objectList
    {
        lockedDoor, // 0
        unlockedDoor, // 1
        document, // 2
        key, // 3
        openableDoor, // 4
        lever, // 5
        elevator, // 6
        eventObject = 10, // 10
    }

    public Image fadeImg;
    public Image mapImg;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        audioListenerPlayer = player.GetComponent<AudioListener>();
        flashlight = FindObjectOfType<Flashlight>().gameObject;
        enemy = FindObjectOfType<Enemy>();
        objectIdScript = FindObjectOfType<ObjectId>();
        talkTextManager = FindObjectOfType<TalkTextManager>();
        fadeImg = GameObject.Find("FadeImage").GetComponent<Image>();
        mapImg = GameObject.Find("Map").GetComponent<Image>();
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManager>();
        audioFilterEnemy = enemy.GetComponent<AudioLowPassFilter>();
    }

    private void Update()
    {
        anim.SetBool("showPanel", showPanel);
    }

    public void Talking(GameObject scanObject)
    {
        objectId = scanObject.GetComponent<ObjectId>().objectId;
        objectNumber = scanObject.GetComponent<ObjectId>().objectNumber;
        string[] text = talkTextManager.GetText(objectId * 100 + objectNumber);

        if (page == 0 || page >= text.Length)
            showPanel = !showPanel;

        if (page < text.Length)
        {
            StopCoroutine("DelayText");
            talkingTitle.text = scanObject.GetComponent<ObjectId>().title[page];
            StartCoroutine("DelayText", talkTextManager.GetText(objectId * 100 + objectNumber)[page++]);
        }
        else
            page = 0;
    }

    public void OpenDoor(GameObject scanObject)
    {
        Vector2 location = scanObject.GetComponent<ObjectId>().location;

        if (!player.inRoom)
        {
            audioListener = scanObject.GetComponent<AudioListener>();
        }

        if (!enemy.isPlayerFounded)
        {
            player.inRoom = !player.inRoom;

            if (audioListener != null) {
                if (player.inRoom)
                {
                    audioFilterEnemy.enabled = true;
                    audioListener.enabled = true;
                    audioListenerPlayer.enabled = false;
                }
                else
                {
                    audioFilterEnemy.enabled = false;
                    audioListener.enabled = false;
                    audioListenerPlayer.enabled = true;
                }
            }

            StartCoroutine("ChangeRoom", location);
        }
    }

    public void LockedDoor()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = doorLockSFX;
            audioSource.Play();
        }
    }

    public void GetKey(GameObject scanObject)
    {
        ObjectId obj = scanObject.GetComponent<ObjectId>();

        Talking(scanObject);
        player.key[obj.objectNumber] = true;
        if (page == 0)
        {
            SaveSystem.Save(player);
            Destroy(scanObject);
        }
    }

    public void OpenDoorByKey(GameObject scanObject)
    {
        ObjectId obj = scanObject.GetComponent<ObjectId>();

        if (player.key[obj.objectNumber])
        {
            if (obj.isAdjacentToHallway)
            {
                OpenDoor(scanObject);
            }
            else
            {
                StartCoroutine("ChangeRoom", obj.location);
            }
        }
        else
        {
            if (page == 0)
                LockedDoor();

            Talking(scanObject);
        }
    }

    public void Lever(GameObject scanObject)
    {
        if (!player.isGameCleared)
        {
            player.isGameCleared = true;
            player.isGameStarted = false;

            flashlight.SetActive(false);
            enemy.gameObject.SetActive(false);
            gm.globalLight.SetActive(true);

            audioSource.clip = leverSFX;
            audioSource.Play();

            Talking(scanObject);
        }
        else
        {
            Talking(scanObject);
        }

        if (page == 0)
        {
            SaveSystem.Save(player);
        }
    }

    public void Elevator(GameObject scanObject)
    {
        if (player.isGameCleared)
            StartCoroutine("ChangeStage", scanObject.GetComponent<ObjectId>().floor);
        else
            Talking(scanObject);
    }

    public void Event(GameObject eventObject)
    {
        switch (eventObject.GetComponent<ObjectId>().objectNumber)
        {
            case 0:
                
                Talking(eventObject);
                if (page == 0)
                {
                    if (!flashlight.activeSelf)
                    {
                        flashlight.SetActive(true);
                        flashlight.GetComponent<Flashlight>().CalcDirection(player.lastDirection.x, player.lastDirection.y);
                    }
                    SaveSystem.Save(player);
                    Destroy(eventObject);
                }
                break;
            case 1:
                if (!mapImg.gameObject.activeSelf)
                    player.hasMap = true;

                Talking(eventObject);
                if (page == 0)
                    Destroy(eventObject);
                break;
            case 2:
                player.isGameStarted = true;
                if (!enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(true);
                    enemy.nodes.isNodeChanged = true;
                    StartCoroutine("Event3", eventObject);
                }
                Talking(eventObject);

                if (page == 0)
                    Destroy(eventObject);

                break;
        }
    }

    public void OpenMap()
    {
        if (player.hasMap)
            showMap = !showMap;

        if (showMap)
            mapImg.gameObject.SetActive(true);
        else
            mapImg.gameObject.SetActive(false);
    }

    IEnumerator DelayText(string text)
    {
        float delaySec = 0.025f;
        for (int i = 0; i < text.Length; i++)
        {
            talkingText.text = text.Substring(0, i + 1);
            yield return new WaitForSeconds(delaySec);
        }
    }

    IEnumerator ChangeRoom(Vector2 location)
    {
        fadeImg.gameObject.SetActive(true);
        if (enemy.gameObject.activeSelf)
            enemy.StartCoroutine("DelayPathFinding", enemy.nodes.thisNode.connections[0]);
        player.enabled = false;

        // 효과음 오디오 소스는 메인 카메라에 달려있음.
        audioSource.clip = doorOpenSFX;
        audioSource.Play();

        for (float i = 0; i <= 1; i += Time.deltaTime * 5)
        {
            fadeImg.color = new Color(0, 0, 0, i);
            yield return null;
        }

        fadeImg.color = new Color(0, 0, 0, 1);
        player.transform.position = location;

        yield return new WaitForSecondsRealtime(1);

        for (float i = 1; i >= 0; i -= Time.deltaTime * 5)
        {
            fadeImg.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fadeImg.color = new Color(0, 0, 0, 0);

        player.enabled = true;
        fadeImg.gameObject.SetActive(false);
    }

    IEnumerator ChangeStage(int floor)
    {
        fadeImg.gameObject.SetActive(true);
        player.enabled = false;

        // 효과음 오디오 소스는 메인 카메라에 달려있음.

        for (float i = 0; i <= 1; i += Time.deltaTime * 5)
        {
            fadeImg.color = new Color(0, 0, 0, i);
            yield return null;
        }

        fadeImg.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(2);
        audioSource.clip = elevatorSFX;
        audioSource.Play();
        yield return new WaitForSeconds(4);

        FloorData floorData = new FloorData();
        player.floor = floor;

        SaveSystem.Save(player);

        SceneManager.LoadScene(floorData.data[player.floor]);
    }

    IEnumerator Event3(GameObject eventObject)
    {
        yield return new WaitUntil(() =>
        {
            return Input.GetKeyDown(KeyCode.Z);
        });

        Event(eventObject);
    }
}   
