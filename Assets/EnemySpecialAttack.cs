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

    public SpriteRenderer outerRing;
    public SpriteRenderer innerRing;


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

    Vector3 originalScale;
    public void InitSpecialAttack()
    {
        doingSpecial = true;

        originalScale = innerRing.transform.localScale;
        innerRing.transform.localScale = Vector3.one * 0.001f;
        innerRing.gameObject.SetActive(true);
        outerRing.gameObject.SetActive(true);
        StartCoroutine(SpecialAttack());
    }

    IEnumerator SpecialAttack()
    {
        // turn on rings

        float t = 0.1f;
        while (t < specialChargeDuration)
        {
            float perc = t / specialChargeDuration;

            innerRing.transform.localScale = Vector3.Lerp(Vector3.one * 0.001f, originalScale, perc);

            t += Time.deltaTime;
            yield return null;
        }
        outerRing.gameObject.SetActive(false);
        innerRing.gameObject.SetActive(false);
        doingSpecial = false;

        // did we hit something?
        Collider2D overlaps = Physics2D.OverlapCircle(transform.position, 10f);
        if (overlaps.CompareTag("Player") || overlaps.CompareTag("Friend"))
        {
            print("we hit " + overlaps.tag.ToString());
            overlaps.SendMessage("GotHit");
        }

        yield return new WaitForSeconds(specialCooldown);

        

        
        canDoSpecial = true;
    }

}
