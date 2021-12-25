using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour
{
    public Dictionary<int, string> data = new Dictionary<int, string>();
    
    public FloorData()
    {
        data.Add(60, "FirstFloor");
    }
}
