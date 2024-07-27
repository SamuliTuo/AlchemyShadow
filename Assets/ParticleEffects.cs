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
    public ParticleSystem combineToSS;
    public ParticleSystem combineToSSS;

    /// <summary>
    /// Use string as input:
    /// friendSpawn,
    /// friendFreed,
    /// enemyDeath,
    /// shoot,
    /// shotHit,
    /// combineToSS, combineToSSS
    /// </summary>
    public void PlayParticles(string input, Vector3 position, Vector3 rotation, bool showInFront = false)
    {
        switch (input)
        {
            case "friendSpawn": PlayParticle(friendSpawn, position, rotation, showInFront); break;
            case "friendFreed": PlayParticle(friendFreed, position, rotation, showInFront); break;
            case "enemyDeath": PlayParticle(enemyDeath, position, rotation, showInFront); break;
            case "shoot": PlayParticle(shoot, position, rotation, showInFront); break;
            case "shotHit": PlayParticle(shotHit, position, rotation, showInFront); break;
            case "combineToSS": PlayParticle(combineToSS, position, rotation, showInFront); break;
            case "combineToSSS": PlayParticle(combineToSSS, position, rotation, showInFront); break;
        }
    }

    void PlayParticle(ParticleSystem sys, Vector3 pos, Vector3 forw, bool showInFront)
    {
        if (showInFront)
        {
            sys.transform.position = pos + new Vector3(0, 0, -1);
        }
        else
        {
            sys.transform.position = pos + new Vector3(0, 0, 1);
        }
        
        sys.transform.LookAt(sys.transform.position + forw);
        sys.Play();
    }
}
