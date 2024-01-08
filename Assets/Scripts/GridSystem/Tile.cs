using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ITiledObject
{
    public TileType TileType
    {
        get { return _tileType; }
        set { _tileType = value; }
    }

    public Vector2Int Coordinate { get; set; }
    private TileType _tileType;
}
