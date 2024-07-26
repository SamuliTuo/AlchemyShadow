using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSpawner : MonoBehaviour
{
    public List<GameObject> friends = new List<GameObject>();

    private Transform player;
    private Camera cam;
    
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        cam = Camera.main;
    }



    public void SpawnAFriend()
    {
        var f = friends[Random.Range(0, friends.Count)];
        if (f == null)
        {
            return;
        }

        Instantiate(f, GameManager.Instance.GetRandomPosAtScreenEdge(), Quaternion.identity);
    }
}
