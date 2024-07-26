using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy_01;
    public float enemySpawnRate = 0.5f;


    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        float t = enemySpawnRate;
        var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
        pos.z = 0;
        Instantiate(enemy_01, pos, Quaternion.identity);
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Spawner());
    }
}