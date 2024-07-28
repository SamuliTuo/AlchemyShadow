using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Vector2 wanderDirChangeIntervalMinMax;
    public float moveSpeed = 1;
    public float acceleration = 1;

    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;
    Rigidbody2D rb;
    private SpriteRenderer graphics;

    private void Awake()
    {
        graphics = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        player = GameObject.Find("Player")?.transform;
    }

    void Update()
    {
        AI();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.MoveTowards(rb.velocity, moveVector * moveSpeed, acceleration);

        //flip sprite
        if (rb.velocity.x > 0.1f)
        {
            graphics.flipX = true;
        }
        else if (rb.velocity.x < -0.1f)
        {
            graphics.flipX = false;
        }
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
        GameManager.Instance.ParticleEffects.PlayParticles("enemyDeath", transform.position, transform.forward);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Friend"))
        {
            collision.gameObject.SendMessage("GotHit");
        }
    }
}
