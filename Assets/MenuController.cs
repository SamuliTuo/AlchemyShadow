using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject silverScreen;
    private Image screen;
    [Space(32)]
    [Header("PUT SAME NUMBER OF THINGS IN EACH PLS! \n\n" +
        "1st element of each list corresponds with 1st of each other \n" +
        "and so on.")]
    [Space(20)]
    public Material blackScreenMaterial;
    public float firstBlackScreenDuration = 1;
    public List<Material> images = new List<Material>();
    public List<float> pictureDurations = new List<float>();
    public List<float> afterPictureDarkTime = new List<float>();
    //public List<float> pictureMoveSpeeds = new List<float>();
    //public List<Vector2> pictureMoveDirections = new List<Vector2>();

    bool watchingCutscene = false;
    public float cutSceneTimerBeforeSkip = 1;
    float t = 0;
    AudioSource audiosource;


    private void Start()
    {
        screen = silverScreen.GetComponent<Image>();
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (watchingCutscene)
        {
            if (t < cutSceneTimerBeforeSkip)
            {
                print("slow down " + Time.time);
                t += Time.deltaTime;
                return;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                print("go skip");
                StopAllCoroutines();
                ChangeToGameScene();
            }
        }
    }


    public void StartGame()
    {
        StartCoroutine(Cutscene());
    }
    public void Settings()
    {
        print("implemtnt settngs");
    }
    public void ExitGame()
    {
        print("impelemnt exit gamepls!");
    }


    IEnumerator Cutscene()
    {
        audiosource.Play();
        watchingCutscene = true;
        screen.gameObject.SetActive(true);
        screen.material = blackScreenMaterial;
        yield return new WaitForSeconds(firstBlackScreenDuration);

        for (int i = 0; i < images.Count; i++)
        {
            screen.material = images[i];
            float t2 = 0;
            while (t2 < pictureDurations[i])
            {
                screen.transform.localScale = Vector3.one * Mathf.Lerp(1, 1.25f, t2 / pictureDurations[i]);
                t2 += Time.deltaTime;
                yield return null;
            }
            
            //yield return new WaitForSeconds(pictureDurations[i]);
            screen.material = blackScreenMaterial;
            yield return new WaitForSeconds(afterPictureDarkTime[i]);
        }

        ChangeToGameScene();

        //silverScreen.GetComponent<Image>().sprite = images[0];
        //leftBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.1f, Screen.height * 0.5f, cam.nearClipPlane));
        //rightBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.9f, Screen.height * 0.5f, cam.nearClipPlane));
        //topBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.9f, cam.nearClipPlane));
        //botBar.position = cam.ScreenToWorldPoint(new(Screen.width * 0.5f, Screen.height * 0.1f, cam.nearClipPlane));
        //leftBar.localPosition = new(leftBar.localPosition.x, leftBar.localPosition.y, 20);
        //rightBar.localPosition = new(rightBar.localPosition.x, rightBar.localPosition.y, 20);
        //topBar.localPosition = new(topBar.localPosition.x, topBar.localPosition.y, 20);
        //botBar.localPosition = new(botBar.localPosition.x, botBar.localPosition.y, 20);




    }

    void ChangeToGameScene()
    {
        watchingCutscene = false;
        SceneManager.LoadScene(1);
    }
}
