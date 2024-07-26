using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaveController : MonoBehaviour
{
    public Vector2 wanderDirChangeIntervalMinMax;
    public float moveSpeed = 0.11f;
    public float stopRange = 1;
    public GameObject helpSign;


    bool isFree = false;
    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;
    

    private void Start()
    {
        player = GameObject.Find("Player")?.transform;
    }

    void Update()
    {
        if (!isFree)
        {
            return;
        }

        AI();
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed;
    }

    void AI()
    {
        // Thinking
        if (t > 0)
        {
            t -= Time.deltaTime;
            return;
        }

        // Follow the player:
        if (player != null)
        {
            moveVector = player.position - transform.position;
            if (moveVector.sqrMagnitude < stopRange)
            {
                moveVector = Vector2.zero;
            }
            moveVector = moveVector.normalized;
            return;
        }

        // Get random dir:
        moveVector = Random.insideUnitCircle.normalized;
        t = Random.Range(wanderDirChangeIntervalMinMax.x, wanderDirChangeIntervalMinMax.y);
    }

    public void GotHit()
    {
        if (!isFree)
        {
            return;
        }
        GameManager.Instance.EXPSpawner.SpawnEXP(transform.position, EXPTiers.small);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFree)
        {
            if (collision.collider.CompareTag("Player"))
            {
                helpSign.SetActive(false);
                isFree = true;
            }
        }
    }
}
