using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
public class Shotgun_Wave : Weapon
{
    public int bulletCount = 8;
    public float shotSpreadAngle = 45f;
    public bool goesThrough = false;

    public Shotgun_Wave(
        float weaponCooldownSpeed,
        float shootInterval,
        float bulletSpeed,
        float bulletLifetime,
        GameObject bullet) : base(weaponCooldownSpeed, shootInterval, bulletSpeed, bulletLifetime, bullet)
    { }

    public override void Shoot(float damage, Transform barrelEnd = null, int extraBullets = 0, int penetrations = 0)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            for (int i = 0; i < bulletCount + extraBullets; i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations, BulletTypes.BASIC, goesThrough);
            }
            GameManager.Instance.AudioManager.PlayClip("player_shoot");
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            for (int i = 0;i < bulletCount + extraBullets; i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations, BulletTypes.BASIC, goesThrough);
            }
            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            control.PlayCorrectShootSound();
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}