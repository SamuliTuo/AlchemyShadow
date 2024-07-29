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
public enum SlaveTier { S, SS, SSS, }

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
    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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
        var sqrMag = moveVector.magnitude;
        if (sqrMag < stopRange)
        {
            moveVector = Vector2.zero;
        }
        else
        {
            actualMoveSpd = Mathf.Lerp(0, randomizedMoveSpeed * 5, sqrMag * 0.01f);
            if (anim != null)
            {
                anim.speed = actualMoveSpd * 0.4f;
            }
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
        PlayCorrectDeathSound();
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
            else if (collision.collider.CompareTag("Friend"))
            {
                var controller = collision.collider.GetComponent<SlaveController>();
                if (controller != null)
                {
                    if (controller.isFree)
                    {
                        SetFree();
                    }
                }
            }
        }
    }

    public void SetFree()
    {
        GameManager.Instance.FriendSpawner.FriendWasFreed(gameObject);
        isFree = true;
        helpSign.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Friend");
        GetComponent<PlayerWeapons>().StartShooting();
        StartCoroutine(GotFreedTween());

        if (GameManager.Instance.PartyManager.AddFriendAndCheckIfCombines(gameObject))
        {
            return;
        }

        PlayCorrectSoundAndParticles();
    }


    void PlayCorrectSoundAndParticles()
    {
        switch (slaveType)
        {
            case SlaveTypes.slave0_S:
                GameManager.Instance.AudioManager.PlayClip("ally1_rescue_S");
                GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave0_SS:
                GameManager.Instance.AudioManager.PlayClip("ally1_rescue_SS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave0_SSS:
                GameManager.Instance.AudioManager.PlayClip("ally1_rescue_SSS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSSS", transform.position, transform.forward, true);
                break;


            case SlaveTypes.slave1_S:
                GameManager.Instance.AudioManager.PlayClip("ally2_rescue_S");
                GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave1_SS:
                GameManager.Instance.AudioManager.PlayClip("ally2_rescue_SS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave1_SSS:
                GameManager.Instance.AudioManager.PlayClip("ally2_rescue_SSS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSSS", transform.position, transform.forward, true);
                break;


            case SlaveTypes.slave2_S:
                GameManager.Instance.AudioManager.PlayClip("ally0_rescue_S");
                GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave2_SS:
                GameManager.Instance.AudioManager.PlayClip("ally0_rescue_SS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave2_SSS:
                GameManager.Instance.AudioManager.PlayClip("ally0_rescue_SSS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSSS", transform.position, transform.forward, true);
                break;


            case SlaveTypes.slave3_S:
                GameManager.Instance.AudioManager.PlayClip("ally3_rescue_S");
                GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave3_SS:
                GameManager.Instance.AudioManager.PlayClip("ally3_rescue_SS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave3_SSS:
                GameManager.Instance.AudioManager.PlayClip("ally3_rescue_SSS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSSS", transform.position, transform.forward, true);
                break;


            case SlaveTypes.slave4_S:
                GameManager.Instance.AudioManager.PlayClip("ally4_rescue_S");
                GameManager.Instance.ParticleEffects.PlayParticles("friendFreed", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave4_SS:
                GameManager.Instance.AudioManager.PlayClip("ally4_rescue_SS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.slave4_SSS:
                GameManager.Instance.AudioManager.PlayClip("ally4_rescue_SSS");
                GameManager.Instance.ParticleEffects.PlayParticles("combineToSSS", transform.position, transform.forward, true);
                break;
            case SlaveTypes.NULL:
                break;
            default:
                break;
        }
    }

    void PlayCorrectDeathSound()
    {
        switch (slaveType)
        {
            case SlaveTypes.slave0_S: GameManager.Instance.AudioManager.PlayClip("ally1_die_S"); break;
            case SlaveTypes.slave0_SS: GameManager.Instance.AudioManager.PlayClip("ally1_die_SS"); break;
            case SlaveTypes.slave0_SSS: GameManager.Instance.AudioManager.PlayClip("ally1_die_SSS"); break;
            case SlaveTypes.slave1_S: GameManager.Instance.AudioManager.PlayClip("ally2_die_S"); break;
            case SlaveTypes.slave1_SS: GameManager.Instance.AudioManager.PlayClip("ally2_die_SS"); break;
            case SlaveTypes.slave1_SSS: GameManager.Instance.AudioManager.PlayClip("ally2_die_SSS"); break;
            case SlaveTypes.slave2_S: GameManager.Instance.AudioManager.PlayClip("ally0_die_S"); break;
            case SlaveTypes.slave2_SS: GameManager.Instance.AudioManager.PlayClip("ally0_die_SS"); break;
            case SlaveTypes.slave2_SSS: GameManager.Instance.AudioManager.PlayClip("ally0_die_SSS"); break;
            case SlaveTypes.slave3_S: GameManager.Instance.AudioManager.PlayClip("ally3_die_S"); break;
            case SlaveTypes.slave3_SS: GameManager.Instance.AudioManager.PlayClip("ally3_die_SS"); break;
            case SlaveTypes.slave3_SSS: GameManager.Instance.AudioManager.PlayClip("ally3_die_SSS"); break;
            case SlaveTypes.slave4_S: GameManager.Instance.AudioManager.PlayClip("ally4_die_S"); break;
            case SlaveTypes.slave4_SS: GameManager.Instance.AudioManager.PlayClip("ally4_die_SS"); break;
            case SlaveTypes.slave4_SSS: GameManager.Instance.AudioManager.PlayClip("ally4_die_SSS"); break;

            default: break;
        }
    }


    [Header("FreeTween")]
    public float maxSizeWhenSaved = 2.5f;
    public float freedTweenSpeed1 = 1;
    public float freedTweenSpeed2 = 1;
    float originalScale;
    IEnumerator GotFreedTween()
    {
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
            t += Time.deltaTime * freedTweenSpeed1;
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
            t += Time.deltaTime * freedTweenSpeed2;
            yield return null;
        }
        if (graphics != null)
        {
            graphics.localScale = Vector3.one * originalScale;
        }
    }

    [Header("Shoot tween")]
    public float shootTweenGrowDuration = 1;
    public float shootTweenShrinkDuration = 1;
    public float shootTweenMaxSizeHeight = 1.5f;
    public float shootTweenMaxSizeWideness = 1.5f;
    public IEnumerator ShootTween()
    {
        float t = 0;
        while (t < shootTweenGrowDuration)
        {
            float perc = t / shootTweenGrowDuration;
            perc = Mathf.Lerp(0, 1, CorouTweens.easeInOut.Evaluate(perc));
            if (graphics != null)
            {
                float x = Mathf.Lerp(originalScale, originalScale * shootTweenMaxSizeHeight, perc);
                float y = Mathf.Lerp(originalScale, originalScale * shootTweenMaxSizeWideness, perc);
                graphics.localScale = new Vector3(x, y, 1);
            }
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < shootTweenShrinkDuration)
        {
            float perc = t / shootTweenShrinkDuration;
            perc = Mathf.Lerp(0, 1, CorouTweens.easeInOut.Evaluate(perc));
            if (graphics != null)
            {
                float x = Mathf.Lerp(originalScale * shootTweenMaxSizeHeight, originalScale, perc);
                float y = Mathf.Lerp(originalScale * shootTweenMaxSizeWideness, originalScale, perc);
                graphics.localScale = new Vector3(x, y, 1);
            }
            t += Time.deltaTime;
            yield return null;
        }
        graphics.localScale = Vector3.one * originalScale;
    }
}
