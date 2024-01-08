using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Grid : MonoBehaviour
{
    public int XSize, YSize;
    [SerializeField] private GameObject _tilePrefab;
    public Tile[,] TilesInGrid;

    private void Awake()
    {
        GenerateTile();
    }

    public void GenerateTile()
    {
        TilesInGrid = new Tile[XSize, YSize];
        for (var i = 0; i < XSize; i++)
        {
            for (var j = 0; j < YSize; j++)
            {
                Vector3 pos = Vector3.left * i * 3 + Vector3.up * j * 3;
                var tile = MonoBehaviour.Instantiate(_tilePrefab, pos, Quaternion.identity).GetComponent<Tile>();
                tile.Coordinate = new Vector2Int(i, j);
                TilesInGrid[i, j] = tile;
            }
        }
        
        
        TileType[,] tileArray = new TileType[,]
        {
            { TileType.empty, TileType.empty, TileType.rigid },
            { TileType.eatable, TileType.x, TileType.y },
            { TileType.c, TileType.v, TileType.p }
            // Add more rows as needed
        };
        
        
        string filePath = Path.Combine(Application.dataPath, "tile_array.txt");
        WriteTileArrayToFile(tileArray, filePath);
    }


    void WriteTileArrayToFile(TileType[,] tileArray, string filePath)
    {
        // Create a StreamWriter to write to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Get the dimensions of the tile array
            int rows = tileArray.GetLength(0);
            int cols = tileArray.GetLength(1);

            // Write each tile type to the file
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Write the enum value as a string to the file
                    writer.Write((int)tileArray[i, j]);

                    // Add a separator (e.g., space or comma) if needed
                    writer.Write(" ");
                }

                // Move to the next line after each row
                writer.WriteLine();
            }
        }
    }
}