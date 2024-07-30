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

    public float damage = 1;
    public float weaponsCooldownSpeedMultiplier = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;
    public bool isShooting = false;

    private int additionalBulletPenetrations = 0;
    private int additionalBullets = 0;
    public void AddBulletPenetrations(int amount)
    {
        additionalBulletPenetrations += amount;
    }
    public void AddAdditionalBullets(int amount)
    {
        additionalBullets += amount;
    }
    private float t;
    private Camera cam;

    private void Awake()
    {
        t = 1;
        cam = Camera.main;
    }

    public void Update()
    {
        // only player has the gunarm
        if (gunArm != null && GameManager.Instance.paused == false)
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
                    weapons[0].Shoot(damage, barrelEnd, additionalBullets, additionalBulletPenetrations);
                }
                else
                {
                    weapons[0].Shoot(damage, null, additionalBullets, additionalBulletPenetrations);
                }

                t = Mathf.Max(weapons[0].shootInterval / attackSpd, 0.05f);
            }
        }
    }

    float attackSpd = 1;
    //When friends are saved they start shooting
    public void StartShooting()
    {
        isShooting = true;
    }

    public void AddDamage(float addition)
    {
        damage += addition;
    }
    public void AddAttackSpeed(float addition)
    {
        attackSpd += addition;
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

    public virtual void Shoot(float damage, Transform barrelEnd = null, int additionalBullets = 0, int additionalBulletPenetrations = 0)
    {
        var point = GameManager.Instance.cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameManager.Instance.cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, barrelEnd.position, Quaternion.LookRotation(dir));
            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, additionalBulletPenetrations);
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);


        }
        // whoever just shoots from stomach uses this:
        else
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet, transform.position, Quaternion.LookRotation(dir));
            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifetime, additionalBulletPenetrations);
            var control = GetComponent<SlaveController>();
            control.StopAllCoroutines();
            StartCoroutine(control.ShootTween());
            control.PlayCorrectShootSound();
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}   