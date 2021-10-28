using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class TalkManager : MonoBehaviour
{
    public Player player;
    public ObjectId objectIdScript;
    public Animator anim;
    public TalkTextManager talkTextManager;

    public TextMeshProUGUI talkingText;
    public bool showPanel = false;

    public int objectId;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        objectIdScript = FindObjectOfType<ObjectId>();
        talkTextManager = FindObjectOfType<TalkTextManager>();
    }

    private void Update()
    {
        anim.SetBool("showPanel", showPanel);
    }

    public void Talking(GameObject scanObject)
    {
        objectId = scanObject.GetComponent<ObjectId>().objectId;

        talkingText.text = talkTextManager.GetText(objectId);

        if(showPanel != true)
        {
            showPanel = true;
        }
        else
        {
            showPanel = false;
        }
    }
}   
