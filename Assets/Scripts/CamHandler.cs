using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHandler : MonoBehaviour
{
    public Transform camTransform;
    public Transform playerTransform;
    public Vector3 camOffset;

    private void Start()
    {
        camOffset = new Vector3(0, 0, camTransform.position.z);
    }

    private void FixedUpdate()
    {
        camTransform.position = Vector3.Lerp(camTransform.position, playerTransform.position + camOffset, 0.5f);
    }
}
