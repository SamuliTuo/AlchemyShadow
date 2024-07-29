using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float hp = 10;
    public float moveSpeed = 0.1f;
    public float acceleration = 10f;
    public float hpRegenInterval = 1;
    public float hpRegenPercTic = 0.3f;
    public Image expBar, hpBar;
    public TextMeshProUGUI lvlNumber;
    public float startWalkingSpeed = 5;
    public List<UpgradeSlot> slots = new List<UpgradeSlot>();

    float maxHp;
    PlayerWeapons weapon;
    SpriteRenderer graphics;
    Vector2 inputVector = Vector2.zero;
    private int lvl = 1;
    private float exp = 0;
    private float expRequiredForLvlUp = 5;
    private Rigidbody2D rb;
    private Animator anim;
    private float hpRegenTimer = 0;


    void Awake()
    {
        //weapons = GetComponent<PlayerWeapons>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponent<PlayerWeapons>();
        weapon.StartShooting();
        graphics = GetComponentInChildren<SpriteRenderer>();
        maxHp = hp;
    }

    void Update()
    {
        GatherInput();

        // hp regen
        hpRegenTimer += Time.deltaTime;
        if (hpRegenTimer > hpRegenInterval)
        {
            HealHP(hpRegenPercTic);
            hpRegenTimer = 0;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.MoveTowards(rb.velocity, inputVector * moveSpeed, acceleration);

        // walk anim
        if (rb.velocity.sqrMagnitude > startWalkingSpeed)
        {
            if (rb.velocity.x > 0.1f)
            {
                graphics.flipX = true;
            }
            else if (rb.velocity.x < -0.1f)
            {
                graphics.flipX = false;
            }
            anim.Play("walk");
        }
        // idle anim
        else
        {
            anim.Play("idle");
        }
    }


    void GatherInput()
    {
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.A))
        {
            x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y -= 1;
        }
        inputVector = new Vector2(x, y).normalized;
    }

    public float invulnTimer = 0.3f;
    bool invuln = false;
    IEnumerator Invuln()
    {
        invuln = true;
        yield return new WaitForSeconds(invulnTimer);
        invuln = false;
    }

    public void GotHit()
    {
        if (invuln)
        {
            return;
        }
        hp--;
        hpBar.fillAmount = Mathf.Min(hp / maxHp, 1);
        if (hp <= 0)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Invuln());
    }

    public void AddMaxHp(float addHp)
    {
        maxHp += addHp;
        hp += addHp;
        hpBar.fillAmount = Mathf.Min(hp / maxHp, 1);
    }
    public void HealHP(float amount)
    {
        hp += amount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hpBar.fillAmount = hp / maxHp;
    }

    // Gather exp
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loot"))
        {
            exp += expRate;
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);

            // LEVEL UP ! :D
            if (exp >= expRequiredForLvlUp)
            {
                exp -= expRequiredForLvlUp;
                expRequiredForLvlUp *= 1.6f;
                lvl++;
                lvlNumber.text = lvl.ToString();

                var options = GetRandomOptions();
                PresentOptions(options);

                GameManager.Instance.PausedTheGame();
                Time.timeScale = 0;
            }

            //update bar
            expBar.fillAmount = exp / expRequiredForLvlUp;
        }
    }



    public List<PowerUp> powerUps = new List<PowerUp>();
    List<PowerUp> GetRandomOptions()
    {
        int[] ints = GameManager.Instance.GenerateRandomUniqueIntegers(new(3, 3), new(0, powerUps.Count));

        return new List<PowerUp>
        {
            powerUps[ints[0]],
            powerUps[ints[1]],
            powerUps[ints[2]]
        };
    }

    public void PresentOptions(List<PowerUp> l)
    {
        slots[0].gameObject.SetActive(true);
        slots[1].gameObject.SetActive(true);
        slots[2].gameObject.SetActive(true);
        slots[0].InitOption(l[0]);
        slots[1].InitOption(l[1]);
        slots[2].InitOption(l[2]);
    }

    public void ChooseOption(PowerUp upgrade)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        ApplyEffects(upgrade);

        GameManager.Instance.UnpausedTheGame();
        Time.timeScale = 1;
    }

    void ApplyEffects(PowerUp p)
    {
        if (p.damage > 0)
        {
            weapon.AddDamage(p.damage);
        }
        if (p.attackSpeed > 0)
        {
            weapon.AddAttackSpeed(p.attackSpeed);
        }
        if (p.moveSpeed > 0)
        {
            moveSpeed += p.moveSpeed;
        }
        if (p.lootRange > 0)
        {
            lootTrigger.radius += p.lootRange;
        }
        if (p.regen > 0)
        {
            hpRegenPercTic += p.regen;
        }
        if (p.hp > 0)
        {
            AddMaxHp(p.hp);
        }
        if (p.expRate > 0)
        {
            expRate += p.expRate;
        }
        if (p.flagArea > 0)
        {
            flagArea.localScale = new(flagArea.localScale.x + p.flagArea, flagArea.localScale.y + p.flagArea, 1);
        }
    }

    private float expRate = 1;
    public CircleCollider2D lootTrigger;
    public Transform flagArea;

}

// DAMAGE, ATT_SPD, MOVE_SPD, LOOT_RANGE, REGEN, HP, EXP, FLAG_AREA
[System.Serializable]
public class PowerUp
{
    public string name;
    public string description;

    [Header("Example:  0.15 = 15% added to according stat.")]
    public float damage = 0;
    public float attackSpeed = 0;
    public float moveSpeed = 0;
    public float lootRange = 0;
    public float regen = 0;
    public float hp = 0;
    public float expRate = 0;
    public float flagArea = 0;

    public PowerUp(
        string name, 
        string description, 
        float damage, 
        float attackSpeed, 
        float moveSpeed, 
        float lootRange, 
        float regen, 
        float hp, 
        float expRage, 
        float flagArea)
    {
        this.name = name;
        this.description = description;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
        this.lootRange = lootRange;
        this.regen = regen;
        this.hp = hp;
        this.expRate = expRage;
        this.flagArea = flagArea;
    }
}
