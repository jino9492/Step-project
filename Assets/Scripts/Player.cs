using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public TalkManager talkManager;

    public float moveSpeed;
    public Vector2 moveInput;

    public Vector2 lastDirection;
    public LayerMask layerMask;

    #region PathFinding
    public GraphController gc;
    public NodesInfo nodes;
    #endregion

    private void Start()
    {
        talkManager = FindObjectOfType<TalkManager>();
    }

    private void Update()
    {
        if (!talkManager.showPanel) //대화창있을때 움직 ㄴㄴ
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
        }

        Vector2 direction = new Vector2(moveInput.x, moveInput.y);
        if (direction != Vector2.zero)
        {
            lastDirection = direction;
        }

        //상호작용 (space)
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, -10), direction * 1.5f, Color.green);
        RaycastHit2D interObject = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), lastDirection, 1.5f, layerMask);
        if (Input.GetKeyDown(KeyCode.Space) && interObject.collider)
        {
            Debug.Log(interObject.collider.name);
            if (interObject.collider != null && interObject.collider.tag == "Obstacle")
            {
                
                talkManager.Talking(interObject.collider.gameObject);
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
