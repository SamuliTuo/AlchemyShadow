using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float acceleration = 10f;
    public Image expBar;
    public TextMeshProUGUI lvlNumber;

    PlayerWeapons weapon;
    Vector2 inputVector = Vector2.zero;
    private int lvl = 1;
    private float exp = 0;
    private float expRequiredForLvlUp = 5;
    private Rigidbody2D rb;


    void Awake()
    {
        //weapons = GetComponent<PlayerWeapons>();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<PlayerWeapons>();
        weapon.StartShooting();
    }

    void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.MoveTowards(rb.velocity, inputVector * moveSpeed, acceleration);
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

    public void GotHit()
    {
        Destroy(gameObject);
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
