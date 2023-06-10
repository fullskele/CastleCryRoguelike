using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorConfig_", menuName = "Data/FloorConfigData")]
public class FloorConfig : ScriptableObject {
    public int iterations = 10, walkLength = 10;
    public bool startRandomEachIteration = true;
    public List<EnemySpawnData> enemySpawnDataList;

}
