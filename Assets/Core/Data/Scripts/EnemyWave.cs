using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public List<EnemySpawn> waves = new List<EnemySpawn>();
    public List<EnemySpawn> endRandomWaves = new List<EnemySpawn>();
}

[Serializable]
public class EnemySpawn
{
    public int ratAmount = 0;
    public int cooldownBeforeWave;
    public int spawnPointsAmount = 1;
}