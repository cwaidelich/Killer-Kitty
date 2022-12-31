using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public TagNachtCycles TagNachtCycles;
    [SerializeField] private List<Transform> spawnPositions;
    int EnemyCount;
    int MaxEnemyPerNight;
    const int MINIMUM_ENEMY_PER_DAY = 1;
    const float MINIMUM_SPAWN_TIME = 0.5f;

    private bool beingHandled = false;

    bool active = false;

    [SerializeField] private List<GameObject> enemies;

    void Start()
    {
        MaxEnemyPerNight = 0;
        EnemyCount = 0;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;


        if (TagNachtCycles.isDay)
        {
            EnemyCount = 0;
            return;
        }

        MaxEnemyPerNight = TagNachtCycles.nightCount * TagNachtCycles.nightCount + MINIMUM_ENEMY_PER_DAY;

        if (EnemyCount <= MaxEnemyPerNight && !beingHandled)
            StartCoroutine(EnemySpawing());
        
    }

    public void setActive(bool val)
    {
        if (val == active)
            return;

        active = val;
        if (!active)
            StopCycle();
    }
    private void StopCycle()
    {
        EnemyCount = 0;
        foreach (GameObject mouse in enemies)
        {
            GameObject.Destroy(mouse);
        }
        enemies.Clear();
    }

    IEnumerator EnemySpawing()
    {
        beingHandled = true;
        EnemyCount++;
        int nextSpawnPosition = Random.Range(0, spawnPositions.Count);

        GameObject newMouse = Instantiate(EnemyPrefab, spawnPositions[nextSpawnPosition].position, Quaternion.identity, transform);
        enemies.Add(newMouse);
        yield return new WaitForSeconds(Random.Range(MINIMUM_SPAWN_TIME, TagNachtCycles.dayTimeInSeconds));
        beingHandled = false;
    }

    public void clearMousesCounter()
    {
        foreach(GameObject mouse in enemies)
        {
            mouse.GetComponentInChildren<Enemy>().onNightEnd();
            GameObject.Destroy(mouse);
        }
        enemies.Clear();
    }

}
