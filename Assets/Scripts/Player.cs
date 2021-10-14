using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 moveInput;

    public Transform flashlightSprite;
    public Quaternion flashlightDeltaAngle;


    public void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;

        if (moveInput.x == 0 || moveInput.y == 0) // 대각선 이동이 아닐 경우에만
        {
            if (moveInput.y < 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 0); // 아래쪽
            else if (moveInput.x > 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 90); // 왼쪽
            else if (moveInput.y > 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 180); // 위쪽
            else if (moveInput.x < 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 270); // 오른쪽

        }
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        flashlightSprite.rotation = Quaternion.Lerp(flashlightSprite.rotation, flashlightDeltaAngle, 0.4f);

    }
}
