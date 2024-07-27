using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public Transform barrelEnd;
    public Transform gunArm;

    public float weaponsCooldownSpeed = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;
    public bool isShooting = false;

    private float timer_basicWeapon;
    private Camera cam;

    private void Awake()
    {
        timer_basicWeapon = basicWeaponInterval;
        cam = Camera.main;
    }

    public void Update()
    {
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
        if (timer_basicWeapon > 0)
        {
            timer_basicWeapon -= Time.deltaTime * weaponsCooldownSpeed;
        }
        else
        {
            Shoot();
            timer_basicWeapon = basicWeaponInterval;
        }
    }

    void LateUpdate()
    {
        
    }

    public void StartShooting()
    {
        isShooting = true;
    }
    
    void Shoot()
    {
        GameManager.Instance.ParticleEffects.PlayParticles("shoot", transform.position, transform.forward);
        var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));

        // player has barrel sooo...
        if (barrelEnd != null)
        {
            var dir = point - barrelEnd.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet_basic, barrelEnd.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(dir, basicWeaponBulletSpeed, basicWeaponBulletLifetime);
        }
        // whoever just shoots from stomach uses this:
        else
        {
            var dir = point - transform.position;
            dir.z = 0;
            dir = dir.normalized;
            var clone = Instantiate(bullet_basic, transform.position, Quaternion.identity);
            clone.GetComponent<BulletController>().Init(dir, basicWeaponBulletSpeed, basicWeaponBulletLifetime);
        } 
    }
}
