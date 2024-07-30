using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public Vector2 startTime = Vector2.zero;
    public GameObject bossPrefab;
    public Transform bossSpawnZone;

    public List<SpawnerEvent> enemySpawnEvents;
    [Space(20)]
    public int startingTestFriends = 0;
    public List<Vector2Int> friendSpawnEvents;

    public float gameTime = 0;
    public float friendIntervalTimer = 0;

    List<int> friendSpawnTimes = new List<int>();

    public GameObject timerPlanet;
    public GameObject timerSun;
    public GameObject timerEclipse;
    Vector3 timerPlanetStartPos;
    bool bossFight = false;

    
    private void Start()
    {
        timerPlanetStartPos = timerPlanet.transform.position;
        friendIntervalTimer = GameManager.Instance.FriendSpawner.friendSpawnRate - 0.5f;
        friendSpawnTimes = new List<int>();
        foreach (SpawnerEvent e in enemySpawnEvents)
        {
            e.startTimeInSeconds = e.start_minutesAndSeconds.x * 60 + e.start_minutesAndSeconds.y;
        }
        foreach (Vector2Int time in friendSpawnEvents)
        {
            int _t = time.x * 60 + time.y;
            friendSpawnTimes.Add(_t);
        }

        SetTestStartTime();
    }


    public void UpdateGame()
    {
        if (bossFight)
        {
            UpdateBossFight();
            return;
        }

        UpdateGameTimer();

        if (startingTestFriends > 0)
        {
            for (int i = 0; i < startingTestFriends; i++)
            {
                GameManager.Instance.FriendSpawner.SpawnAFriend();
            }
            startingTestFriends = 0;
        }

        // Start enemy spawners
        for (int i = enemySpawnEvents.Count - 1; i >= 0; i--)
        {
            if (gameTime >= enemySpawnEvents[i].startTimeInSeconds)
            {
                StartCoroutine(SpawnerRunning(enemySpawnEvents[i]));
                enemySpawnEvents.RemoveAt(i);
            }
        }

        // Spawn friends
        for (int i = friendSpawnTimes.Count - 1; i >= 0; i--)
        {
            if (gameTime >= friendSpawnTimes[i])
            {
                GameManager.Instance.FriendSpawner.SpawnAFriend();
                friendSpawnTimes.RemoveAt(i);
            }
        }


        if (gameTime >= 900)
        {
            StartBossFight();
            return;
        }
        
        
        // update time
        gameTime += Time.deltaTime;
    }


    void StartBossFight()
    {
        bossFight = true;
    }

    enum bossPhases { NONE, SPAWNING, PHASE1, PHASE2, PHASE3};
    private bossPhases phase = bossPhases.NONE;
    float bossT = 0;
    public float bossSpawnTime = 2;
    public GameObject activeBoss;

    void UpdateBossFight()
    {
        if (phase == bossPhases.NONE)
        {
            //spawn the boss -phase
            bossSpawnZone.gameObject.SetActive(true);
            phase = bossPhases.SPAWNING;
            bossT = 0;
        }
        else if (phase == bossPhases.SPAWNING)
        {
            bossT += Time.deltaTime;
            if (bossT >= bossSpawnTime)
            {
                activeBoss = Instantiate(bossPrefab, bossSpawnZone.position, Quaternion.identity);
                phase = bossPhases.PHASE1;
                bossT = 0;
            }
        }
        else if (phase == bossPhases.PHASE1)
        {
            // tsekkaa bossin HP, jos alle 70% aktivoi phase 2
        }
        else if (phase == bossPhases.PHASE2)
        {
            // 40% aktivoi vika phase
        }
        else if (phase == bossPhases.PHASE3)
        {
            // vika feissi oh nooooo
        }
        else
        {
            // aktivoi ending jeee!
        }

        

        

        // put active a red mark in the middle of map
        // (activate boss-tracker for player)

        // wait some seconds

        // spawn the boss in middle of marker

        // go to 1st phase of fight 
        // ...
    }


    void SetTestStartTime()
    {
        gameTime = startTime.x * 60 + startTime.y;
        for (int i = 0; i < enemySpawnEvents.Count; i++)
        {
            if (enemySpawnEvents[i].startTimeInSeconds < gameTime)
            {
                enemySpawnEvents.RemoveAt(i);
            }
        }
    }


    // Sun timer
    void UpdateGameTimer()
    {
        float perc = gameTime / 900;
        timerPlanet.transform.position = Vector3.Lerp(timerPlanetStartPos, timerSun.transform.position, perc);
    }

    void BossDoneAnimateSolsticeAway()
    {
        // siirrä jollain nopeella tweenillä auringonpimennys pois kun bossi on voitettu ennen loppuscreeniä.
    }




    IEnumerator SpawnerRunning(SpawnerEvent spawner)
    {
        float endTime = gameTime + spawner.duration;
        while (gameTime <= endTime)
        {
            var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
            pos.z = 0;
            Instantiate(spawner.prefab, pos, Quaternion.identity);

            yield return new WaitForSeconds(spawner.spawnInterval);
        }
    }
}








[System.Serializable]
public class SpawnerEvent
{
    public string name;
    public GameObject prefab;
    [Header("Start time:")]
    public Vector2Int start_minutesAndSeconds;

    [Header("Duration in seconds:")]
    public float duration;
    public float spawnInterval;

    [Space(20)]
    [Tooltip("spawn immediately and then how often??")]

    [HideInInspector] public float startTimeInSeconds;

    public SpawnerEvent(string name, Vector2Int start_minutesAndSeconds, float duration, GameObject spawnThis, float spawnInterval)
    {
        this.name = name;
        this.start_minutesAndSeconds = start_minutesAndSeconds;
        this.duration = duration;
        this.prefab = spawnThis;
        this.spawnInterval = spawnInterval;
        startTimeInSeconds = start_minutesAndSeconds.x * 6 + start_minutesAndSeconds.y;
    }
}
