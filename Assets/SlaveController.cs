using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlaveTypes
{
    slave0_S, slave0_SS, slave0_SSS,
    slave1_S, slave1_SS, slave1_SSS,
    slave2_S, slave2_SS, slave2_SSS,

    NULL,
}

public class SlaveController : MonoBehaviour
{
    public Vector2 wanderDirChangeIntervalMinMax;
    public float moveSpeed = 0.11f;
    public float acceleration = 10f;
    public float stopRange = 1;
    public GameObject helpSign;
    public SlaveTypes slaveType;


    bool isFree = false;
    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;
    Rigidbody2D rb;
    

    private void Start()
    {
        player = GameObject.Find("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
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
        rb.velocity = Vector3.MoveTowards(rb.velocity, moveVector * moveSpeed, acceleration);
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
                SetFree();
            }
        }
    }

    public void SetFree()
    {
        isFree = true;
        helpSign.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Friend");
        GetComponent<PlayerWeapons>().StartShooting();
        GameManager.Instance.PartyManager.FriendDied(gameObject);
        GameManager.Instance.PartyManager.AddFriend(gameObject);
    }
}
