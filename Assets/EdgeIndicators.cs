using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeIndicators : MonoBehaviour
{
    public GameObject bossIndicator;

    private Camera cam;


    private void Start()
    {
        cam = GameManager.Instance.cam;
    }

    int t = 0;
    private void Update()
    {
        
    }

    public void TrackUnfreedFriends()
    {
        //Vector3 mid = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        //mid.z = 0;

        //if (indexBeingUpdated >= unfreedFriends.Count)
        //{
        //    indexBeingUpdated = 0;
        //}
        //var friend = unfreedFriends[indexBeingUpdated];
        //var indicator = friendIndicators[indexBeingUpdated];

        //Vector3 rayDir = friend.transform.position - mid;
        //RaycastHit2D hit = Physics2D.Raycast(mid, rayDir.normalized, rayDir.magnitude, edgerRaycaster);

        //if (hit.collider != null)
        //{
        //    indicator.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
        //}
        //else
        //{
        //    indicator.transform.position = leftBar.position + edgeLocatorsIdlePositionOffset;
        //}

        //indexBeingUpdated++;
    }
}
