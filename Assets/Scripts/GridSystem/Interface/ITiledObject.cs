using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITiledObject 
{
   public TileType TileType { get; set;}
   public Vector2Int Coordinate { get; set; }
}
