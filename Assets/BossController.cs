using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float acceleration = 2;
    public Coroutine currentAction = null;
    public Coroutine currentMoveAction = null;

    public GameObject innerRing;
    public GameObject outerRing;

    [Header("phase 1")]
    public float p1_moveSpeed = 10;

    float currentMoveSpeed;
    Vector2 moveVector = Vector2.zero;
    Transform player;
    Rigidbody2D rb;
    private SpriteRenderer graphics;
    Material normalMat;
    Material hitFlashMat;
    BossWeapons weapons;
    private GameObject hpBarObj;
    private Image hpBar;


    public void InitBoss()
    {
        graphics = GetComponentInChildren<SpriteRenderer>();
        normalMat = graphics.material;
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player;
        hpBarObj = GameManager.Instance.bossHpBarObj;
        hpBarObj.SetActive(true);
        hpBar = GameManager.Instance.bossHpBar;
        hp = maxHp;
        UpdateHPBar();

        hitFlashMat = GameManager.Instance.hitFlashMaterial;
        weapons = GetComponent<BossWeapons>();

        weapons.Init(this);
        GameManager.Instance.cam.GetComponentInChildren<EdgeIndicators>().SetBoss(this.transform);
    }


    void UpdateHPBar()
    {
        if (hpBar == null)
        {
            return;
        }
        hpBar.fillAmount = hp / maxHp;
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
            ChooseAction();
        }
        if (currentMoveAction == null)
        {
            Time.timeScale = 1;
            ChooseMoveAction();

        }
    }


    void ChooseAction()
    {
        int rand;
        if (specialIsOnCooldown)
        {
            rand = Random.Range(0, 4);
        }
        else
        {
            rand = Random.Range(0, 5);
        }

        switch (rand)
        {
            case 0:
                if (Random.Range(0, 2) == 0)
                {
                    currentAction = StartCoroutine(weapons.ShootRings(300, 2, 0.09f, 3.5f, 7));
                }
                else
                {
                    currentAction = StartCoroutine(weapons.ShootRings(400, 4, 0.03f, 1, 10));
                }
                break;

            case 1: 
                currentAction = StartCoroutine(weapons.ShootAtRandomDirections(5, 0.1f, 2.5f, 4, 2.2f)); 
                break;

            case 2: 
                currentAction = StartCoroutine(weapons.ShootAtPlayer(1, 3, 10, 5)); 
                break;

            case 3:
                var spawns = GetRandomEnemiesToSpawn();
                currentAction = StartCoroutine(SpawnEnemies(spawns.enemy, spawns.interval, spawns.summonCount));
                break;

            case 4:
                if (currentMoveAction != null)
                {
                    StopCoroutine(currentMoveAction);
                }
                moveVector = Vector3.zero;
                currentMoveAction = currentAction = StartCoroutine(SpecialAttack());
                break;

            default: break;
        }
    }

    void ChooseMoveAction()
    {
        switch (Random.Range(0, 3))
        {
            case 0: currentMoveAction = StartCoroutine(FollowPlayer(2.5f, Random.Range(3, 6))); break;
            case 1: currentMoveAction = StartCoroutine(Wander(Random.Range(3, 7), 7, 0.2f)); break;
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

    RandomEnemyPacket GetRandomEnemiesToSpawn()
    {
        switch (Random.Range(0, 7))
        {
            case 0: return new RandomEnemyPacket(GameManager.Instance.birdMachine, 2, 2);
            case 1: return new RandomEnemyPacket(GameManager.Instance.blurbo, 0.2f, 9);
            case 2: return new RandomEnemyPacket(GameManager.Instance.demon, 2, 2);
            case 3: return new RandomEnemyPacket(GameManager.Instance.pyramid, 0.1f, 12);
            case 4: return new RandomEnemyPacket(GameManager.Instance.skull, 1, 10);
            case 5: return new RandomEnemyPacket(GameManager.Instance.snek, 0.1f, 5);
            case 6: return new RandomEnemyPacket(GameManager.Instance.spider, 1, 3);
            default: return new RandomEnemyPacket(GameManager.Instance.snek, 0.1f, 5);
        }
    }
    class RandomEnemyPacket
    {
        public GameObject enemy;
        public float interval;
        public int summonCount;
        public RandomEnemyPacket(GameObject enemy, float interval, int summonCount)
        {
            this.enemy = enemy;
            this.interval = interval;
            this.summonCount = summonCount;
        }
    }

    public IEnumerator SpawnEnemies(GameObject enemy, float interval, int summonAmount)
    {
        float t = 0;
        int summons = summonAmount;
        GameManager.Instance.FriendSpawner.SpawnAFriend();
        while (summons > 0)
        {
            while (t < interval)
            {
                t += Time.deltaTime;
                yield return null;
            }
            var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
            pos.z = 0;
            Instantiate(enemy, pos, Quaternion.identity);
            t = 0;
            summons--;
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
            if (player != null)
            {
                moveVector = (player.position - transform.position).normalized;
            }
            t += Time.deltaTime;
            yield return null;
        }
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
        currentMoveAction = null;
    }
    IEnumerator Stop(float duration)
    {
        moveVector = Vector3.zero;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        currentMoveAction = null;
    }








    // SPecial attack
    bool specialIsOnCooldown = false;
    Vector3 originalScale;
    public float specialChargeDuration = 5;
    public float specialCooldownDuration = 10;
    public float specialDamage;
    IEnumerator SpecialAttack()
    {
        originalScale = innerRing.transform.localScale;
        innerRing.transform.localScale = Vector3.one * 0.001f;
        innerRing.gameObject.SetActive(true);
        outerRing.gameObject.SetActive(true);
        // turn on rings

        float t = 0.1f;
        while (t < specialChargeDuration)
        {
            moveVector = Vector3.zero;
            float perc = t / specialChargeDuration;
            innerRing.transform.localScale = Vector3.Lerp(Vector3.one * 0.001f, originalScale, perc);
            t += Time.deltaTime;
            yield return null;
        }
        outerRing.gameObject.SetActive(false);
        innerRing.gameObject.SetActive(false);

        // did we hit something?
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(innerRing.transform.position, 16.1f);

        foreach (Collider2D col in overlaps)
        {
            if (col.CompareTag("Player") || col.CompareTag("Friend"))
            {
                col.SendMessage("GotHit", specialDamage);
            }
        }

        specialIsOnCooldown = true;
        StartCoroutine(SpecialCooldown());
        currentAction = currentMoveAction = null;
    }

    IEnumerator SpecialCooldown()
    {
        float t = 0;
        while (t < specialCooldownDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        specialIsOnCooldown = false;
    }












    public void GotHit(float damage)
    {
        hp -= damage;
        UpdateHPBar();
        if (hp <= 0)
        {
            hpBarObj.SetActive(false);
            GameManager.Instance.GameLoop.BossDied();
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
    SpriteRenderer gunarm;
    IEnumerator HitFlash()
    {
        if (gunarm == null)
        {
            gunarm = weapons.gunArm.GetComponentInChildren<SpriteRenderer>();
        }
        graphics.material = hitFlashMat;
        gunarm.material = hitFlashMat;
        yield return new WaitForSeconds(GameManager.Instance.enemyHitFlashTime);
        gunarm.material = normalMat;
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
