using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{

    [Header("Particle systems:")]
    public ParticleSystem friendSpawn;
    public ParticleSystem friendFreed;
    public ParticleSystem enemyDeath;
    public ParticleSystem shoot;
    public ParticleSystem shotHit;

    /// <summary>
    /// Use string as input:
    /// friendSpawn,
    /// friendFreed,
    /// enemyDeath,
    /// shoot,
    /// shotHit,
    /// </summary>
    public void PlayParticles(string input, Vector3 position, Vector3 rotation)
    {
        switch (input)
        {
            case "friendSpawn": PlayParticle(friendSpawn, position, rotation); break;
            case "friendFreed": PlayParticle(friendFreed, position, rotation); break;
            case "enemyDeath": PlayParticle(enemyDeath, position, rotation); break;
            case "shoot": PlayParticle(shoot, position, rotation); break;
            case "shotHit": PlayParticle(shotHit, position, rotation); break;
        }
    }

    void PlayParticle(ParticleSystem sys, Vector3 pos, Vector3 forw)
    {
        sys.transform.position = pos + new Vector3(0, 0, 1);
        sys.transform.LookAt(sys.transform.position + forw);
        sys.Play();
    }
}
