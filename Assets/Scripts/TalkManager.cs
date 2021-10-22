using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class TalkManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI talkingText;
    public bool showPanel = false;
    public Animator anim;

    private void Start()
    {
        player = FindObjectOfType<Player>();    
    }

    private void Update()
    {
        anim.SetBool("showPanel", showPanel);
    }

    public void Talking(GameObject scanObject)
    {
        talkingText.text = scanObject.name + "오브젝트";
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
