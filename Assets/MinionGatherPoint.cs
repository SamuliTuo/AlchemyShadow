using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGatherPoint : MonoBehaviour
{
    public void UpdateGatherPointPosition(Vector3 aimDir)
    {
        transform.localPosition = aimDir * GameManager.Instance.PartyManager.GetPartyCount();
    }
}
