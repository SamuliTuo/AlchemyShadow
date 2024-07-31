using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSpawner : MonoBehaviour
{
    public List<GameObject> starterFriends = new List<GameObject>();

    public LayerMask edgerRaycaster;
    public List<GameObject> friends = new List<GameObject>();
    public float friendSpawnRate = 3;
    public float spawnGrowTime = 1;

    private List<GameObject> unfreedFriends = new List<GameObject>();
    private List<GameObject> friendIndicators = new List<GameObject>();
    //private Dictionary<GameObject, GameObject> unfreedFriends = new Dictionary<GameObject, GameObject>();

    private Transform player;
    private Camera cam;
    private Vector3 edgeLocatorsIdlePositionOffset = Vector3.left * 100f;
    private Transform leftBar;
    
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        cam = Camera.main;
        leftBar = cam.GetComponent<CameraController>().leftBar;
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
        unfreedFriends.Add(clone);
        friendIndicators.Add(GetFreeIndicator());
        //unfreedFriends.Add(clone, GetFreeIndicator());
        GameManager.Instance.ParticleEffects.PlayParticles("friendSpawn", pos, Vector3.up);
        StartCoroutine(SpawnTween(clone));
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

    public void FriendWasFreed(GameObject friend)
    {
        for (int i = 0; i < unfreedFriends.Count; i++)
        {
            if (unfreedFriends[i] == friend)
            {
                friendIndicators[i].SetActive(false);
                unfreedFriends.RemoveAt(i);
                friendIndicators.RemoveAt(i);
                break;
            }
        }
    }


    IEnumerator SpawnTween(GameObject obj)
    {
        float originalScale = obj.transform.GetChild(0).localScale.x;
        obj.transform.GetChild(0).localScale = Vector3.one * 0.001f;
        float t = 0;
        while (t < spawnGrowTime)
        {
            float perc = t / spawnGrowTime;
            perc = Mathf.Sin(perc * Mathf.PI * 0.5f);
            obj.transform.GetChild(0).localScale = Vector3.one * Mathf.Lerp(0, originalScale, perc);
            t += Time.deltaTime;
            yield return null;
        }
        obj.transform.GetChild(0).localScale = Vector3.one * originalScale;
    }




    // Tracking
    public List<GameObject> indicators = new List<GameObject>();
    private int indexBeingUpdated = 0;
    public void TrackUnfreedFriends()
    {
        if (unfreedFriends.Count <= 0)
        {
            return;
        }

        Vector3 mid = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        mid.z = 0;

        if (indexBeingUpdated >= unfreedFriends.Count)
        {
            indexBeingUpdated = 0;
        }
        var friend = unfreedFriends[indexBeingUpdated];
        var indicator = friendIndicators[indexBeingUpdated];

        Vector3 rayDir = friend.transform.position - mid;
        RaycastHit2D hit = Physics2D.Raycast(mid, rayDir.normalized, rayDir.magnitude, edgerRaycaster);

        if (hit.collider != null)
        {
            indicator.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
        }
        else
        {
            indicator.transform.position = leftBar.position + edgeLocatorsIdlePositionOffset;
        }

        indexBeingUpdated++;
    }

    GameObject GetFreeIndicator()
    {
        foreach (var f in indicators)
        {
            if (f.activeSelf == false)
            {
                f.SetActive(true);
                f.transform.position = leftBar.position + edgeLocatorsIdlePositionOffset;
                return f;
            }
        }
        return null;
    }
}
