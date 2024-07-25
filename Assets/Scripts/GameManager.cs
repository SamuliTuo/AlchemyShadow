using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EnemySpawner EnemySpawner { get; private set; }
    public EXPSpawner EXPSpawner { get; private set; }
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

        EnemySpawner = GetComponentInChildren<EnemySpawner>();
        EXPSpawner = GetComponentInChildren<EXPSpawner>();
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
        print("put game logic here");
        yield return null;
    }
}
