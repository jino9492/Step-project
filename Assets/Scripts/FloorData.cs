using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    public Dictionary<int, string> data = new Dictionary<int, string>();
    
    public FloorData()
    {
        data.Add(60, "FirstFloor");
        data.Add(30, "SecondFloor");
        data.Add(0, "Ending");
    }
}
