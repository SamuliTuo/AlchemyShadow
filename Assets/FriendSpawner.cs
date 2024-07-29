using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FriendSpawner : MonoBehaviour
{
    public List<GameObject> starterFriends = new List<GameObject>();

    public LayerMask edgerRaycaster;
    public List<GameObject> friends = new List<GameObject>();
    public float friendSpawnRate = 3;
    public float spawnGrowTime = 1;

    private Dictionary<GameObject, GameObject> unfreedFriends = new Dictionary<GameObject, GameObject>();

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
        unfreedFriends.Add(clone, GetFreeIndicator());
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
        foreach (var f in unfreedFriends)
        {
            if (f.Key == friend)
            {
                f.Value.SetActive(false);
                break;
            }
        }
        unfreedFriends.Remove(friend);

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










    public List<GameObject> indicators = new List<GameObject>();

    public void TrackUnfreedFriends()
    {
        Vector3 mid = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        mid.z = 0;
        foreach (var f in unfreedFriends)
        {
            Vector3 rayDir = f.Key.transform.position - mid;
            RaycastHit2D hit = Physics2D.Raycast(mid, rayDir.normalized, rayDir.magnitude, edgerRaycaster);

            // If it hits something...
            if (hit.collider != null)
            {
                f.Value.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                f.Value.SetActive(true);
                
                indicators[Random.Range(0, indicators.Count)].transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                print(hit.collider.name);
            }
            else
            {
                f.Value.SetActive(false);
            }
        }
    }

    GameObject GetFreeIndicator()
    {
        foreach (var f in indicators)
        {
            if (!f.activeSelf)
            {
                return f;
            }
        }
        return null;
    }
}
