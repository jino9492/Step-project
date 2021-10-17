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
    public bool isRespawned;

    #region PathFinding
    public Graph graph;
    public Node targetNode;
    public Follower follower;
    public GraphController gc;
    public NodesInfo nodes;
    #endregion

    private void Start()
    {
        flash = GameObject.Find("Flashlight").GetComponent<Flashlight>();
        player = GameObject.Find("Player").GetComponent<Transform>();

        for (int i = 0; i < nodes.allNodes.Length; i++)
            spawnPoints[i] = nodes.allNodes[i].transform.position;

        
    }

    
    private void Update()
    {

        gc.FindShortestNode(ref nodes.allNodes, ref nodes.thisNode, ref nodes.minIndex, ref nodes.isNodeChanged); // 가장 가까운 노드 찾아 설정

        if (nodes.isNodeChanged)
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
    }


    public void RespawnEnemy() // 리스폰
    {
        follower.StopMoving();
        nodes.allNodes[nodes.minIndex].connections.Remove(nodes.thisNode);
        GameObject newObj = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
        newObj.transform.parent = graph.gameObject.transform;
        newObj.GetComponent<Enemy>().isRespawned = true;

        Destroy(this.gameObject);
        flash.isEnemyDead = false;
    }

    public void RePathFinding() // AI : 길 찾기
    {
        Path path = graph.GetShortestPath(nodes.thisNode, targetNode);
        follower.Follow(path);
        nodes.isNodeChanged = false;
    }

    IEnumerator DelayPathFinding()
    {
        yield return new WaitForSeconds(2);
        RePathFinding();
        isRespawned = false;
    }
}
