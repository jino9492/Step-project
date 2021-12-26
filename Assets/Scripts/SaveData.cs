using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int floor;
    public float[] playerPosition;
    public bool[] key;
    public bool inRoom;
    public bool onTutorial;
    public bool isGameStarted;
    public bool isGameCleared;
    public bool hasMap;
    public bool isActivePlayerAudioListener;
    public string doorAudioListener;

    public SaveData(Player player)
    {
        onTutorial = player.onTutorial;
        floor = player.floor;
        key = player.key;
        inRoom = player.inRoom;
        hasMap = player.hasMap;
        isGameStarted = player.isGameStarted;
        isGameCleared = player.isGameCleared;
        isActivePlayerAudioListener = player.interactManager.audioListenerPlayer.enabled;
        doorAudioListener = player.interactManager.audioListener.gameObject.name;
        playerPosition = new float[2];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
    }
}