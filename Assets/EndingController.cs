using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    float t = 0;
    private void Update()
    {
        if (t < 1)
        {
            t += Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(0);
        }
    }
}