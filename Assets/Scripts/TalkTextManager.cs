using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTextManager : MonoBehaviour
{
    private Dictionary<int,string[]> talkData = new Dictionary<int, string[]>();
    private ObjectId[] objectCollection;

    private void Awake()
    {
        objectCollection = FindObjectsOfType<ObjectId>();

        foreach(ObjectId objectElement in objectCollection)
        {
            if (objectElement.text.Length > 0)
            {
                AddData(objectElement);
            }
        }
    }

    public void AddData(ObjectId objectElement)
    {
        //기본 대사 - talkData.Add(오브젝트 아이디 * 100 + 오브젝트 넘버, "텍스트")
        talkData.Add(objectElement.objectId * 100 + objectElement.objectNumber, objectElement.text);

    }

    public string[] GetText(int number)
    {
        return talkData[number];
    }
}
