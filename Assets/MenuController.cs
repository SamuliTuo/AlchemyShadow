using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public RectTransform silverScreen;
    [Space(32)]
    [Header("PUT SAME NUMBER OF THINGS IN EACH PLS! \n\n" +
        "1st element of each list corresponds with 1st of each other \n" +
        "and so on.")]
    [Space(20)]
    public List<Sprite> images = new List<Sprite>();
    public List<float> pictureDurations = new List<float>();
    public List<float> pictureMoveSpeeds = new List<float>();
    public List<Vector2> pictureMoveDirections = new List<Vector2>();

    public void StartGame()
    {
        StartCoroutine(Cutscene());
    }
    public void Settings()
    {

    }
    public void ExitGame()
    {

    }

    IEnumerator Cutscene()
    {
        
        yield return null;
        silverScreen.GetComponent<Image>().sprite = images[0];
        //leftBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.1f, Screen.height * 0.5f, cam.nearClipPlane));
        //rightBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.9f, Screen.height * 0.5f, cam.nearClipPlane));
        //topBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.9f, cam.nearClipPlane));
        //botBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.1f, cam.nearClipPlane));
        //leftBar.localPosition = new(leftBar.localPosition.x, leftBar.localPosition.y, 20);
        //rightBar.localPosition = new(rightBar.localPosition.x, rightBar.localPosition.y, 20);
        //topBar.localPosition = new(topBar.localPosition.x, topBar.localPosition.y, 20);
        //botBar.localPosition = new(botBar.localPosition.x, botBar.localPosition.y, 20);




        SceneManager.LoadScene(1);
    }
}
