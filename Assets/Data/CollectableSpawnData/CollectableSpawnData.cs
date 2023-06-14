using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableSpawnData_", menuName = "Data/CollectableSpawnData")]
public class CollectableSpawnData : ScriptableObject {
    public Collectable collectablePrefab;
    public int spawnChance = 100;
}
