using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array of tile prefabs
    public int gridSizeX = 10;
    public int gridSizeY = 10;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 spawnPosition = new Vector3(x, 0, y); // Adjust the y-coordinate based on your game's needs
                Instantiate(tilePrefabs[0], spawnPosition, Quaternion.identity, transform); // Instantiate the default tile
            }
        }
    }
}