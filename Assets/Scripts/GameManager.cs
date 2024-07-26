using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EnemySpawner EnemySpawner { get; private set; }
    public FriendSpawner FriendSpawner { get; private set; }
    public PartyManager PartyManager { get; private set; }
    public EXPSpawner EXPSpawner { get; private set; }
    private Coroutine gameLoop = null;
    private Camera cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnemySpawner = GetComponentInChildren<EnemySpawner>();
        FriendSpawner = GetComponentInChildren<FriendSpawner>();
        PartyManager = GetComponentInChildren<PartyManager>();
        EXPSpawner = GetComponentInChildren<EXPSpawner>();
        cam = Camera.main;
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
            randomCoordinates = new Vector2(Random.Range(0.0f, Screen.width), Random.Range(0, 2));
        }
        else
        {
            randomCoordinates = new Vector2(Random.Range(0, 2), Random.Range(0.0f, Screen.height));
        }
        var point = cam.ScreenToWorldPoint(new Vector3(randomCoordinates.x, randomCoordinates.y, cam.nearClipPlane));
        return point;
    }
}
