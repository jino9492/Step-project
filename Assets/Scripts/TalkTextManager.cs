using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTextManager : MonoBehaviour
{
    public Dictionary<int,string> talkData = new Dictionary<int, string>();

    private void Awake()
    {
        AddData();
    }

    public void AddData()
    {
        //기본 대사 - talkData.Add(오브젝트아이디, "텍스트")
        talkData.Add(0, "테스트 데이터");

    }

    public string GetText(int id)
    {
        return talkData[id];
    }
}
