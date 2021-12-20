using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public InteractManager interactManager;
    public GameManager gm;
    public AudioSource audio;

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
        interactManager = FindObjectOfType<InteractManager>();
        gm = FindObjectOfType<GameManager>();
        audio = GetComponent<AudioSource>();

        GameObject[] nodeObj = GameObject.FindGameObjectsWithTag("Node");
        nodes.allNodes = new Node[nodeObj.Length];
        for (int i = 0; i < nodeObj.Length; i++)
            nodes.allNodes[i] = nodeObj[i].GetComponent<Node>();
    }

    private void Update()
    {
        if (!interactManager.showPanel) //대화창있을때 움직 ㄴㄴ
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
        }

        Vector2 direction = new Vector2(moveInput.x, moveInput.y);
        if (direction != Vector2.zero)
        {
            lastDirection = direction;
            if (!audio.isPlaying)
                audio.Play();
        }
        else
            audio.Stop();

        //상호작용 (space)
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, -10), direction * 1.5f, Color.green);
        RaycastHit2D interObject = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), lastDirection, 1.5f, layerMask);
        if (Input.GetKeyDown(KeyCode.Space) && interObject.collider)
        {
            ObjectId objectData = interObject.collider.gameObject.GetComponent<ObjectId>();
            if (!objectData)
            {
                Debug.LogError("Object ID Does Not Exist");
            }
            else if (interObject.collider != null && interObject.collider.tag == "InteractiveObject")
            {
                switch (objectData.objectId) // 오브젝트 아이디에 따른 분류
                {
                    case (int)InteractManager.objectList.lockedDoor:
                        interactManager.LockedDoor();
                        break;

                    case (int)InteractManager.objectList.unlockedDoor:
                        interactManager.OpenDoor(interObject.collider.gameObject);
                        break;

                    case (int)InteractManager.objectList.document:
                        interactManager.Talking(interObject.collider.gameObject);
                        break;
                }
                
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
