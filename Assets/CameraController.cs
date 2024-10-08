using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform leftBar, rightBar, topBar, botBar;

    public Transform player;


    private void Start()
    {
        var cam = Camera.main;
        leftBar.position = cam.ScreenToWorldPoint(new (Screen.width * 0.03f, Screen.height * 0.5f, cam.nearClipPlane));
        rightBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.97f, Screen.height * 0.5f, cam.nearClipPlane));
        topBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.97f, cam.nearClipPlane));
        botBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.03f, cam.nearClipPlane));
        leftBar.localPosition = new(leftBar.localPosition.x, leftBar.localPosition.y, 20);
        rightBar.localPosition = new(rightBar.localPosition.x, rightBar.localPosition.y, 20);
        topBar.localPosition = new(topBar.localPosition.x, topBar.localPosition.y, 20);
        botBar.localPosition = new(botBar.localPosition.x, botBar.localPosition.y, 20);
    }
    
    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(player.transform.position.x, player.transform.position.y, -20), 
            0.3f);
    }
}
