using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //플레이어 위치, 아이템, 층
    public int floor;
    public float[] playerPosition;
    
    public SaveData(Player player)
    {
        floor = player.floor;
        playerPosition = new float[2];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
    }
}