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
    public int lootSpawnCount = 5;
    //public List<LootSpawnData> lootSpawnDataList;

    //TODO
    [SerializeField]
    public int specialTileSpawnCount = 20;
    //public List<SpecialTileSpawnData> specialTileSpawnDataList;


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

}
