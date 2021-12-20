using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Player player;
    public InteractManager interactManager;
    public Transform flashlightSprite;
    public Quaternion flashlightDeltaAngle;

    public bool isEnemyDead;

    public float timer = 0; // 괴물이 시야 안에 들어와 있는 시간
    public float enemySurvivalTime; 

    private void Start()
    {
        interactManager = FindObjectOfType<InteractManager>();
    }

    private void Update()
    {

        if (player.moveInput.x == 0 || player.moveInput.y == 0) // 대각선 이동이 아닐 경우에만
        {
            if (player.moveInput.y < 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 0); // 아래쪽
            else if (player.moveInput.x > 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 90); // 왼쪽
            else if (player.moveInput.y > 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 180); // 위쪽
            else if (player.moveInput.x < 0)
                flashlightDeltaAngle = Quaternion.Euler(0, 0, 270); // 오른쪽
        }
    }
    private void FixedUpdate()
    {
        if (!interactManager.showPanel) //판넬있을때 움직 ㄴㄴ
        {
            flashlightSprite.rotation = Quaternion.Lerp(flashlightSprite.rotation, flashlightDeltaAngle, 0.4f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            timer += Time.deltaTime;

            if (timer > enemySurvivalTime)
            {
                isEnemyDead = true;
                timer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            timer = 0;
    }
}
