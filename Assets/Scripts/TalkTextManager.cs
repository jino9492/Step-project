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
        //기본 대사 - talkData.Add(오브젝트 아이디 * 100 + 오브젝트 넘버, "텍스트")
        talkData.Add(200, "테스트 데이터");

    }

    public string GetText(int number)
    {
        return talkData[number];
    }
}
