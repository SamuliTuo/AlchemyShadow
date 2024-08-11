using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public float pushForce;

    Vector2 armDir;
    public void SetArmDir(Vector2 armDir) { this.armDir = armDir; }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Rigidbody2D>().AddForce(armDir * pushForce, ForceMode2D.Impulse);
        }
    }
}
