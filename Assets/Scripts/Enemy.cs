using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Flashlight flash;
    private float distToPlayer;

    #region General
    [Header("General")]
    public Transform player;
    public GameObject enemyPrefab;
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
        flash = FindObjectOfType<Flashlight>();
        player = FindObjectOfType<Player>().GetComponent<Transform>();
        follower = this.GetComponent<Follower>();

        GameObject[] nodeObj = GameObject.FindGameObjectsWithTag("Node");
        nodes.allNodes = new Node[nodeObj.Length];
        spawnPoints = new Vector2[nodeObj.Length];
        for (int i = 0; i < nodeObj.Length; i++)
        {
            nodes.allNodes[i] = nodeObj[i].GetComponent<Node>();
            spawnPoints[i] = nodeObj[i].GetComponent<Node>().transform.position;
        }
            

        originalSpeed = follower.m_Speed;
    }

    
    private void Update()
    {
        distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정

        ChasePlayer();

        if (nodes.isNodeChanged)
        {
            if (isRespawned || Random.Range(0, 3) == 0) // 리스폰 후, 혹은 랜덤으로 2초간 멈칫
                StartCoroutine("DelayPathFinding");
            else
                RePathFinding();
        }

        if (flash.isEnemyDead) 
        {
            RespawnEnemy();
        }
    }


    public void RespawnEnemy() // 리스폰
    {
        nodes.allNodes[nodes.minIndex].connections.Remove(nodes.thisNode); // 노드끼리의 연결 초기화

        Vector2 spawn = Vector2.zero;
        for (int i = 0; i < nodes.allNodes.Length; i++)
        {
            spawn = spawnPoints[Random.Range(0, spawnPoints.Length)]; // 리스폰 장소 설정
            if (Vector2.Distance(spawn, player.position) > respawnPointMinDistance) // 리스폰 거리 조건을 만족시킬 경우
                break;
        }

        if (spawn == Vector2.zero) // 개발자 옵션 : 리스폰 거리 조건을 만족시키는 리스폰 포인트를 못찾을 경우
            Debug.Log("Can not find appropriate spawn point");

        transform.position = spawn;

        Path path = graph.GetShortestPath(nodes.thisNode, nodes.thisNode); // PathFinding 초기화
        follower.Follow(path);

        follower.m_Speed = originalSpeed; // 속도 초기화 (발견 및 추격 속도 -> 일반 속도)

        isRespawned = true;
        isPlayerFounded = false;
        flash.isEnemyDead = false;
    }

    public void RePathFinding() // AI : 길 찾기
    {
        Path path = graph.GetShortestPath(nodes.thisNode, targetNode);
        follower.Follow(path);
        nodes.isNodeChanged = false;
    }

    public void ChasePlayer()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        bool playerInSight = Physics2D.Raycast(transform.position, dir, distToPlayer, 1 << LayerMask.NameToLayer("Obstacle")).collider == null ? true : false; // 플레이어가 벽에 가려지지 않았는가
        bool playerInRecognitionRange = distToPlayer < recognitionRange; // 플레이어가 인식범위 내에 있는가

        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * recognitionRange, Color.red);

        if (playerInSight && playerInRecognitionRange)
        {
            StopCoroutine("DelayPathFinding");
            follower.m_Speed = chasingSpeed;
            isPlayerFounded = true;

            if (isRespawned)
                isRespawned = false;
        }
    }

    IEnumerator DelayPathFinding()
    {
        yield return new WaitForSeconds(2);
        RePathFinding();
        isRespawned = false;
    }
}
