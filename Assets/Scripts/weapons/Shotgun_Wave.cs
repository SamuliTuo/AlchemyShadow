using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

//NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
public class Shotgun_Wave : Weapon
{
    public int bulletCount = 8;
    public float shotSpreadAngle = 45f;

    public Shotgun_Wave(
        float weaponCooldownSpeed,
        float shootInterval,
        float bulletSpeed,
        float bulletLifetime,
        GameObject bullet) : base(weaponCooldownSpeed, shootInterval, bulletSpeed, bulletLifetime, bullet)
    { }

    public override void Shoot(Transform barrelEnd = null)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                var clone = Instantiate(bullet, barrelEnd.position, Quaternion.identity);
                clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime);
            }
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            for (int i = 0;i < bulletCount;i++)
            {
                var dir = point - transform.position;
                dir.z = 0;
                dir = dir.normalized;
                dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                var clone = Instantiate(bullet, transform.position, Quaternion.identity);
                clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime);
            }
            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}