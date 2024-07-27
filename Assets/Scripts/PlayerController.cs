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
    public Image expBar, hpBar;
    public TextMeshProUGUI lvlNumber;
    public float startWalkingSpeed = 5;

    float maxHp;
    PlayerWeapons weapon;
    SpriteRenderer graphics;
    Vector2 inputVector = Vector2.zero;
    private int lvl = 1;
    private float exp = 0;
    private float expRequiredForLvlUp = 5;
    private Rigidbody2D rb;
    private Animator anim;


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

    // Gather exp
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Loot"))
        {
            exp++;
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);

            if (exp >= expRequiredForLvlUp)
            {
                exp -= expRequiredForLvlUp;
                expRequiredForLvlUp *= 1.6f;
                lvl++;
                lvlNumber.text = lvl.ToString();
                print("give a powerup");
            }

            expBar.fillAmount = exp / expRequiredForLvlUp;
        }
    }
}
