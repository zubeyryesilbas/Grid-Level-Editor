using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Custom/Tile Data")]
public class TileData : ScriptableObject
{
    [System.Serializable]
    public class TileInfo
    {
        public string tileName;
        public GameObject tilePrefab;
        public TileType TileType;
        public Sprite tilePreview;
    }

    public TileInfo[] tiles;
}