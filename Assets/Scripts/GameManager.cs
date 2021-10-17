using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject globalLight;

    private void Start()
    {
        globalLight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E를 눌러 불 좀 켜줄래?
            globalLight.SetActive(!globalLight.activeSelf);
    }
}
