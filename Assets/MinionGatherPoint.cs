using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionGatherPoint : MonoBehaviour
{
    public void UpdateGatherPointPosition(Vector3 aimDir)
    {
        transform.localPosition = aimDir * Mathf.Max(GameManager.Instance.PartyManager.GetPartyCount() * 0.1f, 0.25f);
    }
}
