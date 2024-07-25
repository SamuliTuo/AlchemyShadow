using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EXPTiers
{
    small, medium, big,
}

public class EXPSpawner : MonoBehaviour
{
    public GameObject expSmall;
    public GameObject expMedium;
    public GameObject expBig;

    public void SpawnEXP(Vector3 position, EXPTiers tier)
    {
        switch (tier)
        {
            case EXPTiers.small:
                SpawnExp(position, expSmall);
                break;
            case EXPTiers.medium:
                SpawnExp(position, expMedium);
                break;
            case EXPTiers.big:
                SpawnExp(position, expBig);
                break;
            default:
                break;
        }
    }

    void SpawnExp(Vector3 position, GameObject obj)
    {
        var clone = Instantiate(obj, position, Quaternion.identity);
    }
}
