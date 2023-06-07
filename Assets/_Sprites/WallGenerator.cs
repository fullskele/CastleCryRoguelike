using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator {
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer) {

        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        CreateBasicWalls(tileMapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tileMapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions) {
        foreach (var pos in cornerWallPositions) {
            string neighborsBinaryType = "";

            foreach (var direction in Direction2D.eightDirectionsList) {
                var neighborPos = pos + direction;
                if (floorPositions.Contains(neighborPos)) {
                    neighborsBinaryType += "1";
                } else {
                    neighborsBinaryType += "0";
                }
            }

            tileMapVisualizer.PaintSingleCornerWall(pos, neighborsBinaryType);
        }
    }

    private static void CreateBasicWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions) {
        foreach (var pos in basicWallPositions) {
            string neighborBinaryType = "";

            foreach (var direction in Direction2D.cardinalDirectionsList) {
                var neighborPos = pos + direction;
                if (floorPositions.Contains(neighborPos)) {
                    neighborBinaryType += "1";
                } else {
                    neighborBinaryType += "0";
                }
            }

            tileMapVisualizer.PaintSingleBasicWall(pos, neighborBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList) {

        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var pos in floorPositions) {
            foreach (var direction in directionList) {
                var neighborPos = pos + direction;
                if (floorPositions.Contains(neighborPos) == false)
                    wallPositions.Add(neighborPos);
            }
        }
        return wallPositions;
    }
}
