using System.Collections;
using UnityEngine;

public enum SlaveTypes
{
    slave0_S, slave0_SS, slave0_SSS,
    slave1_S, slave1_SS, slave1_SSS,
    slave2_S, slave2_SS, slave2_SSS,
    slave3_S, slave3_SS, slave3_SSS,
    slave4_S, slave4_SS, slave4_SSS,

    NULL,
}

public class SlaveController : MonoBehaviour
{
    public Vector2 wanderDirChangeIntervalMinMax;
    public float moveSpeed = 0.11f;
    public float moveSpeedRandomizerMaxAndMinPerc = 0.2f;
    public Vector2 moveSpeedRanomizerTimerMinMax = new Vector2(0.5f, 2f);
    public float acceleration = 10f;
    public float stopRange = 1;
    public GameObject helpSign;
    public SlaveTypes slaveType;

    bool isFree = false;
    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;
    Transform graphics;
    SpriteRenderer spriteGraphic;
    Rigidbody2D rb;
    float actualMoveSpd;
    

    private void Start()
    {
        player = GameObject.Find("Player")?.transform;
        graphics = transform.GetChild(0);
        spriteGraphic = transform.GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        randomizedMoveSpeed = moveSpeed + Random.Range(-moveSpeed * moveSpeedRandomizerMaxAndMinPerc, moveSpeed * moveSpeedRandomizerMaxAndMinPerc);
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
        rb.velocity = Vector3.MoveTowards(rb.velocity, moveVector * actualMoveSpd, acceleration);

        if (rb.velocity.x > 0.1f)
        {
            spriteGraphic.flipX = true;
        }
        else if (rb.velocity.x < -0.1f)
        {
            spriteGraphic.flipX = false;
        }
    }

    Vector2 randomVector = Vector2.zero;
    public float randomVectorStrength = 1;
    float randomizedMoveSpeed;
    void AI()
    {
        // Thinking
        if (t > 0)
        {
            t -= Time.deltaTime;
        }
        else
        {
            randomVector = Random.insideUnitCircle.normalized;
            randomizedMoveSpeed = moveSpeed + Random.Range(-moveSpeed * moveSpeedRandomizerMaxAndMinPerc, moveSpeed * moveSpeedRandomizerMaxAndMinPerc);
            t = Random.Range(moveSpeedRanomizerTimerMinMax.x, moveSpeedRanomizerTimerMinMax.y);
        }

        // Follow the target if player is holding mouse:
        if (GameManager.Instance.PartyManager.followTheObject)
        {
            MoveTowards(GameManager.Instance.PartyManager.partyFollowObject);
            return;
        }
        // otherwise follow player:
        else if (player != null)
        {
            MoveTowards(player);
            return;
        }

        // Get random dir if no player / orders:
        moveVector = Random.insideUnitCircle.normalized;
        t = Random.Range(wanderDirChangeIntervalMinMax.x, wanderDirChangeIntervalMinMax.y);
    }

    void MoveTowards(Transform target)
    {
        moveVector = target.position - transform.position;
        moveVector += randomVector * randomVectorStrength;
        var sqrMag = moveVector.sqrMagnitude;
        if (sqrMag < stopRange)
        {
            moveVector = Vector2.zero;
        }
        else
        {
            actualMoveSpd = Mathf.Lerp(0, randomizedMoveSpeed * 5, sqrMag * 0.01f);
        }
        moveVector = moveVector.normalized;
        return;
    }

    public void GotHit()
    {
        if (!isFree)
        {
            return;
        }
        GameManager.Instance.PartyManager.FriendDied(gameObject);
        GameManager.Instance.EXPSpawner.SpawnEXP(transform.position, EXPTiers.small);
        GameManager.Instance.AudioManager.PlayClip("ally0_die");
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
        StartCoroutine(GotFreedTween());
        GameManager.Instance.AudioManager.PlayClip("ally0_rescue");
        GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward);
        GameManager.Instance.PartyManager.AddFriend(gameObject);
    }

    public float maxSizeWhenSaved = 2.5f;
    IEnumerator GotFreedTween()
    {
        float originalScale;
        if (graphics != null)
        {
            originalScale = graphics.localScale.x;
        }
        else
        {
            graphics = transform.GetChild(0);
            originalScale = graphics.localScale.x;
            print("ERROR ERROR! " + this.name + " has no graphics!");
        }

        float t = 0;
        while (t < 1)
        {
            if (graphics != null)
            {
                graphics.localScale = Vector3.one * Mathf.Lerp(originalScale, originalScale * maxSizeWhenSaved, t);
            }
            else
            {
                print("vieläkin graphics puuttuu...?");
            }
            t += Time.deltaTime * 20;
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            float perc = Mathf.Lerp(0, 1, CorouTweens.bounceOut.Evaluate(t));
            if (graphics != null)
            {
                graphics.localScale = Vector3.one * Mathf.Lerp(originalScale * maxSizeWhenSaved, originalScale, perc);
            }
            t += Time.deltaTime * 1.8f;
            yield return null;
        }
        if (graphics != null)
        {
            graphics.localScale = Vector3.one * originalScale;
        }
    }
}
