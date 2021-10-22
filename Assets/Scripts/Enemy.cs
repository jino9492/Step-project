using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Flashlight flash;
    public Transform player;
    public GameObject enemyPrefab;
    public Vector2[] spawnPoints;
    public float respawnPointMinDistance;
    public bool isRespawned;
    public bool isPlayerFounded;

    [Header("PathFinding")]
    #region PathFinding
    public Graph graph;
    public Node targetNode;
    public Follower follower;
    public GraphController gc;
    public NodesInfo nodes;
    #endregion

    [Header("PlayerChasing")]
    public LayerMask layerMask;
    public float chaseSpeed;
    public float recognitionRange;

    private void Start()
    {
        flash = GameObject.Find("Flashlight").GetComponent<Flashlight>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        follower = this.GetComponent<Follower>();

        for (int i = 0; i < nodes.allNodes.Length; i++)
            spawnPoints[i] = nodes.allNodes[i].transform.position;

        
    }

    
    private void Update()
    {

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정

        if (nodes.isNodeChanged && !isPlayerFounded)
        {
            if (isRespawned)
                StartCoroutine("DelayPathFinding");
            else
                RePathFinding();
        }

        if (flash.isEnemyDead) 
        {
            RespawnEnemy();
        }

        ChasePlayer();
        
    }


    public void RespawnEnemy() // 리스폰
    {
        nodes.allNodes[nodes.minIndex].connections.Remove(nodes.thisNode);

        Vector2 spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        while (Vector2.Distance(spawn, player.position) < respawnPointMinDistance)
            spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        transform.position = spawn;

        Path path = graph.GetShortestPath(nodes.thisNode, nodes.thisNode);
        follower.Follow(path);

        isRespawned = true;
        isPlayerFounded = true;
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
        bool playerInSight = !Physics2D.Raycast(transform.position, (transform.position - player.position).normalized, layerMask);
        bool playerInRecognitionRange = Vector2.Distance(transform.position, player.transform.position) < recognitionRange;

        if (playerInSight && playerInRecognitionRange)
        {
            graph.GetShortestPath(nodes.thisNode, nodes.thisNode);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed);
        }
    }

    IEnumerator DelayPathFinding()
    {
        yield return new WaitForSeconds(2);
        RePathFinding();
        isRespawned = false;
    }
}
