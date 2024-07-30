using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttack : MonoBehaviour
{
    [HideInInspector] public bool canDoSpecial = true;
    [HideInInspector] public bool doingSpecial = false;

    public float specialRangeSquared = 5f;
    public float specialChargeDuration = 3;
    public float specialCooldown = 3;
    public int specialDamage = 3;


    public bool ShouldWeDoSpecialNow(Transform player)
    {
        if (!canDoSpecial)
        {
            return false;
        }

        var dist = (player.position - transform.position).sqrMagnitude;
        if (dist < specialRangeSquared)
        {
            return true;
        }
        return false;
    }

    public void InitSpecialAttack()
    {
        doingSpecial = true;
        StartCoroutine(SpecialAttack());
    }

    IEnumerator SpecialAttack()
    {
        // turn on rings

        float t = 0;
        while (t < specialChargeDuration)
        {
            float perc = t / specialChargeDuration;

            // update ring

            t += Time.deltaTime;
            yield return null;
        }

        // turn off rings

        doingSpecial = false;

        yield return new WaitForSeconds(specialCooldown);

        canDoSpecial = true;
    }

}
