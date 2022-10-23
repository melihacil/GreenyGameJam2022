using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Instantiate
    // Start is called before the first frame update

    [SerializeField] private bool didSpawnEnemy = false;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private GameObject enemyTrial;
    [SerializeField] private Transform spawnPos;


    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<EnemyController>().isInRoom)
            return;
    }

    public void SpawnEnemy()
    {
        Debug.Log("SpawningEnemy");
        didSpawnEnemy = true;
        Instantiate(enemyTrial, spawnPos.position, Quaternion.identity);
        Invoke(nameof(ResetTimer), enemySpawnTime);
    }





    void ResetTimer()
    {
        didSpawnEnemy = false;
    }
}
