using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnTableEntry_", menuName = "ESD/EnemySpawnTableData")]
public class EnemySpawnTableData : ScriptableObject { 
    public int spawnWeight = 100;
    public Enemy enemyPrefab;
    public string[] tags;

}
