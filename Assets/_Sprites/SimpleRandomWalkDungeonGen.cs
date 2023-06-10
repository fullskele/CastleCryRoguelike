using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGen : AbstractDungeonGen {

    [SerializeField]
    protected FloorConfig config;
    [HideInInspector]
    public bool isGeneratingLevel;
    [HideInInspector]
    public bool isEnemyClearing;
    [HideInInspector]

    protected override void RunProceduralGen() {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(config, startPos);
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(FloorConfig parameters, Vector2Int pos) {
        var currentPos = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++) {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, parameters.walkLength);
            floorPositions.UnionWith(path); //copy without duplicates

            if (parameters.startRandomEachIteration)
                currentPos = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
