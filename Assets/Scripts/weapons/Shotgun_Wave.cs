using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
public class Shotgun_Wave : Weapon
{
    public int bulletCount = 8;
    public float shotSpreadAngle = 45f;
    public bool goesThrough = false;
    public int numberOfWaves = 1;
    public float waveInterval = 1.0f;
    public Shotgun_Wave(
        float weaponCooldownSpeed,
        float shootInterval,
        float bulletSpeed,
        float bulletLifetime,
        int numberOfWaves,
        GameObject bullet) : base(weaponCooldownSpeed, shootInterval, bulletSpeed, bulletLifetime, bullet)
    { }



    public override void Shoot(float damage, Transform barrelEnd = null, int extraBullets = 0, int penetrations = 0)
    {
        StartCoroutine(Shooting(damage, barrelEnd, extraBullets, penetrations));
    }



    IEnumerator Shooting(float damage, Transform barrelEnd = null, int extraBullets = 0, int penetrations = 0)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {

            var dir = GetShootingDirFromMousePos();
            for (int i = 0; i < numberOfWaves; i++)
            {
                for (int j = 0; j < bulletCount + extraBullets; j++)
                {

                    var newdir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                    var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(newdir));
                    clone.GetComponent<BulletController>().Init(
                        damage,
                        newdir,
                        Random.Range(bulletSpeed * 0.9f, bulletSpeed * 1.1f),
                        bulletLifetime,
                        penetrations,
                        BulletTypes.BASIC,
                        goesThrough);
                }
                GameManager.Instance.AudioManager.PlayClip("player_shoot");
                GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);

                float t = 0;
                while (t <= waveInterval)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            
        }
        // whoever just shoots from stomach uses this:
        else
        {
            var control = GetComponent<SlaveController>();


            for (int i = 0; i < numberOfWaves; i++)
            {
                var dir = GetShootingDirForClosestEnemyInRange(shootRangeRadius);
                if (dir == errorVector)
                {
                    yield break;
                }

                for (int j = 0; j < bulletCount + extraBullets; j++)
                {
                    var newDir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
                    var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(newDir));
                    clone.GetComponent<BulletController>().Init(
                        damage, 
                        newDir, 
                        Random.Range(bulletSpeed * 0.9f, bulletSpeed * 1.1f), 
                        bulletLifetime, 
                        penetrations, 
                        BulletTypes.BASIC, 
                        goesThrough);
                }
                control.StopAllCoroutines();
                StartCoroutine(control.ShootTween());
                control.PlayCorrectShootSound();
                float t = 0;
                while (t <= waveInterval)
                {
                    t += Time.deltaTime;
                    yield return null;
                }
            }
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }

    }




}