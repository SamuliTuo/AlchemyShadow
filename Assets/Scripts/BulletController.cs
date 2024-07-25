using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 dir;
    float speed;
    float lifeTime;

    public void Init(Vector3 dir, float speed, float lifeTime)
    {
        this.dir = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        StartCoroutine(BulletCoroutine());
    }

    IEnumerator BulletCoroutine()
    {
        float t = 0;
        while (t < lifeTime)
        {
            transform.position += dir * speed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().GotHit();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
