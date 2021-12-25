using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectId : MonoBehaviour
{
    public int objectId;
    public int objectNumber;
    [Header("Settings")]
    public Vector2 location;
    public string[] title;
    public string[] text;
    public int floor;

    public bool isAdjacentToHallway;
}
