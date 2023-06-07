using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGen : SimpleRandomWalkDungeonGen {

    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;

    [SerializeField]
    public int enemySpawnLimit = 4;
    [SerializeField]
    public int enemiesRemaining;
    [SerializeField]
    private GameObject player;

    //TODO: make this a table of enemyprefabs, minfloorlevel, spawnchance
    [SerializeField]
    private GameObject enemyToSpawn;

    public void UpdateEnemyCount(int change) {
        //if percent of enemies left is less than 10%, make new level
        enemiesRemaining += change;
        if ((float)enemiesRemaining / (float)enemySpawnLimit <= .10) {
            RunProceduralGen();
        }
    }

    protected override void RunProceduralGen() {
        //TODO: hook into floorLevel and if % 3 = 0, spawn a boss room instead
        tileMapVisualizer.Clear();
        CreateRooms();
    }

    private void PlacePlayerAtStart(List<Vector2Int> roomCenters) {
        var randomRoomCenter = roomCenters[Random.Range(0, roomCenters.Count-1)];
        player.transform.position = new Vector3((float)(randomRoomCenter.x+.5), (float)(randomRoomCenter.y+.5), 0);
    }

    //TODO: Implement hallway size functions!

    private void CreateRooms() {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int
            (dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        isGeneratingLevel = true;
        isEnemyClearing = true;

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms) {
            floor = CreateRoomsRandomly(roomsList);
        } else {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList) {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        PlacePlayerAtStart(roomCenters);

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

        //make separate paintCorridorTiles
        tileMapVisualizer.PaintCorridorTiles(corridors);
        tileMapVisualizer.PaintFloorTiles(floor);


        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies) {
            enemy.DeleteSelf();
        }

        spawnEnemies(tileMapVisualizer.FindAllFloorPositions(), enemySpawnLimit);

        enemiesRemaining = enemySpawnLimit;
        //this might not work long-term
        HashSet<Vector2Int> allFloors = floor;
        allFloors.UnionWith(corridors);

        //floor.UnionWith(corridors);
        WallGenerator.CreateWalls(allFloors, tileMapVisualizer);




        //wait 1 frame for new enemies near player to delete themselves, then stop
        StartCoroutine(StopGeneratingLevel());
    }
    //TODO is this function necessary? Find way without 1f wait?
    IEnumerator StopGeneratingLevel() {
        yield return null;
        isGeneratingLevel = false;
    }

    private void spawnEnemies(List<Vector2> floorTiles, int enemySpawnCount) {
        while (enemySpawnCount > 0) {
            Vector2 randomTile = floorTiles[Random.Range(0, floorTiles.Count - 1)];
            Vector2 randomPos = new Vector2((float)(randomTile.x+.5), (float)(randomTile.y+.5));
            Instantiate(enemyToSpawn, randomPos, Quaternion.identity);
            //prevent duplicate locations
            floorTiles.Remove(randomPos);
            enemySpawnCount -= 1;
        }
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList) {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++) {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParams, roomCenter);
            foreach (var pos in roomFloor) {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) 
                    && pos.y >= (roomBounds.yMin - offset) && pos.y <= (roomBounds.yMax - offset)) {

                    floor.Add(pos);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters) {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0) {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination) {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var pos = currentRoomCenter;
        corridor.Add(pos);

        while (pos.y != destination.y) {

            if (destination.y > pos.y) {
                pos += Vector2Int.up;

            } else if (destination.y < pos.y) {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }
        while (pos.x != destination.x) {

            if (destination.x > pos.x) {
                pos += Vector2Int.right;

            } else if (destination.x < pos.x) {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters) {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var pos in roomCenters) {
            float currentDist = Vector2.Distance(pos, currentRoomCenter);

            if (currentDist < distance) {
                distance = currentDist;
                closest = pos;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList) {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList) {
            for (int col = offset; col < room.size.x - offset; col++) {
                for (int row = offset; row < room.size.y - offset; row++) {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
