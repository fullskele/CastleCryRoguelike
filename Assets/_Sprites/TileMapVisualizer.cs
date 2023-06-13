using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapVisualizer : MonoBehaviour {
    
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap, corridorTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownLeft, wallDiagonalCornerDownRight, wallDiagonalCornerUpLeft, wallDiagonalCornerUpRight;



    //TODO: Create method that also includes hallway positions
    public List<Vector2> FindFloorPositions() {
        List<Vector2> floorPositions = new List<Vector2>();
        floorTilemap.CompressBounds();
        var bounds = floorTilemap.cellBounds;

        // loop over the bounds (from min to max) on both axes
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                var cellPosition = new Vector3Int(x, y, 0);

                // get the sprite and tile object at the specified location
                var sprite = floorTilemap.GetSprite(cellPosition);
                var tile = floorTilemap.GetTile(cellPosition);

                // this is a sanity check that i've included to ensure we're only
                // looking at populated tiles. you can change this up!
                if (tile == null && sprite == null) {
                    continue;
                }

                // add to list to return
                //Debug.Log(x + ", " + y);
                floorPositions.Add(new Vector2(x,y));
            }
        }
        return floorPositions;
    }

    public List<Vector2> FindFloorAndCorridorPositions() {
        List<Vector2> corridorPositions = new List<Vector2>();
        corridorTilemap.CompressBounds();
        var bounds = corridorTilemap.cellBounds;

        // loop over the bounds (from min to max) on both axes
        for (int x = bounds.min.x; x < bounds.max.x; x++) {
            for (int y = bounds.min.y; y < bounds.max.y; y++) {
                var cellPosition = new Vector3Int(x, y, 0);

                // get the sprite and tile object at the specified location
                var sprite = corridorTilemap.GetSprite(cellPosition);
                var tile = corridorTilemap.GetTile(cellPosition);

                // this is a sanity check that i've included to ensure we're only
                // looking at populated tiles. you can change this up!
                if (tile == null && sprite == null) {
                    continue;
                }

                // add to list to return
                //Debug.Log(x + ", " + y);
                corridorPositions.Add(new Vector2(x, y));
            }
        }

        List<Vector2> floorPositions = FindFloorPositions();
        foreach (var pos in floorPositions) {
            corridorPositions.Add(pos);
        }
        return corridorPositions;
    }

    //takes a generic collection of Vector2Int positions
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) {
        PaintFloorTiles(floorPositions, floorTilemap, floorTile);
    }

    public void PaintCorridorTiles(IEnumerable<Vector2Int> corridorPositions) {
        PaintCorridorTiles(corridorPositions, corridorTilemap, floorTile);
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType) {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt)) {
            tile = wallTop;
        } else if (WallTypesHelper.wallSideRight.Contains(typeAsInt)) {
            tile = wallSideRight;
        } else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt)) {
            tile = wallSideLeft;
        } else if (WallTypesHelper.wallBottm.Contains(typeAsInt)) {
            tile = wallBottom;
        } else if (WallTypesHelper.wallFull.Contains(typeAsInt)) {
            tile = wallFull;
        }

        if (tile != null) 
            PaintSingleTile(wallTilemap, tile, position);
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile) {
        foreach (var pos in positions) {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    private void PaintCorridorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile) {
        foreach (var pos in positions) {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position) {
        var tilePos = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePos, tile);
    }

    public void Clear() {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        corridorTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int pos, string binaryType) {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt)) {
            tile = wallInnerCornerDownLeft;
        } else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt)) {
            tile = wallInnerCornerDownRight;
        } else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt)) {
            tile = wallDiagonalCornerDownLeft;
        } else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt)) {
            tile = wallDiagonalCornerDownRight;
        } else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt)) {
            tile = wallDiagonalCornerUpLeft;
        } else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt)) {
            tile = wallDiagonalCornerUpRight;
        } else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt)) {
            tile = wallFull;
        } else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt)) {
            tile = wallBottom;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }
}
