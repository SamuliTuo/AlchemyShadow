using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
    public Transform flagTransform;
    public Transform ring1, ring2;
    [Space(10)]
    public float flagTurnOffCooldown = 1;
    public bool followTheObject = false;
    public float doubleClickTimer = 0.5f;

    private Camera cam;
    Coroutine checkForDoubleClickTimer = null;

    float flagTurnOffT = 1;
    bool autoHoldDownFlag = false;

    private void Awake()
    {
        flagTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        SwapRings(ring1);
        cam = GameManager.Instance.cam;
    }



    public void OnPause()
    {
        autoHoldDownFlag = false;
        holdCancelled = true;
        SwapRings(ring1);
    }



    bool holdCancelled = false;
    private void Update()
    {
        if (GameManager.Instance.paused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (autoHoldDownFlag)
            {
                autoHoldDownFlag = false;
                SwapRings(ring1);
            }
            holdCancelled = false;
            if (checkForDoubleClickTimer == null)
            {
                checkForDoubleClickTimer = StartCoroutine(DoubleClickCheck());
            }
            else
            {
                SwapRings(ring2);
                autoHoldDownFlag = true;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0))
            {
                holdCancelled = true;
            }
            SwapRings(ring1);
            StopFlag();
        }
        else if (holdCancelled == false && (Input.GetMouseButton(0) || autoHoldDownFlag))
        {
            flagTurnOffT = flagTurnOffCooldown;
            var point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            flagTransform.position = new Vector3(point.x, point.y, 0);
            flagTransform.gameObject.SetActive(true);
            followTheObject = true;
        }
        else
        {
            flagTurnOffT -= Time.deltaTime;
            if (flagTurnOffT < 0 && !autoHoldDownFlag)
            {
                StopFlag();
            }
        }
    }

    void SwapRings(Transform activateThisRing)
    {
        if (activateThisRing == ring1)
        {
            ring1.gameObject.SetActive(true);
            ring2.gameObject.SetActive(false);
        }
        else if (activateThisRing == ring2)
        {
            ring1.gameObject.SetActive(false);
            ring2.gameObject.SetActive(true);
        }
    }



    public void AddFlagRange(float amount, Vector3 pos)
    {
        flagTransform.position = pos;
        flagTurnOffT = flagTurnOffCooldown;
        flagTransform.gameObject.SetActive(true);
        followTheObject = true;
        flagTransform.GetComponentInChildren<RingTween>().AddRingRange(amount);
    }

    public void PlayerIsInFlagRange()
    {
        flagTurnOffT = flagTurnOffCooldown;
    }

    void StopFlag()
    {
        autoHoldDownFlag = false;
        flagTransform.gameObject.SetActive(false);
        followTheObject = false;
    }

    IEnumerator DoubleClickCheck()
    {
        float t = 0;
        while (t < doubleClickTimer)
        {
            t += Time.deltaTime;
            yield return null;
        }
        checkForDoubleClickTimer = null;
    }
}
