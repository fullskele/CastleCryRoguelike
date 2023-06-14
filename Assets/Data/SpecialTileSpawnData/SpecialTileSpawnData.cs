using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialTileSpawnData_", menuName = "Data/SpecialTileSpawnData")]
public class SpecialTileSpawnData : ScriptableObject {
    public SpecialTile specialTilePrefab;
    public int spawnChance = 100;
}
