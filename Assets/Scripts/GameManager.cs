using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public Camera cam;
    public EnemySpawner EnemySpawner { get; private set; }
    public FriendSpawner FriendSpawner { get; private set; }
    public PartyManager PartyManager { get; private set; }
    public EXPSpawner EXPSpawner { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public ParticleEffects ParticleEffects { get; private set; }
    public GameLoop GameLoop { get; private set; }
    public PauseMenu PauseManager { get; private set; }
    public RingTween RingTween { get; private set; }
    public EdgeIndicators EdgeIndicators { get; private set; }

    [HideInInspector] public Material hitFlashMaterial;
    public float enemyHitFlashTime;
    public bool paused = false;
    public Transform player;
    public LayerMask edgeLayermask;

    // Boss fight stuff
    public GameObject birdMachine;
    public GameObject blurbo;
    public GameObject demon;
    public GameObject pyramid;
    public GameObject skull;
    public GameObject snek;
    public GameObject spider;
    public GameObject bossHpBarObj;
    public Image bossHpBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        player = GameObject.Find("Player").transform;
        cam = Camera.main;
        EnemySpawner = GetComponentInChildren<EnemySpawner>();
        FriendSpawner = GetComponentInChildren<FriendSpawner>();
        PartyManager = GetComponentInChildren<PartyManager>();
        EXPSpawner = GetComponentInChildren<EXPSpawner>();
        AudioManager = GetComponentInChildren<AudioManager>();
        ParticleEffects = GetComponentInChildren<ParticleEffects>();
        GameLoop = GetComponent<GameLoop>();
        PauseManager = GetComponentInChildren<PauseMenu>();
        RingTween = GetComponentInChildren<RingTween>();
        hitFlashMaterial = Resources.Load("hitFlashMaterial") as Material;
    }

    private void Update()
    {
        GameLoop.UpdateGame();
    }
    private void FixedUpdate()
    {
        FriendSpawner.TrackUnfreedFriends();
    }

    public void PauseTheGame()
    {
        paused = true;
        Time.timeScale = 0;
    }
    public void UnpauseTheGame()
    {
        Time.timeScale = 1;
        StartCoroutine(Unpause());
    }
    IEnumerator Unpause()
    {
        yield return new WaitForSeconds(0.5f);
        paused = false;
    }

    public bool playerIsAlive = true;
    public void PlayerDied()
    {
        playerIsAlive = false;
        PauseManager.OpenDiedPanel();
        //open end game popup
        //GameLoop.PlayerDied();
    }
    public void PlayerWin()
    {
        SceneManager.LoadScene(2);
    }


    // Helper methods
    int deepnes = 0;
    public Vector3 GetRandomPosAtScreenEdge()
    {
        deepnes = 0;
        return DoThat();
    }
    Vector3 DoThat()
    {
        Vector2 randomCoordinates;
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) > 0)
            {
                randomCoordinates = new Vector2(Random.Range(0.0f, Screen.width), 0);
            }
            else
            {
                randomCoordinates = new Vector2(Random.Range(0.0f, Screen.width), Screen.height);
            }
        }
        else
        {
            if (Random.Range(0, 2) > 0)
            {
                randomCoordinates = new Vector2(0, Random.Range(0.0f, Screen.height));
            }
            else
            {
                randomCoordinates = new Vector2(Screen.width, Random.Range(0.0f, Screen.height));
            }
        }
        var point = cam.ScreenToWorldPoint(new Vector3(randomCoordinates.x, randomCoordinates.y, cam.nearClipPlane));

        if (Physics2D.OverlapPoint(point, edgeLayermask))
        {
            //inside a wall
            if (deepnes > 100)
            {
                return Vector3.zero;
            }
            else
            {
                deepnes++;
                return GetRandomPosAtScreenEdge();
            }
        }

        return point;
    }

    public int[] GenerateRandomUniqueIntegers(Vector2Int countRange, Vector2Int valueRange)
    {
        if (valueRange == Vector2Int.zero)
            return null;

        var values = new List<int>();
        for (int i = Mathf.Min(valueRange.x, valueRange.y); i < Mathf.Max(valueRange.x, valueRange.y); i++)
            values.Add(i);

        var randomNumbers = new int[Random.Range(Mathf.Min(countRange.x, countRange.y), Mathf.Max(countRange.x, countRange.y))];
        for (int i = 0; i < randomNumbers.Length; i++)
        {
            if (values.Count == 0)
                continue;

            var thisNumber = Random.Range(0, values.Count);
            randomNumbers[i] = values[thisNumber];
            values.RemoveAt(thisNumber);
        }

        return randomNumbers;
    }
}
