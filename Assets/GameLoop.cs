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

    private void Start()
    {
        friendIntervalTimer = GameManager.Instance.FriendSpawner.friendSpawnRate - 0.5f;
        foreach (SpawnerEvent e in spawnEvents)
        {
            e.startTimeInSeconds = e.start_minutesAndSeconds.x * 60 + e.start_minutesAndSeconds.y;
            e.duration = e.end_minutesAndSeconds.x * 60 + e.end_minutesAndSeconds.y - e.startTimeInSeconds;
        }
        friendSpawnTimes = new List<int>();
        foreach (Vector2Int time in friendSpawnEvents)
        {
            int _t = time.x * 60 + time.y;
            friendSpawnTimes.Add(_t);
            print("added a timer : " + _t);
        }
    }
    public void UpdateGame()
    {
        // Start spawners
        for (int i = spawnEvents.Count - 1; i >= 0; i--)
        {
            if (gameTime >= spawnEvents[i].startTimeInSeconds)
            {
                print("STARTING SPAWNER!!!" + spawnEvents[i].spawnThis.name);
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

    IEnumerator SpawnerRunning(SpawnerEvent spawner)
    {
        while (gameTime <= gameTime + spawner.duration)
        {
            var pos = GameManager.Instance.GetRandomPosAtScreenEdge();
            pos.z = 0;
            Instantiate(spawner.spawnThis, pos, Quaternion.identity);
            yield return new WaitForSeconds(spawner.spawnInterval);
        }
    }
}








[System.Serializable]
public class SpawnerEvent
{
    [Header("Start time:")]
    public Vector2Int start_minutesAndSeconds;

    [Header("Ending time:")]
    public Vector2Int end_minutesAndSeconds;

    [Space(20)]
    public GameObject spawnThis;
    public float spawnInterval;

    [HideInInspector] public float startTimeInSeconds;
    [HideInInspector] public float duration;

    public SpawnerEvent(Vector2Int start_minutesAndSeconds, Vector2Int end_minutesAndSeconds, GameObject spawnThis, float spawnInterval)
    {
        this.start_minutesAndSeconds = start_minutesAndSeconds;
        this.end_minutesAndSeconds = end_minutesAndSeconds;
        this.spawnThis = spawnThis;
        this.spawnInterval = spawnInterval;
        startTimeInSeconds = start_minutesAndSeconds.x * 6 + start_minutesAndSeconds.y;
        duration = end_minutesAndSeconds.x * 60 + end_minutesAndSeconds.y - startTimeInSeconds;
    }
}
