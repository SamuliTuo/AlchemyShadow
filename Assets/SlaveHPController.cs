using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTiers
{
    NONE, LIGHT, MEDIUM, HEAVY
}

public class SlaveHPController : MonoBehaviour
{
    [HideInInspector] public bool isInvulnerable = false;
    [HideInInspector] public DamageTiers currentDamageAmount = DamageTiers.NONE;
    public SlaveTier tier = SlaveTier.S;

    public float hitFlashTime;
    public float invulnTime;
    public float healTime;
    public Color invulnColor;
    public Color damagedColor_light;
    public Color damagedColor_medium;
    public Color damagedColor_heavy;

    Material hitFlashMaterial;
    Material defaultMaterial;
    SpriteRenderer graphics;
    SlaveController control;


    private void Awake()
    {
        graphics = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = graphics.material;
        control = GetComponent<SlaveController>();
    }
    private void Start()
    {
        hitFlashMaterial = GameManager.Instance.hitFlashMaterial;
    }

    public void TookDamage(float damage = 1)
    {
        GameManager.Instance.ParticleEffects.PlayParticles("friendDamaged", transform.position, Vector3.up);
        if (currentDamageAmount == DamageTiers.HEAVY)
        {
            GameManager.Instance.ParticleEffects.PlayParticles("friendSpawn", transform.position, Vector3.up);
            GameManager.Instance.PartyManager.FriendDied(gameObject);
            GameManager.Instance.EXPSpawner.SpawnEXP(transform.position, EXPTiers.small);
            control.PlayCorrectDeathSound();
            Destroy(gameObject);
            return;
        }
        control.PlayCorrectHitSound();
        StopAllCoroutines();
        StartCoroutine(InvulnerabilityAndDamagedCoroutine());
    }

    IEnumerator InvulnerabilityAndDamagedCoroutine()
    {
        isInvulnerable = true;
        graphics.material = hitFlashMaterial;

        // change currentDamage amount
        switch (tier)
        {
            case SlaveTier.S:
                currentDamageAmount = DamageTiers.HEAVY;
                break;

            case SlaveTier.SS:
                if (currentDamageAmount == DamageTiers.NONE)
                {
                    currentDamageAmount = DamageTiers.MEDIUM;
                }
                else
                {
                    currentDamageAmount = DamageTiers.HEAVY;
                }
                break;

            case SlaveTier.SSS:
                if (currentDamageAmount == DamageTiers.NONE)
                {
                    currentDamageAmount = DamageTiers.LIGHT;
                }
                if (currentDamageAmount == DamageTiers.LIGHT)
                {
                    currentDamageAmount = DamageTiers.MEDIUM;
                }
                else if (currentDamageAmount == DamageTiers.MEDIUM)
                {
                    currentDamageAmount = DamageTiers.HEAVY;
                }
                break;

            default: break;
        }
        yield return new WaitForSeconds(hitFlashTime);

        graphics.material = defaultMaterial;
        graphics.color = invulnColor;

        
        // wait for invu
        yield return new WaitForSeconds(invulnTime);
        isInvulnerable = false;


        // invu over, make me look damaged and heal me
        switch (tier)
        {
            // S tier
            case SlaveTier.S:
                switch (currentDamageAmount)
                {
                    case DamageTiers.LIGHT:
                        graphics.color = damagedColor_light;
                        yield return new WaitForSeconds(healTime);
                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.MEDIUM:
                        graphics.color = damagedColor_medium;
                        yield return new WaitForSeconds(healTime);
                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.HEAVY:
                        graphics.color = damagedColor_heavy;
                        yield return new WaitForSeconds(healTime);
                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    default: break;
                }
                break;

            // SS tier
            case SlaveTier.SS:
                switch (currentDamageAmount)
                {
                    case DamageTiers.LIGHT:
                        graphics.color = damagedColor_light;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.MEDIUM:
                        graphics.color = damagedColor_medium;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.HEAVY:
                        graphics.color = damagedColor_heavy;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        graphics.color = damagedColor_medium;
                        currentDamageAmount = DamageTiers.MEDIUM;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    default: break;
                }
                break;

            // SSS tier
            case SlaveTier.SSS:
                switch (currentDamageAmount)
                {
                    case DamageTiers.LIGHT:
                        graphics.color = damagedColor_light;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.MEDIUM:
                        graphics.color = damagedColor_medium;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        graphics.color = damagedColor_light;
                        currentDamageAmount = DamageTiers.LIGHT;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    case DamageTiers.HEAVY:
                        graphics.color = damagedColor_heavy;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        graphics.color = damagedColor_medium;
                        currentDamageAmount = DamageTiers.MEDIUM;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        graphics.color = damagedColor_light;
                        currentDamageAmount = DamageTiers.LIGHT;
                        yield return new WaitForSeconds(healTime);

                        GameManager.Instance.ParticleEffects.PlayParticles("friendHeal", transform.position, Vector3.up);
                        currentDamageAmount = DamageTiers.NONE;
                        graphics.color = Color.white;
                        break;

                    default: break;
                }
                break;

            default: 
                break;
        }
    }
}
