using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Enemy enemy;
    public Rigidbody2D rb;
    public InteractManager interactManager;
    public GameManager gm;
    public AudioSource audioSource;
    public TutorialManager tutorial;

    public Animator anim;
    public bool isDirectionChanged;

    public float moveSpeed;
    public Vector2 moveInput;

    public Vector2 lastDirection;
    public LayerMask layerMask;

    public bool isDead;

    [Header("Save Data")]
    #region SaveData
    public int floor = 60;
    public bool onTutorial;
    public bool inRoom;
    public bool[] key;
    public bool isGameStarted; // 괴물 활동 시작 시기 (철창 너머로 괴물 처음 봤을 때 기준)
    public bool isGameCleared;
    #endregion

    [Header("Path Finding")]
    #region PathFinding
    public GraphController gc;
    public NodesInfo nodes;
    #endregion

    private void Start()
    {
        tutorial = FindObjectOfType<TutorialManager>();
        gc = FindObjectOfType<GraphController>();
        enemy = FindObjectOfType<Enemy>();
        interactManager = FindObjectOfType<InteractManager>();
        gm = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        key = new bool[key.Length - 1];
        Debug.Log(key[0]);

        GameObject[] nodeObj = GameObject.FindGameObjectsWithTag("Node");
        nodes.allNodes = new Node[nodeObj.Length];
        for (int i = 0; i < nodeObj.Length; i++)
            nodes.allNodes[i] = nodeObj[i].GetComponent<Node>();
    }

    private void Update()
    {
        // enemy가 처음부터 비활성화 되어있을 때
        if (enemy != null)
        {
            if (!isGameStarted)
                enemy.gameObject.SetActive(false);
            else
                enemy.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Enemy's state is false");
        }

        // 튜토리얼 상태 체크
        if (onTutorial)
        {
            tutorial.gameObject.SetActive(true);
            if (tutorial.tutorialStep == -1)
                tutorial.Tutorial();
        }
        else
            tutorial.gameObject.SetActive(false);

        //대화창있을때 움직 ㄴㄴ
        if (!interactManager.showPanel)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        // 애니메이션 및 사운드
        Vector2 direction = new Vector2(moveInput.x, moveInput.y);
        if (direction != Vector2.zero)
        {
            if (direction == lastDirection)
                isDirectionChanged = false;
            else
                isDirectionChanged = true;

            lastDirection = direction;
            if (!audioSource.isPlaying)
                audioSource.Play();

            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
            audioSource.Stop();
        }

        // 애니메이션
        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);

        if (isDirectionChanged)
        {
            anim.SetBool("IsDirectionChanged", true);
            isDirectionChanged = false;
        }
        else
        {
            anim.SetBool("IsDirectionChanged", false);
        }

        //상호작용 (space)
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, -10), direction * 5f, Color.green);
        RaycastHit2D interObject = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), lastDirection, 5f, layerMask);
        if (Input.GetKeyDown(KeyCode.Space) && interObject.collider)
        {
            ObjectId objectData = interObject.collider.gameObject.GetComponent<ObjectId>();
            if (!objectData)
            {
                Debug.LogError("Object ID Does Not Exist : " + interObject.collider.name);
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

                    case (int)InteractManager.objectList.key:
                        interactManager.GetKey(interObject.collider.gameObject);
                        break;

                    case (int)InteractManager.objectList.openableDoor:
                        interactManager.OpenDoorByKey(interObject.collider.gameObject);
                        break;

                    case (int)InteractManager.objectList.lever:
                        interactManager.Lever(interObject.collider.gameObject);
                        break;

                    case (int)InteractManager.objectList.elevator:
                        interactManager.Elevator(interObject.collider.gameObject);
                        break;

                    case (int)InteractManager.objectList.eventObject:
                        interactManager.Event(interObject.collider.gameObject);
                        break;

                }
                
            }
        }

        if (nodes.isNodeChanged)
            nodes.isNodeChanged = false;

        // 방 안에 있을 때 간선 연결 해제
        if (!inRoom)
        {
            if (nodes.thisNode.connections.Count == 0)
            {
                nodes.thisNode.connections.Add(nodes.thisNode);
                gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged, true);
            }
            gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정
        }
        else if (nodes.thisNode.connections.Count > 0)
        {
            nodes.thisNode.connections[0].connections.Remove(nodes.thisNode);
            nodes.thisNode.connections.Clear();
        }
        else if (enemy.GetComponent<Follower>().m_Current == nodes.thisNode)
        {
            enemy.nodes.isNodeChanged = true;
        }
            
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
