using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float weaponsCooldownSpeed = 1;
    public float basicWeaponInterval = 2;
    public float basicWeaponBulletSpeed = 10;
    public float basicWeaponBulletLifetime = 2;
    public GameObject bullet_basic;

    private float timer_basicWeapon;
    private Camera cam;

    private void Awake()
    {
        timer_basicWeapon = basicWeaponInterval;
        cam = Camera.main;
    }

    public void UpdateWeapons()
    {
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

    void Shoot()
    {
        var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        var dir = point - transform.position;
        dir.z = 0;
        dir = dir.normalized;
        var clone = Instantiate(bullet_basic, transform.position, Quaternion.identity);
        clone.GetComponent<BulletController>().Init(dir, basicWeaponBulletSpeed, basicWeaponBulletLifetime);
    }
}
