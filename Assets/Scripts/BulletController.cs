using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTypes
{
    BASIC, SINE_WAVE,
}

public class BulletController : MonoBehaviour
{
    public float amplitude = 10f;
    public float period = 5f;

    float damage;
    Vector3 dir;
    float speed;
    float lifeTime;
    BulletTypes type;
    bool goesThrough;
    bool negativeWave;
    Vector3 startPos;
    bool isEnemyProjectile = false;
    int penetrations = 0;

    public void Init(float damage, Vector3 dir, float speed, float lifeTime, int penetrations, BulletTypes type = BulletTypes.BASIC, bool goesThrough = false, bool negativeWave = false, bool thisIsEnemyProjectile = false)
    {
        this.damage = damage;
        this.dir = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.penetrations = penetrations;
        this.type = type;
        this.goesThrough = goesThrough;
        this.negativeWave = negativeWave;
        this.isEnemyProjectile = thisIsEnemyProjectile;
        StartCoroutine(BulletCoroutine());
    }

    IEnumerator BulletCoroutine()
    {
        float t = 0;
        float waveT = 0;
        if (type == BulletTypes.SINE_WAVE)
        {
            startPos = transform.GetChild(0).localPosition;
        }
        
        while (t < lifeTime)
        {
            transform.position += dir * speed * Time.deltaTime;

            if (type == BulletTypes.SINE_WAVE)
            {
                if (negativeWave)
                {
                    waveT -= Time.deltaTime / period;
                }
                else
                {
                    waveT += Time.deltaTime / period;
                }
                float distance = amplitude * Mathf.Sin(waveT);
                transform.GetChild(0).localPosition = startPos + Vector3.up * distance;
            }

            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            GameManager.Instance.ParticleEffects.PlayParticles("shotHit", transform.position, transform.forward);
            Destroy(gameObject);
            return;
        }

        // enemy projectiles
        if (isEnemyProjectile)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Friend"))
            {
                collision.gameObject.SendMessage("GotHit");
                if (goesThrough)
                {
                    return;
                }
                else if (penetrations >= 0)
                {
                    penetrations--;
                    return;
                }
                Destroy(gameObject);
            }
            return;
        }

        // player team projectiles
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().GotHit(damage);
            GameManager.Instance.ParticleEffects.PlayParticles("shotHit", transform.position, transform.forward);
            if (goesThrough)
            {
                return;
            }
            else if (penetrations >= 0)
            {
                penetrations--;
                return;
            }
            Destroy(gameObject);
        }
    }
}
