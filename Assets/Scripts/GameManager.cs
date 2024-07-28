using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Camera cam;
    public EnemySpawner EnemySpawner { get; private set; }
    public FriendSpawner FriendSpawner { get; private set; }
    public PartyManager PartyManager { get; private set; }
    public EXPSpawner EXPSpawner { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public ParticleEffects ParticleEffects { get; private set; }
    private Coroutine gameLoop = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        cam = Camera.main;
        EnemySpawner = GetComponentInChildren<EnemySpawner>();
        FriendSpawner = GetComponentInChildren<FriendSpawner>();
        PartyManager = GetComponentInChildren<PartyManager>();
        EXPSpawner = GetComponentInChildren<EXPSpawner>();
        AudioManager = GetComponentInChildren<AudioManager>();
        ParticleEffects = GetComponentInChildren<ParticleEffects>();
    }

    private void Start()
    {
        if (gameLoop == null)
        {
            gameLoop = StartCoroutine(GameLoop());
        }
    }

    private IEnumerator GameLoop()
    {
        float t = 0;

        while (t < FriendSpawner.friendSpawnRate)
        {
            t += Time.deltaTime;
            yield return null;
        }

        FriendSpawner.SpawnAFriend();

        StartCoroutine(GameLoop());
    }


    // Helper methods
    public Vector3 GetRandomPosAtScreenEdge()
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
        return point;
    }
}
