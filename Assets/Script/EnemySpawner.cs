using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyToSpawn
{
    public GameObject enemyToSpawn;
    public float spawnDelay;
}

public class EnemySpawner : MonoBehaviour
{

    private float spawnTimeCount;

    public List<EnemyToSpawn> enemyList;

    public float spawnDistance;

    private GameObject player;

    // Update is called once per frame

    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }
    void Update()
    {
        spawnTimeCount -= Time.deltaTime;

        if (spawnTimeCount <= 0)
        {
            SpawnEnemy();
        }    
    }

    void SpawnEnemy()
    {
        EnemyToSpawn nmeToSpawn = enemyList[Random.Range(0, enemyList.Count)];

        GameObject newEnemy = nmeToSpawn.enemyToSpawn;

        float spawnHeight = newEnemy.GetComponent<Enemy>().isWinged ? Random.Range(10f, 14f): 9f ;

        Instantiate(newEnemy, new Vector3((Random.Range(0, 2) * 2 - 1) * spawnDistance + player.transform.position.x, spawnHeight, 0), Quaternion.identity);

        spawnTimeCount = nmeToSpawn.spawnDelay;
    }
}
