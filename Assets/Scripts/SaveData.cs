using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int floor;
    public int keyRoomStack;
    public float[] playerPosition;
    public bool[] key;
    public bool inRoom;
    public bool onTutorial;
    public bool isGameStarted;

    public SaveData(Player player)
    {
        onTutorial = player.onTutorial;
        keyRoomStack = player.keyRoomStack;
        floor = player.floor;
        key = player.key;
        inRoom = player.inRoom;
        isGameStarted = player.isGameStarted;
        playerPosition = new float[2];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
    }
}