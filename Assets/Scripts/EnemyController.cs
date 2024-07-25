using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector2 wanderDirChangeIntervalMinMax;

    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        AI();
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(moveVector.x, moveVector.y, 0) * 0.02f;
    }

    void AI()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            return;
        }

        // Follow the player:
        if (player != null)
        {
            moveVector = (player.position - transform.position).normalized;
            return;
        }
        
        // Get random dir:
        moveVector = Random.insideUnitCircle.normalized;
        t = Random.Range(wanderDirChangeIntervalMinMax.x, wanderDirChangeIntervalMinMax.y);
    }

    public void GotHit()
    {
        GameManager.Instance.EXPSpawner.SpawnEXP(transform.position, EXPTiers.small);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.collider.gameObject);
        }
    }
}
