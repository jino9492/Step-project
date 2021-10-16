using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Flashlight flash;
    public GameObject enemyPrefab;
    public Vector2[] spawnPoints;

    public Transform player;
    private NavMeshAgent enemy;
    

    private void Start()
    {
        flash = GameObject.Find("Flashlight").GetComponent<Flashlight>();
        player = GameObject.Find("Player").GetComponent<Transform>();

        enemy = GetComponent<NavMeshAgent>();
        enemy.updateRotation = false;
        enemy.updateUpAxis = false;
    }

    
    private void Update()
    {
        if (flash.isEnemyDead) // 리스폰
        {
            RespawnEnemy();
            flash.isEnemyDead = false;
        }

        if(enemy.isOnNavMesh)
            enemy.SetDestination(player.position);
    }


    public void RespawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
        Destroy(this.gameObject);
    }
}
