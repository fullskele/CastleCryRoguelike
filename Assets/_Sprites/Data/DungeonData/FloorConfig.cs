using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorConfig_", menuName = "Data/FloorConfigData")]
public class FloorConfig : ScriptableObject {
    public int iterations = 10, walkLength = 10;
    public bool startRandomEachIteration = true;

    [SerializeField]
    public int enemySpawnCount = 30;
    public List<EnemySpawnData> enemySpawnDataList;

    //TODO
    [SerializeField]
    public int collectableSpawnCount = 5;
    public List<CollectableSpawnData> collectableSpawnDataList;

    //TODO
    [SerializeField]
    public int specialTileSpawnCount = 20;
    public List<SpecialTileSpawnData> specialTileSpawnDataList;

    //Yes these are awful. I'll fix it one day.
    //TODO: Make all of these generic functions RollForDataEntry, GetRarestDataEntry
    public Enemy RollForEnemy() {
        //pick from 1-100, add all enemies with that number or higher as their spawnchance, then chooses the rarest to return
        int randomChance = Random.Range(1, 101);
        List<EnemySpawnData> possibleEnemies = new List<EnemySpawnData>();
        for (int i = 0; i < enemySpawnDataList.Count; i++) {
            if (randomChance <= enemySpawnDataList[i].spawnChance) {
                possibleEnemies.Add(enemySpawnDataList[i]);
            }
        }
        if (possibleEnemies.Count > 0) {
            //find rarest of all rolled enemies
            Enemy chosenEnemy = GetRarestEnemy(possibleEnemies);
            return chosenEnemy;
        }
        return null;
    }

    //get rarest enemy of the ones rolled
    private Enemy GetRarestEnemy(List<EnemySpawnData> enemyList) {
        int rarestChance = 101;
        int rarestIndex = 404;
        for (int i = 0; i < enemyList.Count; i++) {
            if (enemySpawnDataList[i].spawnChance < rarestChance) {
                rarestChance = enemySpawnDataList[i].spawnChance;
                rarestIndex = i;
            }
        }
        return enemySpawnDataList[rarestIndex].enemyPrefab;
    }



    public Collectable RollForCollectable() {
        //pick from 1-100, add all collectables with that number or higher as their spawnchance, then chooses the rarest to return
        int randomChance = Random.Range(1, 101);
        List<CollectableSpawnData> possibleCollectables = new List<CollectableSpawnData>();
        for (int i = 0; i < collectableSpawnDataList.Count; i++) {
            if (randomChance <= collectableSpawnDataList[i].spawnChance) {
                possibleCollectables.Add(collectableSpawnDataList[i]);
            }
        }
        if (possibleCollectables.Count > 0) {
            //find rarest of all rolled collectables
            Collectable chosenCollectable = GetRarestCollectable(possibleCollectables);
            return chosenCollectable;
        }
        return null;
    }

    //get rarest enemy of the ones rolled
    private Collectable GetRarestCollectable(List<CollectableSpawnData> collectableList) {
        int rarestChance = 101;
        int rarestIndex = 404;
        for (int i = 0; i < collectableList.Count; i++) {
            if (collectableSpawnDataList[i].spawnChance < rarestChance) {
                rarestChance = collectableSpawnDataList[i].spawnChance;
                rarestIndex = i;
            }
        }
        return collectableSpawnDataList[rarestIndex].collectablePrefab;
    }



    public SpecialTile RollForSpecialTile() {
        //pick from 1-100, add all specialTile with that number or higher as their spawnchance, then chooses the rarest to return
        int randomChance = Random.Range(1, 101);
        List<SpecialTileSpawnData> possibleSpecialTile = new List<SpecialTileSpawnData>();
        for (int i = 0; i < specialTileSpawnDataList.Count; i++) {
            if (randomChance <= specialTileSpawnDataList[i].spawnChance) {
                possibleSpecialTile.Add(specialTileSpawnDataList[i]);
            }
        }
        if (possibleSpecialTile.Count > 0) {
            //find rarest of all rolled collectables
            SpecialTile chosenSpecialTile = GetRarestSpecialTile(possibleSpecialTile);
            return chosenSpecialTile;
        }
        return null;
    }

    //get rarest specialTile of the ones rolled
    private SpecialTile GetRarestSpecialTile(List<SpecialTileSpawnData> specialTileList) {
        int rarestChance = 101;
        int rarestIndex = 404;
        for (int i = 0; i < specialTileList.Count; i++) {
            if (specialTileSpawnDataList[i].spawnChance < rarestChance) {
                rarestChance = specialTileSpawnDataList[i].spawnChance;
                rarestIndex = i;
            }
        }
        return specialTileSpawnDataList[rarestIndex].specialTilePrefab;
    }
}
