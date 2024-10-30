using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] EnemyWave data;
    [SerializeField] Enemy ratPrefab;
    [SerializeField] float spawnSpread = 1;


    void Start()
    {
        Debug.LogWarning("TODO : Call the StartEnemyWaveFunction from the gameManager");
        StartEnemyWaves();
    }

    public void StartEnemyWaves()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        for (int waveIndex = 0; waveIndex < data.waves.Count; waveIndex++)
        {
            System.Random random = new System.Random();
            List<Transform> possibleSpawnPoints = spawnPoints.OrderBy(x => random.Next()).Take(data.waves[waveIndex].spawnPointsAmount).ToList();
            yield return new WaitForSeconds(data.waves[waveIndex].cooldownBeforeWave);

            for (int ratIndex = 0; ratIndex < data.waves[waveIndex].ratAmount; ratIndex++)
            {
                int rand = Random.Range(0, possibleSpawnPoints.Count);
                Debug.Log(rand);
                Transform targetSpawnPoint = possibleSpawnPoints[rand];
                Enemy ratInstance = Instantiate(ratPrefab, targetSpawnPoint.position, Quaternion.identity);
                Vector2 randomDir = Random.insideUnitCircle * spawnSpread;
                ratInstance.transform.position += new Vector3(randomDir.x, randomDir.y, 0);
            }
        }
    }
}
