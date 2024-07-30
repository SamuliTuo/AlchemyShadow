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

    Coroutine currentAction = null;
    Coroutine currentMoveAction = null;

    public void InitBoss()
    {
        graphics = GetComponentInChildren<SpriteRenderer>();
        normalMat = graphics.material;
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player;
        hp = maxHp;
        hitFlashMat = GameManager.Instance.hitFlashMaterial;
        weapons = GetComponent<BossWeapons>();
        weapons.Init();
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
        if (currentAction != null)
        {
            ChooseAction();
        }
        if (currentMoveAction != null)
        {
            ChooseMoveAction();
        }
    }

    
    void ChooseAction()
    {
        switch (Random.Range(0, 3))
        {
            case 0: currentAction = StartCoroutine(weapons.ShootRings(5, 10, 0.3f, 5, 20)); break;
            case 1: currentAction = StartCoroutine(weapons.ShootAtRandomDirections(5, 0.1f, 1, 10, 2)); break;
            case 2: currentAction = StartCoroutine(weapons.ShootAtPlayer(1, 3, 20, 5)); break;
            default: break;
        }
    }

    void ChooseMoveAction()
    {
        switch (Random.Range(0,3))
        {
            case 0: currentMoveAction = StartCoroutine(FollowPlayer(10, 10)); break;
            case 1: currentMoveAction = StartCoroutine(Wander(10, 30, 1.5f)); break;
            case 2: currentMoveAction = StartCoroutine(Stop(3)); break;
        }
    }



    // ACTIONS
    IEnumerator ShootPlayer(int shotcount, float shotInterval)
    {
        for (int i = 0; i < shotcount; i++)
        {
            // shoot
            yield return new WaitForSeconds(shotInterval);
        }
    }


    // MOVE ACTIONS
    IEnumerator FollowPlayer(float speed, float duration)
    {
        float t = 0;
        currentMoveSpeed = speed;
        while (t < duration)
        {
            if (player != null)
            {
                moveVector = (player.position - transform.position).normalized;
            }
            t += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Wander(int dashes, float speed, float changeDirectionInterval)
    {
        for (int i = 0; i < dashes; i++)
        {
            moveVector = Random.insideUnitCircle.normalized;
            currentMoveSpeed = speed;
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }
    IEnumerator Stop(float duration)
    {
        moveVector = Vector3.zero;
        yield return new WaitForSeconds(duration);
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
            StopAllCoroutines();
            StartCoroutine(HitFlash());
        }
    }

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
