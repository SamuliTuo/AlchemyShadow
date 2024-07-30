using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyWeapons : MonoBehaviour
{
    public Transform barrelEnd;
    public Transform gunArm;
    //public List<Weapon> weapons;

    public float shootInterval = 1;
    public float damage = 1;
    public float bulletLifeTime = 2;
    public float weaponsCooldownSpeedMultiplier = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;
    public float bulletSpeed;
    public bool isShooting = false;

    private float t;
    private Camera cam;
    private Transform player;

    private void Awake()
    {
        t = 1;
        cam = Camera.main;
    }

    public void Update()
    {
        if (player == null)
        {
            player = GameManager.Instance.player;
        }

        // only player and boss has the gunarm
        if (gunArm != null && GameManager.Instance.paused == false)
        {
            //make a random shooter
            //var dir = Random.insideUnitCircle.normalized;

            var dir = (player.transform.position - transform.position).normalized;
            gunArm.LookAt(transform.position + new Vector3(dir.x, dir.y, 0));
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
            if (barrelEnd != null)
            {
                Shoot(damage, barrelEnd);
            }
            else
            {
                Shoot(damage);
            }

            t = Mathf.Max(shootInterval, 0.05f);
        }
    }
    


    public void Shoot(float damage, Transform barrelEnd = null)
    {
        var dir = (player.transform.position - transform.position).normalized;
        // player has barrel sooo...
        if (barrelEnd != null)
        {
            var clone = Instantiate(bullet_basic, barrelEnd.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifeTime, BulletTypes.BASIC, false, false, true);
            GameManager.Instance.ParticleEffects.PlayParticles("shoot", barrelEnd.position, barrelEnd.forward, true);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            var clone = Instantiate(bullet_basic, transform.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(damage, dir, bulletSpeed, bulletLifeTime, BulletTypes.BASIC, false, false, true);
            //var control = GetComponent<EnemyController>();
            //control.StopAllCoroutines();
            //StartCoroutine(control.ShootTween());
            //GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        }
    }
}