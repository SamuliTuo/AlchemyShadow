using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    Dictionary<SlaveTypes, List<GameObject>> party = new Dictionary<SlaveTypes, List<GameObject>>();
		

    private void Start()
    {
        foreach (SlaveTypes type in Enum.GetValues(typeof(SlaveTypes)))
        {
            party.Add(type, new List<GameObject>());
        }
    }

    public void AddFriend(GameObject friend)
    {
        var control = friend.GetComponent<SlaveController>();
        List<GameObject> list;
        party.TryGetValue(control.slaveType, out list);

        //print("adding a '"+friend.GetComponent<SlaveController>().slaveType+ "' unit to a list of: " + list.Count);
        list.Add(friend);

        if (list.Count >= 3)
        {
            var u1 = list[2];
            var u2 = list[1];
            var u3 = list[0];
            party[control.slaveType].RemoveRange(0, 3);
            CombineUnits(u1, u2, u3);
        }
    }

    public void FriendDied(GameObject friend)
    {
        var control = friend.GetComponent<SlaveController>();
        List<GameObject> list;
        party.TryGetValue(control.slaveType, out list);

        if (list.Contains(friend))
        {
            list.Remove(friend);
        }
        party[control.slaveType] = list;
    }

    void CombineUnits(GameObject unit1, GameObject unit2, GameObject unit3)
    {
        var type = GetUpgradedSlaveType(unit1.GetComponent<SlaveController>().slaveType);

        var totalX = 0f;
        var totalY = 0f;
        totalX += unit1.transform.position.x;
        totalX += unit2.transform.position.x;
        totalX += unit3.transform.position.x;
        totalY += unit1.transform.position.y;
        totalY += unit2.transform.position.y;
        totalY += unit3.transform.position.y;
        var centerX = totalX / 3;
        var centerY = totalY / 3;

        Destroy(unit1);
        Destroy(unit2);
        Destroy(unit3);

        GameManager.Instance.FriendSpawner.SpawnAFreeFriend(type, new Vector3(centerX, centerY, 0));
    }

    SlaveTypes GetUpgradedSlaveType(SlaveTypes type)
    {
        switch (type)
        {
            case SlaveTypes.slave0_S: return SlaveTypes.slave0_SS;
            case SlaveTypes.slave1_S: return SlaveTypes.slave1_SS;
            case SlaveTypes.slave2_S: return SlaveTypes.slave2_SS;

            case SlaveTypes.slave0_SS: return SlaveTypes.slave0_SSS;
            case SlaveTypes.slave1_SS: return SlaveTypes.slave1_SSS;
            case SlaveTypes.slave2_SS: return SlaveTypes.slave2_SSS;

            default: return SlaveTypes.NULL;
        }
    }
}

public class PartyUnit
{
    public SlaveTypes type;
    public GameObject obj;
    public SlaveController controller;

    public PartyUnit(GameObject obj, SlaveController controller)
    {
        this.type = controller.slaveType;
        this.obj = obj;
        this.controller = controller;
    }
}
