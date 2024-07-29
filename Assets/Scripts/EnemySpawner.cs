using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public float enemySpawnRate = 0.5f;


    //private void Start()
    //{
    //    StartCoroutine(Spawner());
    //}
    
    //IEnumerator Spawner()
    //{
    //    float t = enemySpawnRate;
    //    var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
    //    pos.z = 0;

    //    var enemy = enemies[Random.Range(0, enemies.Count)];
    //    Instantiate(enemy, pos, Quaternion.identity);
    //    while (t > 0)
    //    {
    //        t -= Time.deltaTime;
    //        yield return null;
    //    }
    //    StartCoroutine(Spawner());
    //}
}