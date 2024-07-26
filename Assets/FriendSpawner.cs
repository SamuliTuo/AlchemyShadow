using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSpawner : MonoBehaviour
{
    public List<GameObject> starterFriends = new List<GameObject>();

    public List<GameObject> friends = new List<GameObject>();
    public float friendSpawnRate = 3;

    private Transform player;
    private Camera cam;
    
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        cam = Camera.main;
    }


    // Randomly spawning units
    public GameObject SpawnAFriend()
    {
        var f = starterFriends[Random.Range(0, starterFriends.Count)];
        if (f == null)
        {
            return null;
        }
        var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
        pos.z = 0;
        var clone = Instantiate(f, pos, Quaternion.identity);
        return clone;
    }


    // When upgrading
    public GameObject SpawnAFreeFriend(SlaveTypes type, Vector3 pos)
    {
        GameObject friend = null;
        foreach (var f in friends)
        {
            if (f.GetComponent<SlaveController>().slaveType == type)
            {
                friend = f;
            }
        }
        if (friend == null)
        {
            return null;
        }
        var clone = Instantiate(friend, pos, Quaternion.identity);
        clone.GetComponent<SlaveController>().SetFree();
        return clone;
    }
}
