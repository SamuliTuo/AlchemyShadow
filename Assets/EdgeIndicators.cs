using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeIndicators : MonoBehaviour
{
    private Transform leftBar;
    //leftBar = cam.GetComponent<CameraController>().leftBar;


    public LayerMask edgerRaycaster;
    public GameObject bossIndicator;

    private Camera cam;
    private Transform boss;
    public void SetBoss(Transform boss) { this.boss = boss; }

    private void Start()
    {
        cam = GameManager.Instance.cam;
    }

    int delay = 0;
    private void Update()
    {
        if (boss == null)
        {
            //boss = GameManager.Instance.GameLoop.activeBoss.transform;
            return;
        }
        if (delay > 0)
        {
            delay--;
            return;
        }

        TrackBoss();
    }

    void TrackBoss()
    {
        Vector3 mid = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        mid.z = 0;

        Vector3 rayDir = boss.transform.position - mid;
        RaycastHit2D hit = Physics2D.Raycast(mid, rayDir.normalized, rayDir.magnitude, edgerRaycaster);

        if (hit.collider != null)
        {
            bossIndicator.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
        }
        else
        {
            bossIndicator.transform.position = leftBar.position + edgeLocatorsIdlePositionOffset;
        }
    }

    public void TrackUnfreedFriends()
    {
    }
}
