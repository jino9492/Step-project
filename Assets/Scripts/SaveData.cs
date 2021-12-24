using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //플레이어 위치, 아이템, 층
    public int floor;
    public float[] playerPosition;
    public bool[] key;
    public bool inRoom;
    public bool onTutorial;
    
    public SaveData(Player player)
    {
        onTutorial = player.onTutorial;
        floor = player.floor;
        key = player.key;
        inRoom = player.inRoom;
        playerPosition = new float[2];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
    }
}