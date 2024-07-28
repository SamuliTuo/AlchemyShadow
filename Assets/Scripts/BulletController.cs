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

    Vector3 dir;
    float speed;
    float lifeTime;
    BulletTypes type;
    bool goesThrough;
    bool negativeWave;
    Vector3 startPos;

    List<GameObject> enemiesHit = new List<GameObject>();

    public void Init(Vector3 dir, float speed, float lifeTime, BulletTypes type = BulletTypes.BASIC, bool goesThrough = false, bool negativeWave = false)
    {
        enemiesHit.Clear();
        this.dir = dir;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.type = type;
        this.goesThrough = goesThrough;
        this.negativeWave = negativeWave;
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
        if (collision.CompareTag("Enemy"))
        {
            if (enemiesHit.Contains(gameObject))
            {
                return;
            }
            enemiesHit.Add(gameObject);
            collision.GetComponent<EnemyController>().GotHit();
            GameManager.Instance.ParticleEffects.PlayParticles("shotHit", transform.position, transform.forward);
            if (goesThrough)
            {
                return;
            }
            Destroy(gameObject);
        }
    }
}
