using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Weapons
{
    NONE, BASIC_PEWPEW, WAVE_SHOTGUN, CLUSTER_SHOT, RANDOM_DIR, WAVE_AOE, AALTOILIJAT
}

public class PlayerWeapons : MonoBehaviour
{
    public Transform barrelEnd;
    public Transform gunArm;

    public List<Weapon> weapons;

    public float weaponsCooldownSpeedMultiplier = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;
    public bool isShooting = false;

    private float t;
    private Camera cam;

    private void Awake()
    {
        t = basicWeaponInterval;
        cam = Camera.main;
    }

    public void Update()
    {
        // only player has the gunarm
        if (gunArm != null)
        {
            var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            point.z = 0;
            gunArm.LookAt(point);
        }

        if (isShooting == false)
        {
            return;
        }
        if (t > 0)
        {
            t -= Time.deltaTime * weaponsCooldownSpeedMultiplier;
        }
        else
        {
            if (weapons[0] != null)
            {
                if (barrelEnd != null)
                {
                    weapons[0].Shoot(barrelEnd);
                }
                else
                {
                    weapons[0].Shoot();
                }
                
                t = weapons[0].shootInterval;
            }
        }
    }

    //When friends are saved they start shooting
    public void StartShooting()
    {
        isShooting = true;
    }
}





// Baseclass for  WEAPONS
[System.Serializable]
public class Weapon : MonoBehaviour
{
    public float shootInterval = 2;
    public float bulletSpeed = 10;
    public float bulletLifetime = 2;
    public GameObject bullet;
    public bool isShooting = false;

    private float t;

    public Weapon(float weaponCooldownSpeed, float shootInterval, float bulletSpeed, float bulletLifetime, GameObject bullet)
    {
        this.shootInterval = shootInterval;
        this.bulletSpeed = bulletSpeed;
        this.bulletLifetime = bulletLifetime;
        this.bullet = bullet;
    }

    public virtual void Shoot(Transform barrelEnd = null)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, barrelEnd.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime);
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, transform.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(dir, bulletSpeed, bulletLifetime);
            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}   