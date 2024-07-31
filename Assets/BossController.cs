using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float acceleration = 2;

    [Header("phase 1")]
    public float p1_moveSpeed = 10;

    float currentMoveSpeed;
    Vector2 moveVector = Vector2.zero;
    float t = 0;
    Transform player;
    Rigidbody2D rb;
    private SpriteRenderer graphics;
    Material normalMat;
    Material hitFlashMat;
    BossWeapons weapons;

    public Coroutine currentAction = null;
    public Coroutine currentMoveAction = null;

    public void InitBoss()
    {
        graphics = GetComponentInChildren<SpriteRenderer>();
        normalMat = graphics.material;
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player;
        hp = maxHp;
        hitFlashMat = GameManager.Instance.hitFlashMaterial;
        weapons = GetComponent<BossWeapons>();
        
        weapons.Init(this);
        GameManager.Instance.cam.GetComponentInChildren<EdgeIndicators>().SetBoss(this.transform);
    }


    void FixedUpdate()
    {
        //spessu
    //    if (special != null)
    //    {
    //        if (special.doingSpecial)
    //        {
    //            rb.velocity = Vector2.zero;
    //            return;
    //        }
    //    }

        rb.velocity = Vector3.MoveTowards(rb.velocity, moveVector * currentMoveSpeed, acceleration);

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


    public void UpdatePhaseOne()
    {
        if (currentAction == null)
        {
            Time.timeScale = 1;
            print("currentaction is null, chhoosing.");
            ChooseAction();
        }
        if (currentMoveAction == null)
        {
            Time.timeScale = 1;
            print("current move act null");
            ChooseMoveAction();
            
        }
    }

    
    void ChooseAction()
    {
        switch (Random.Range(0, 3))
        {
            case 0: currentAction = StartCoroutine(weapons.ShootRings(300, 3, 0.07f, 2.5f, 20)); break;
            case 1: currentAction = StartCoroutine(weapons.ShootAtRandomDirections(5, 0.1f, 1, 2.5f, 2)); break;
            case 2: currentAction = StartCoroutine(weapons.ShootAtPlayer(1, 3, 5, 5)); break;
            default: break;
        }
    }

    void ChooseMoveAction()
    {
        switch (Random.Range(0,3))
        {
            case 0: currentMoveAction = StartCoroutine(FollowPlayer(2.5f, Random.Range(3,6))); break;
            case 1: currentMoveAction = StartCoroutine(Wander(Random.Range(3,7), 7, 0.2f)); break;
            case 2: currentMoveAction = StartCoroutine(Stop(Random.Range(0.5f, 2))); break;
        }
    }


    public IEnumerator WaitAfterShoot(float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        currentAction = null;
    }

    // MOVE ACTIONS
    IEnumerator FollowPlayer(float speed, float duration)
    {
        float t = 0;
        currentMoveSpeed = speed;
        while (t < duration)
        {
            print("follooowing");
            if (player != null)
            {
                moveVector = (player.position - transform.position).normalized;
            }
            t += Time.deltaTime;
            yield return null;
        }
        print("folow end");
        currentMoveAction = null;
    }
    IEnumerator Wander(int dashes, float speed, float changeDirectionInterval)
    {

        for (int i = 0; i < dashes; i++)
        {
            float t = 0;
            moveVector = Random.insideUnitCircle.normalized;
            currentMoveSpeed = speed;
            while (t < changeDirectionInterval)
            {
                t += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        print("wander ened");
        currentMoveAction = null;
    }
    IEnumerator Stop(float duration)
    {
        print("stoppin");
        moveVector = Vector3.zero;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        print("stop ended");
        currentMoveAction = null;
    }

//            if (t > 0)
//        {
//            t -= Time.deltaTime;
//            return;
//        }

//if (special != null && player != null)
//{
//    if (special.doingSpecial)
//    {
//        moveVector = Vector2.zero;
//        return;
//    }
//    if (special.ShouldWeDoSpecialNow(player))
//    {
//        special.InitSpecialAttack();
//    }
//}

//// Follow the player:
//if (player != null)
//{
//    moveVector = (player.position - transform.position).normalized;
//    return;
//}

//// Get random dir:
//moveVector = Random.insideUnitCircle.normalized;
//t = Random.Range(wanderDirChangeIntervalMinMax.x, wanderDirChangeIntervalMinMax.y);
































public void GotHit(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameManager.Instance.EXPSpawner.SpawnEXP(transform.position, EXPTiers.small);
            GameManager.Instance.ParticleEffects.PlayParticles("enemyDeath", transform.position, transform.forward);
            Destroy(gameObject);
        }
        else
        {
            if (hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
            }
            hitFlashCoroutine = StartCoroutine(HitFlash());
        }
    }

    Coroutine hitFlashCoroutine = null;
    IEnumerator HitFlash()
    {
        graphics.material = hitFlashMat;
        yield return new WaitForSeconds(GameManager.Instance.enemyHitFlashTime);
        graphics.material = normalMat;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Friend"))
        {
            collision.gameObject.SendMessage("GotHit", 1);
        }
    }
}
