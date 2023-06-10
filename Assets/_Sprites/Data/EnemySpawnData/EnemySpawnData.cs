using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData_", menuName = "Data/EnemySpawnData")]
public class EnemySpawnData : ScriptableObject {
    public Enemy enemyPrefab;
    public int spawnChance = 100;
}
