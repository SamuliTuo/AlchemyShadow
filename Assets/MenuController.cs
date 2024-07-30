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


    public float flashDuration = 0.1f;

    public float secondPic1stFlashPerc = 0.5f;
    public float secondPic2ndFlashPerc = 0.7f;
    public float secondPic3rdFlashPerc = 0.75f;
    public float secondPic4thFlashPerc = 0.8f;
    
    public bool did1stFlash = false;
    public bool did2ndFlash = false;
    public bool did3rdFlash = false;
    public bool did4thFlash = false;

    private float flashT = 0;

    IEnumerator Cutscene()
    {
        audiosource.Play();
        watchingCutscene = true;
        screen.gameObject.SetActive(true);
        screen.material = blackScreenMaterial;
        yield return new WaitForSeconds(firstBlackScreenDuration);

        // 1st pic
        screen.material = images[0];
        float t2 = 0;
        while (t2 < pictureDurations[0])
        {
            screen.transform.localScale = Vector3.one * Mathf.Lerp(1, 1.25f, t2 / pictureDurations[0]);
            t2 += Time.deltaTime;
            yield return null;
        }
        // black screen
        screen.material = blackScreenMaterial;
        yield return new WaitForSeconds(afterPictureDarkTime[0]);


        // 2nd pic
        screen.material = images[1];
        t2 = 0;
        while (t2 < pictureDurations[1])
        {
            if (flashT < flashDuration)
            {
                flashT += Time.deltaTime;
            }
            else if (!did4thFlash)
            {
                screen.material = images[1];
            }


            float perc = t2 / pictureDurations[1];
            if (!did1stFlash && perc >= secondPic1stFlashPerc)
            {
                did1stFlash = true;
                screen.material = images[2];
                flashT = 0;
            }
            else if (!did2ndFlash && perc >= secondPic2ndFlashPerc)
            {
                did2ndFlash = true;
                screen.material = images[2];
                flashT = 0;
            }
            else if (!did3rdFlash && perc >= secondPic3rdFlashPerc)
            {
                did3rdFlash = true;
                screen.material = images[2];
                flashT = 0;
            }
            else if (!did4thFlash && perc >= secondPic4thFlashPerc)
            {
                did4thFlash = true;
                screen.material = images[2];
                flashT = 0;
            }
            

            screen.transform.localScale = Vector3.one * Mathf.Lerp(1, 1.25f, t2 / pictureDurations[1]);
            t2 += Time.deltaTime;
            yield return null;
        }
        // black screen
        screen.material = blackScreenMaterial;
        yield return new WaitForSeconds(afterPictureDarkTime[1]);

        // 1st pic
        screen.material = images[3];
        t2 = 0;
        while (t2 < pictureDurations[2])
        {
            screen.transform.localScale = Vector3.one * Mathf.Lerp(1, 1.25f, t2 / pictureDurations[2]);
            t2 += Time.deltaTime;
            yield return null;
        }
        // black screen
        screen.material = blackScreenMaterial;
        yield return new WaitForSeconds(afterPictureDarkTime[2]);


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
