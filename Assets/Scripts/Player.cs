using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 moveInput;

    #region PathFinding
    public GraphController gc;
    public NodesInfo nodes;
    #endregion


    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;

        //상호작용
        Vector2 direction = new Vector2(moveInput.x, moveInput.y);
        Debug.DrawRay(transform.position, direction * 1.5f, Color.green);
        RaycastHit2D interObject = Physics2D.Raycast(transform.position, direction * 3);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (interObject.collider != null)
            {
                Debug.Log(interObject.collider.name);
            }
        }

        if (nodes.isNodeChanged)
            nodes.isNodeChanged = false;

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

    }
}
