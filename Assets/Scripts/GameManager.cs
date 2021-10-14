using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Fog;

    private void Start()
    {
        Fog.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // F를 눌러 불 좀 켜줄래?
            Fog.SetActive(!Fog.activeSelf);
    }
}
