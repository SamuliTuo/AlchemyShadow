using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitData")]
public class UnitData : ScriptableObject
{
    //public Sprite art;
    public float moveSpeed = 0.1f;
    public float shootInterval = 1;
    public float projectileSpeed = 5;
    public float damage = 1;
}
