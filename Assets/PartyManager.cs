using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public bool followTheObject = false;
    public Transform partyFollowObject;

    Dictionary<SlaveTypes, List<GameObject>> party = new Dictionary<SlaveTypes, List<GameObject>>();
    private Camera cam;

    [Space(10)]
    float flagTurnOffT = 1;
    public float flagTurnOffCooldown = 1;

    private void Awake()
    {
        cam = Camera.main;
        partyFollowObject.gameObject.SetActive(false);
    }
    private void Start()
    {
        foreach (SlaveTypes type in Enum.GetValues(typeof(SlaveTypes)))
        {
            party.Add(type, new List<GameObject>());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            flagTurnOffT = flagTurnOffCooldown;
            var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            partyFollowObject.position = new Vector3(point.x, point.y, 0);
            partyFollowObject.gameObject.SetActive(true);
            followTheObject = true;
        }
        else if (Input.GetMouseButton(1))
        {
            StopFlag();
        }
        else
        {
            flagTurnOffT -= Time.deltaTime;
            if (flagTurnOffT < 0)
            {
                StopFlag();
            }
        }
    }

    public void PlayerIsInFlagRange()
    {
        flagTurnOffT = flagTurnOffCooldown;
    }

    void StopFlag()
    {
        partyFollowObject.gameObject.SetActive(false);
        followTheObject = false;
    }

    public bool AddFriendAndCheckIfCombines(GameObject friend)
    {
        var control = friend.GetComponent<SlaveController>();
        List<GameObject> list;
        party.TryGetValue(control.slaveType, out list);

        if (list.Count >= 2)
        {
            var u1 = friend;
            var u2 = list[1];
            var u3 = list[0];
            party[control.slaveType].RemoveRange(0, 2);
            CombineUnits(u1, u2, u3);
            return true;
        }
        else
        {
            list.Add(friend);
            return false;
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
            case SlaveTypes.slave3_S: return SlaveTypes.slave3_SS;
            case SlaveTypes.slave4_S: return SlaveTypes.slave4_SS;

            case SlaveTypes.slave0_SS: return SlaveTypes.slave0_SSS;
            case SlaveTypes.slave1_SS: return SlaveTypes.slave1_SSS;
            case SlaveTypes.slave2_SS: return SlaveTypes.slave2_SSS;
            case SlaveTypes.slave3_SS: return SlaveTypes.slave3_SSS;
            case SlaveTypes.slave4_SS: return SlaveTypes.slave4_SSS;

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
