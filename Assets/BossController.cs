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
            print("hp bar is null");
            return;
        }
        print("update hp");
        print("hp : " + hp + ", maxHp" + maxHp);
        print("fill amount: " + (hp / maxHp));
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
        print("choosing action");
        switch (Random.Range(0, 4))
        {
            case 0: currentAction = StartCoroutine(weapons.ShootRings(300, 2.5f, 0.09f, 3.5f, 20)); break;
            case 1: currentAction = StartCoroutine(weapons.ShootAtRandomDirections(5, 0.1f, 2.5f, 4, 2.2f)); break;
            case 2: currentAction = StartCoroutine(weapons.ShootAtPlayer(1, 5, 6, 5)); break;
            case 3:
                var spawns = GetRandomEnemiesToSpawn();
                currentAction = StartCoroutine(SpawnEnemies(spawns.enemy, spawns.interval, spawns.summonCount));
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
        print("spawned some " + enemy.name);
        currentAction = StartCoroutine(WaitAfterShoot(1));
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


    public void GotHit(float damage)
    {
        print("boss got hit");
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
