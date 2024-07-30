using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

//NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
public class SalmarinMarjapommit : Weapon
{
    public int waveCount = 3;
    public int burstShotCount = 8;
    public float burstShotInterval = 0.2f;
    public float burstSpread = 45f;

    public SalmarinMarjapommit(
        float weaponCooldownSpeed,
        float shootInterval,
        float bulletSpeed,
        float bulletLifetime,
        GameObject bullet) : base(weaponCooldownSpeed, shootInterval, bulletSpeed, bulletLifetime, bullet)
    { }
    float damage;
    int penetrations;
    public override void Shoot(float damage, Transform barrelEnd = null, int extraBullets = 0, int penetrations = 0)
    {
        this.damage = damage;
        this.penetrations = penetrations;
        StartCoroutine(Burst(barrelEnd));
    }


    IEnumerator Burst(Transform barrelEnd)
    {
        for (int i = 0; i < burstShotCount; i++)
        {
            ShootABurstShot(barrelEnd);
            yield return new WaitForSeconds(burstShotInterval);
        }
    }

    void ShootABurstShot(Transform barrelEnd)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            for (int i = 0; i < waveCount; i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-burstSpread, burstSpread)) * dir;
                var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations);
            }
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            for (int i = 0; i < waveCount; i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-burstSpread, burstSpread)) * dir;
                var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations);
            }
            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            control.PlayCorrectShootSound();
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}