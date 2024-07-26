using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {

        transform.position = Vector3.Lerp(
            transform.position, 
            new Vector3(player.transform.position.x, player.transform.position.y, -20), 
            0.3f);
    }
}
