using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHandler : MonoBehaviour
{
    public Transform camTransform;
    public Transform playerTransform;
    public Flashlight flash;
    public float camAdj;

    private const float camSpeed = 0.1f; // 여기서 Lerp값 수정
    private Vector3 camOffset;

    private void Start()
    {
        camOffset = new Vector3(0, 0, camTransform.position.z);
    }

    private void FixedUpdate()
    {
        if (flash.flashlightDeltaAngle == Quaternion.Euler(0, 0, 0)) // 아래
            camTransform.position = Vector3.Lerp(camTransform.position, playerTransform.position + camOffset + (-Vector3.up * camAdj), camSpeed);
        else if (flash.flashlightDeltaAngle == Quaternion.Euler(0, 0, 180)) // 위
            camTransform.position = Vector3.Lerp(camTransform.position, playerTransform.position + camOffset + (Vector3.up * camAdj), camSpeed);
        else // 좌, 우
            camTransform.position = Vector3.Lerp(camTransform.position, playerTransform.position + camOffset, camSpeed);
    }
}
