using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class InteractManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public ObjectId objectIdScript;
    public Animator anim;
    public TalkTextManager talkTextManager;
    public GameObject flashlight;

    public TextMeshProUGUI talkingText;
    public TextMeshProUGUI talkingTitle;
    public bool showPanel = false;
    public int page = 0;

    public AudioClip doorOpenSFX;
    public AudioClip doorLockSFX;
    public AudioSource audioSource;

    public int objectId;
    public int objectNumber;

    public enum objectList
    {
        lockedDoor, // 0
        unlockedDoor, // 1
        document, // 2
        key, // 3
        openableDoor, // 4
        eventObject = 10, // 10
    }

    public SpriteRenderer fadeImg;

    private void Start()
    {
        flashlight = FindObjectOfType<Flashlight>().gameObject;
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        objectIdScript = FindObjectOfType<ObjectId>();
        talkTextManager = FindObjectOfType<TalkTextManager>();
        fadeImg = GameObject.Find("FadeImage").GetComponent<SpriteRenderer>();
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
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

        talkingTitle.text = scanObject.GetComponent<ObjectId>().title;

        if (page == 0 || page >= text.Length)
            showPanel = !showPanel;

        if (page < text.Length)
        {
            talkingText.text = talkTextManager.GetText(objectId * 100 + objectNumber)[page++];
        }
        else
            page = 0;
    }

    public void OpenDoor(GameObject scanObject)
    {
        Vector2 location = scanObject.GetComponent<ObjectId>().location;
        AudioListener audioListener = scanObject.GetComponent<AudioListener>();
        AudioListener audioListenerPlayer = player.GetComponent<AudioListener>();
        AudioLowPassFilter audioFilterEnemy = enemy.GetComponent<AudioLowPassFilter>();

        if (!enemy.isPlayerFounded)
        {
            player.inRoom = !player.inRoom;
            Debug.Log(scanObject);
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
            Destroy(scanObject);
    }

    public void OpenDoorByKey(GameObject scanObject)
    {
        ObjectId obj = scanObject.GetComponent<ObjectId>();

        if (player.key[obj.objectNumber])
        {
            if (player.inRoom)
                player.inRoom = !player.inRoom;
            OpenDoor(scanObject);
        }
        else
        {
            if (page == 0)
                LockedDoor();

            Talking(scanObject);
        }
    }

    public void Event(GameObject eventObject)
    {
        switch (eventObject.GetComponent<ObjectId>().objectNumber)
        {
            case 0:
                StartCoroutine("Event1", eventObject);
                flashlight.SetActive(true);
                flashlight.GetComponent<Flashlight>().CalcDirection(player.lastDirection.x, player.lastDirection.y);
                Destroy(eventObject);
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    IEnumerator ChangeRoom(Vector2 location)
    {
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
    }

    // 손전등과 지도를 얻는 이벤트
    IEnumerator Event1(GameObject eventObject)
    {
        // 구현하기
        yield return null;
    }
}   
