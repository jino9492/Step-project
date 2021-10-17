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

        if (nodes.isNodeChanged)
            nodes.isNodeChanged = false;

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

    }
}
