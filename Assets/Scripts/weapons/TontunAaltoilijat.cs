using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

//NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
public class TontunAaltoilijat : Weapon
{
    public bool shootDoubleWave = false;

    public TontunAaltoilijat(
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
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
            clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime, BulletTypes.SINE_WAVE, true, false);
            if (shootDoubleWave)
            {
                var clone2 = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
                clone2.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime, BulletTypes.SINE_WAVE, true, true);
            }

            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
            clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime, BulletTypes.SINE_WAVE, true, false);
            if (shootDoubleWave)
            {
                var clone2 = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
                clone2.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime, BulletTypes.SINE_WAVE, true, true);
            }

            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}