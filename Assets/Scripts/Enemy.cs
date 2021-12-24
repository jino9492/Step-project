using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Flashlight flash;
    private InteractManager interactManager;
    private float distToPlayer;

    #region Audio
    private AudioSource audio;
    public AudioClip walkingSound;
    public AudioClip runningSound;
    #endregion

    float animTimer = 0;

    #region General
    [Header("General")]
    public Player player;
    public GameObject enemyPrefab;
    public GameManager gm;
    public Vector2[] spawnPoints;
    public float respawnPointMinDistance;
    public bool isRespawned;
    public bool isPlayerFounded;
    #endregion

    #region PathFinding
    [Header("PathFinding")]
    public Graph graph;
    public Node targetNode;
    public Follower follower;
    public GraphController gc;
    public NodesInfo nodes;
    #endregion

    #region PlayerChasing
    [Header("PlayerChasing")]
    public float chasingSpeed;
    public float recognitionRange;
    public float originalSpeed;
    #endregion

    private void Start()
    {
        interactManager = FindObjectOfType<InteractManager>();
        flash = FindObjectOfType<Flashlight>();
        player = FindObjectOfType<Player>();
        follower = GetComponent<Follower>();
        gm = FindObjectOfType<GameManager>();
        audio = FindObjectOfType<Enemy>().GetComponent<AudioSource>();
        gc = FindObjectOfType<GraphController>();
        graph = FindObjectOfType<Graph>();

        targetNode = GameObject.Find("Player").GetComponent<Node>();

        GameObject[] nodeObj = GameObject.FindGameObjectsWithTag("Node");
        nodes.allNodes = new Node[nodeObj.Length];
        spawnPoints = new Vector2[nodeObj.Length];
        for (int i = 0; i < nodeObj.Length; i++)
        {
            nodes.allNodes[i] = nodeObj[i].GetComponent<Node>();
            spawnPoints[i] = nodeObj[i].GetComponent<Node>().transform.position;
        }

        audio.clip = walkingSound;

        RePathFinding();
    }

    
    private void Update()
    {
        Vector2 distVector = player.transform.position - transform.position;
        float angle = Mathf.Atan2(distVector.y, distVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정
        CheckPlayerInSight();

        animTimer += Time.deltaTime;

        if (animTimer > 0.08)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            animTimer = 0;
        }

        
        if (nodes.isNodeChanged && !isPlayerFounded)
        {
            // 리스폰 후, 혹은 랜덤으로 2초간 멈칫
            if (player.inRoom)
            {
                Node randNode;
                do{
                    randNode = nodes.allNodes[Random.Range(0, nodes.allNodes.Length - 1)];
                } while (nodes.thisNode.connections.Contains(randNode));

                if (Random.Range(0, 2) == 0)
                    StartCoroutine("DelayPathFinding", randNode);
                else
                    RePathFinding(randNode);
            }
            else
            {
                if (Random.Range(0, 2) == 0 || isRespawned)
                    StartCoroutine("DelayPathFinding");
                else
                    RePathFinding();
            }

            nodes.isNodeChanged = false;
        }

        if (flash.isEnemyDead) 
        {
            RespawnEnemy();
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerFounded)
        {
            ChasePlayer();
        }
    }

    public void RespawnEnemy() // 리스폰
    {
        nodes.allNodes[nodes.minIndex].connections.Remove(nodes.thisNode); // 노드끼리의 연결 초기화

        Vector2 spawn = Vector2.zero;
        for (int i = 0; i < nodes.allNodes.Length; i++)
        {
            spawn = spawnPoints[Random.Range(0, spawnPoints.Length - 1)]; // 리스폰 장소 설정
            if (Vector2.Distance(spawn, player.transform.position) > respawnPointMinDistance) // 리스폰 거리 조건을 만족시킬 경우
                break;
        }

        if (spawn == Vector2.zero) // 개발자 옵션 : 리스폰 거리 조건을 만족시키는 리스폰 포인트를 못찾을 경우
            Debug.LogError("Can not find appropriate spawn point");

        transform.position = spawn;

        ResetPathFinding();

        isRespawned = true;
        isPlayerFounded = false;
        flash.isEnemyDead = false;

        audio.clip = walkingSound;
    }

    public void RePathFinding() // AI : 길 찾기
    {
        Path path = graph.GetShortestPath(nodes.thisNode, targetNode);
        follower.Follow(path);
    }

    public void RePathFinding(Node node)
    {
        Path path = graph.GetShortestPath(nodes.thisNode, node);
        follower.Follow(path);
    }

    public void ResetPathFinding()
    {
        Path path = graph.GetShortestPath(nodes.thisNode, nodes.thisNode); // PathFinding 초기화
        Debug.Log(path);
        follower.Follow(path);
    }

    public void CheckPlayerInSight()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        bool playerInSight = Physics2D.Raycast(transform.position, dir, distToPlayer, 1 << LayerMask.NameToLayer("Obstacle")).collider == null ? true : false; // 플레이어가 벽에 가려지지 않았는가
        bool playerInRecognitionRange = distToPlayer < recognitionRange; // 플레이어가 인식범위 내에 있는가

        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * recognitionRange, Color.red);

        if (playerInSight && playerInRecognitionRange && !isPlayerFounded)
        {
            isPlayerFounded = true;

            audio.Stop();
            audio.clip = runningSound;
            audio.Play();

            if (isRespawned)
                isRespawned = false;
        }
    }

    public void ChasePlayer()
    {
        StopCoroutine("DelayPathFinding");
        ResetPathFinding();
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chasingSpeed);
    }

    IEnumerator DelayPathFinding()
    {
        audio.Stop();
        ResetPathFinding();
        yield return new WaitForSeconds(2);
        audio.Play();
        RePathFinding();
        isRespawned = false;
    }

    IEnumerator DelayPathFinding(Node node)
    {
        audio.Stop();
        ResetPathFinding();
        yield return new WaitForSeconds(2);
        audio.Play();
        RePathFinding(node);
        isRespawned = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gm.GameOver();
        }
    }
}
