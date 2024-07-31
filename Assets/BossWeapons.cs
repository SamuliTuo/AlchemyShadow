using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    public Transform barrelEnd;
    public Transform gunArm;

    public List<Weapon> weapons;

    public GameObject bullet;
    public float damage = 1;
    public float weaponsCooldownSpeedMultiplier = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;
    public bool isShooting = false;

    BossController controller;
    private Camera cam;
    private Transform player;

    public void Init(BossController contrl)
    {
        controller = contrl;
        cam = GameManager.Instance.cam;
        player = GameManager.Instance.player;
    }



    //weapons[0].Shoot(damage, barrelEnd, additionalBullets, additionalBulletPenetrations);
    //weapons[0].Shoot(damage, null, additionalBullets, additionalBulletPenetrations);


    //var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    //point.z = 0;
    //gunArm.LookAt(point);





    

    public IEnumerator ShootRings(float rotationSpeed, float duration, float shootInterval, float bulletSpeed, float bulletLifetime)
    {
        Vector3 dir = Random.insideUnitCircle.normalized;
        float t = 0;
        float interval = 0;
        while (t < duration)
        {
            dir = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward) * dir;

            //arm
            if (gunArm != null && GameManager.Instance.paused == false)
            {
                var point = gunArm.position + dir;
                gunArm.LookAt(point);
            }

            if (interval >= shootInterval)
            {
                var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, 0, BulletTypes.BASIC, false, false, true);
                //GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
                interval = 0;
            }

            t += Time.deltaTime;
            interval += Time.deltaTime;
            print("shooting rings, t: " + t + ",  duration: " + duration);
            yield return null;
        }
        //then rotate
        print("shootrings end");
        controller.currentAction = StartCoroutine(controller.WaitAfterShoot(2));

    }
    public IEnumerator ShootAtRandomDirections(float duration, float shootInterval, float damage, float bulletSpeed, float bulletLifetime)
    {
        float t = 0;
        float interval = 0;
        while (t < duration)
        {
            if (interval >= shootInterval)
            {
                Vector3 dir = Random.insideUnitCircle;
                dir = dir.normalized;

                //arm
                if (gunArm != null && GameManager.Instance.paused == false)
                {
                    var point = gunArm.position + dir;
                    gunArm.LookAt(point);
                }

                var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
                clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, 0, BulletTypes.SINE_WAVE, false, Random.Range(0,2)==0, true);
                //GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
                interval = 0;
            }

            t += Time.deltaTime;
            interval += Time.deltaTime;
            yield return null;
        }
        print("shootrandom end");
        controller.currentAction = StartCoroutine(controller.WaitAfterShoot(2));
    }

    public IEnumerator ShootAtPlayer(float aimTime, float damage, float bulletSpeed, float bulletLifetime)
    {
        print("shooting at plr");
        //var point = GameManager.Instance.cam.ScreenToWorldPoint(player.posi)); //(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));
        yield return new WaitForSeconds(aimTime);

        var dir = player.position - transform.position;
        dir.z = 0;
        dir = dir.normalized;

        //arm
        if (gunArm != null && GameManager.Instance.paused == false)
        {
            var point = gunArm.position + dir;
            gunArm.LookAt(point);
        }

        var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
        clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, 0, BulletTypes.BASIC, false, false, true);
        GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        //shoot sound?
        controller.currentAction = StartCoroutine(controller.WaitAfterShoot(1));
        print("focus shot coroutine ended");
    }

    //public IEnumerator ShootShotgun(float damage, Transform barrelEnd = null, int extraBullets = 0, int penetrations = 0)
    //{
    //    var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

    //    // player has barrel sooo...
    //    if (barrelEnd != null)
    //    {
    //        for (int i = 0; i < bulletCount + extraBullets; i++)
    //        {
    //            var dir = point - transform.position;
    //            dir.z = 0;
    //            dir = dir.normalized;
    //            dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
    //            var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
    //            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations);
    //        }
    //        GameManager.Instance.AudioManager.PlayClip("player_shoot");
    //        GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
    //    }
    //    // whoever just shoots from stomach uses this:
    //    else
    //    {
    //        for (int i = 0; i < bulletCount + extraBullets; i++)
    //        {
    //            var dir = point - transform.position;
    //            dir.z = 0;
    //            dir = dir.normalized;
    //            dir = Quaternion.Euler(0, 0, Random.Range(-shotSpreadAngle, shotSpreadAngle)) * dir;
    //            var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
    //            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, penetrations);
    //        }
    //        var control = GetComponent<SlaveController>();
    //        control.StopAllCoroutines();
    //        StartCoroutine(control.ShootTween());
    //        control.PlayCorrectShootSound();
    //        //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
    //    }
    //}
}

