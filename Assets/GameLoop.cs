using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public List<SpawnerEvent> spawnEvents;
    public List<Vector2Int> friendSpawnEvents;

    public float gameTime = 0;
    public float friendIntervalTimer = 0;

    List<int> friendSpawnTimes = new List<int>();

    int startingTestFriends = 0;

    private void Start()
    {
        friendIntervalTimer = GameManager.Instance.FriendSpawner.friendSpawnRate - 0.5f;
        friendSpawnTimes = new List<int>();
        foreach (SpawnerEvent e in spawnEvents)
        {
            e.startTimeInSeconds = e.start_minutesAndSeconds.x * 60 + e.start_minutesAndSeconds.y;
            print(e.startTimeInSeconds);
        }
        foreach (Vector2Int time in friendSpawnEvents)
        {
            int _t = time.x * 60 + time.y;
            friendSpawnTimes.Add(_t);
        }
    }
    public void UpdateGame()
    {
        UpdateGameTimer();

        if (startingTestFriends > 0)
        {
            for (int i = 0; i < friendSpawnTimes.Count; i++)
            {
                GameManager.Instance.FriendSpawner.SpawnAFriend();
            }
        }
        // Start spawners
        for (int i = spawnEvents.Count - 1; i >= 0; i--)
        {
            if (gameTime >= spawnEvents[i].startTimeInSeconds)
            {
                print("STARTING SPAWNER!!!" + spawnEvents[i].prefab.name);
                StartCoroutine(SpawnerRunning(spawnEvents[i]));
                spawnEvents.RemoveAt(i);
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
        
        // update time
        gameTime += Time.deltaTime;
    }


    // Sun timer
    public GameObject timerPlanet;
    public GameObject timerSun;
    Vector3 timerPlanetStartPos;
    Vector3 timerSunPos;
    void UpdateGameTimer()
    {
        float perc = gameTime / 900;
        Vector3.Lerp(timerPlanetStartPos, timerSunPos, perc);
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
