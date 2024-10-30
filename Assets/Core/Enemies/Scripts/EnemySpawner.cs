using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] EnemyWave data;
    [SerializeField] Enemy ratPrefab;
    [SerializeField] float spawnSpread = 1;

    Coroutine enemySpawnRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void StartEnemyWaves()
    {
        enemySpawnRoutine = StartCoroutine(EnemySpawnRoutine());
    }

    void StopEnemyWaves()
    {
        if (enemySpawnRoutine != null)
        {
            StopCoroutine(enemySpawnRoutine);
            enemySpawnRoutine = null;
        }
    }

    IEnumerator EnemySpawnRoutine()
    {
        for (int waveIndex = 0; waveIndex < data.waves.Count; waveIndex++)
        {
            yield return StartCoroutine(WaveSpawn(data.waves[waveIndex]));
        }

        while (true)
        {
            yield return StartCoroutine(WaveSpawn(data.endRandomWaves[Random.Range(0, data.endRandomWaves.Count)]));
        }
    }

    IEnumerator WaveSpawn(EnemySpawn wave)
    {
        System.Random random = new System.Random();
        List<Transform> possibleSpawnPoints = spawnPoints.OrderBy(x => random.Next()).Take(wave.spawnPointsAmount).ToList();
        yield return new WaitForSeconds(wave.cooldownBeforeWave);

        for (int ratIndex = 0; ratIndex < wave.ratAmount; ratIndex++)
        {
            if (Enemy.enemyAmount < 150)
            {
                int rand = Random.Range(0, possibleSpawnPoints.Count);
                Transform targetSpawnPoint = possibleSpawnPoints[rand];
                Enemy ratInstance = Instantiate(ratPrefab, targetSpawnPoint.position, Quaternion.identity);
                Vector2 randomDir = Random.insideUnitCircle * spawnSpread;
                ratInstance.transform.position += new Vector3(randomDir.x, randomDir.y, 0);

            }
        }
    }
}
