using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class GridEditorWindow : EditorWindow
{
    private TileData tileData;
    private int selectedTileIndex = 0;
    private TileType _selectedTileType = TileType.empty;
    private bool showGridSettings = true;
    private GUIStyle selectedButtonStyle;
    private ITiledObject [,] _tiles;
    [Range(2, 100)]
    private int _xSize,   _ySize;
    private int _selectedXsize, _selectedYsize, _SelectedCellSize;
    private int _cellSize;
    private Vector2 _scrollPosition;
    private TextAsset selectedTextAsset;
    [MenuItem("Window/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnEnable()
    {
        tileData = Resources.Load<TileData>(nameof(TileData));
        selectedButtonStyle = new GUIStyle(GUIStyle.none);
        selectedButtonStyle.normal.textColor = Color.green;
        selectedButtonStyle.border = new RectOffset(2, 2, 2, 2);
    }

    private void OnGUI()
    {   
        DrawToolbar();
        DrawGridEditor();
    }

    private void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label("Grid Editor", EditorStyles.boldLabel);

        // Add more toolbar buttons or options as needed
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
    }

    private void DrawGridEditor()
    {
        GUILayout.BeginHorizontal();

        // Left side: Tile Selection
        DrawTileSelection();
        // Vertical separator
        GUILayout.Box("", GUILayout.Width(2), GUILayout.ExpandHeight(true));
        DrawGridSettingsAndVisualization();
        GUILayout.EndHorizontal();
       
         
    }

    private void DrawTileSelection()
    {
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.3f));
        GUILayout.Label("Tile Selection", EditorStyles.boldLabel);
        selectedTextAsset = (TextAsset)EditorGUILayout.ObjectField("Text Asset", selectedTextAsset, typeof(TextAsset), false);

        // Button to load the selected text asset
        if (GUILayout.Button("Load Json Asset to edit"))
        {
            if (selectedTextAsset != null)
            {
                LoadTextAsset(selectedTextAsset);
            }
            else
            {
                Debug.LogWarning("No Json Asset selected.");
            }
        }
        _cellSize = EditorGUILayout.IntField("Enter Cell Size", _cellSize);
        _xSize =  EditorGUILayout.IntField("Enter Grid X Size", _xSize);
        _ySize = EditorGUILayout.IntField("Enter Grid Y Size", _ySize);
        _xSize = Mathf.Clamp(_xSize, 2, 30);
        _ySize = Mathf.Clamp(_ySize, 2, 30);
        _cellSize = Mathf.Clamp(_cellSize, 15, 60);
        if (GUILayout.Button("Generate Grid"))
        {
            _selectedXsize = _xSize;
            _selectedYsize = _ySize;
            _SelectedCellSize = _cellSize;
            Debug.Log("Xsize = " + _xSize + "Ysize =" + _ySize);
          
            _tiles = new ITiledObject [_selectedXsize, _selectedYsize];
            for (var i = 0; i < _selectedXsize; i++)
            {
                for (var j = 0; j < _selectedYsize; j++)
                {
                    _tiles[i, j] = new Tile();
                    _tiles[i, j].Coordinate = new Vector2Int(i, j);
                }
            }
        }

        // Check if tileData is not null
        if (tileData != null)
        {
            // Get the TileData ScriptableObject
            tileData = EditorGUILayout.ObjectField("Tile Data", tileData, typeof(TileData), false) as TileData;

            if (tileData != null && tileData.tiles != null)
            {
                var length = tileData.tiles.Length;
                var rows = 3;
                var columns = Mathf.CeilToInt((float)length / (float)3);
                var order = 0;

                for (int i = 0; i < rows; i++)
                {
                    GUILayout.BeginHorizontal();

                    // Determine the number of columns for the current row
                    var currentColumns = Mathf.Min(columns, length - i * columns);

                    for (var j = 0; j < currentColumns; j++)
                    {
                        order = i * columns + j;

                        if (order >= length)
                        {
                            Debug.Log("" + i + "-" + j);
                            break;
                        }

                        DrawTilePreview(order);
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
            }
        }
        DrawExportButton();
        GUILayout.EndVertical();
    }


    private void DrawTilePreview(int index)
    {
        bool isSelected = (index == selectedTileIndex);
        // Set the style based on selection
        GUIStyle buttonStyle = isSelected ? selectedButtonStyle : GUI.skin.button;
        // Display the Texture in GUILayout.Button with the selected style
        if (GUILayout.Button(GeneratePrefabPreview(tileData.tiles[index].tilePrefab), buttonStyle, GUILayout.Width(50), GUILayout.Height(50)))
        {
            selectedTileIndex = index;
            _selectedTileType = tileData.tiles[index].TileType;
        }
    }


    private Texture2D GeneratePrefabPreview(GameObject prefab)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        // Hide the instance in the scene view
        instance.hideFlags = HideFlags.HideAndDontSave;
        // Get the asset path of the prefab
        string prefabPath = AssetDatabase.GetAssetPath(prefab);
        // Load the prefab contents
        Object prefabAsset = PrefabUtility.LoadPrefabContents(prefabPath);
        // Get the preview texture
        Texture2D previewTexture = AssetPreview.GetAssetPreview(prefabAsset);
        // Clean up the loaded contents
        PrefabUtility.UnloadPrefabContents((GameObject)prefabAsset);
        DestroyImmediate(instance);
        return previewTexture;
    }


    private string[] GetTileNames()
    {
        if (tileData != null)
        {
            string[] tileNames = new string[tileData.tiles.Length];
            for (int i = 0; i < tileData.tiles.Length; i++)
            {
                tileNames[i] = tileData.tiles[i].tileName;
            }

            return tileNames;
        }
        return new string[0];
    }

    private void DrawGridSettingsAndVisualization()
    {
        GUILayout.BeginVertical();

        // Top: Toolbar for Grid Settings
        DrawGridSettingsToolbar();

        // Middle: Grid Visualization
        DrawGridVisualization();

        GUILayout.EndVertical();
    }

    private void DrawGridSettingsToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        // Add more toolbar buttons or options for grid settings
        showGridSettings = GUILayout.Toggle(showGridSettings, "Show Grid ", EditorStyles.toolbarButton);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        // Display grid settings based on the showGridSettings flag
        if (showGridSettings)
        {
            // Add your grid settings GUI elements here
        }
    }
    private void DrawGridVisualization()
    {  
        _scrollPosition = GUILayout.BeginScrollView(
            _scrollPosition);

        GUILayout.Label("Grid Visualization", EditorStyles.boldLabel);

        var buttonWidth = GUILayout.Width(_cellSize);
        var buttonHeight = GUILayout.Height(_cellSize);
        
        for (var i = 0; i < _selectedXsize; i++)
        {
            GUILayout.BeginHorizontal();

            for (var j = 0; j < _selectedYsize; j++)
            {
                ITiledObject iTiledObject = _tiles[i, j];
                Texture2D texture = iTiledObject != null && iTiledObject.TileType != TileType.empty
                    ? GeneratePrefabPreview(tileData.tiles.FirstOrDefault(x => x.TileType == iTiledObject.TileType).tilePrefab)
                    : null;
                if (GUILayout.Button(texture ,  GUI.skin.box, buttonWidth, buttonHeight))
                {
                    // Check if _tiles[i, j] is null before accessing properties
                    if (_tiles[i, j] != null)
                    {
                        _tiles[i, j].TileType = _selectedTileType;
                    }
                    else
                    {
                        // Create a new instance of the object if it doesn't exist
                        _tiles[i, j] = new Tile(); // Replace YourTiledObject with the actual type
                        _tiles[i, j].TileType = _selectedTileType;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    private void DrawExportButton()
    {
        if (GUILayout.Button("Export to Json"))
        {
            var fileName = "Test.txt";
            SaveJsonToFile(Serialize2DArray(_tiles) , "Assets/Resources/test.json");
        }
    }
    
    static string Serialize2DArray(ITiledObject[,] tiledArray)
    {
        // Create a jagged array to store serialized objects
        var serializedArray = new object[tiledArray.GetLength(0)][];

        for (int i = 0; i < tiledArray.GetLength(0); i++)
        {
            serializedArray[i] = new object[tiledArray.GetLength(1)];

            for (int j = 0; j < tiledArray.GetLength(1); j++)
            {
                // Serialize each ITiledObject to a JObject
                ITiledObject tiledObject = tiledArray[i, j];
                var serializedObject = new
                {
                    TileType = tiledObject.TileType,
                    Coordinate = new { x = tiledObject.Coordinate.x, y = tiledObject.Coordinate.y }
                };

                serializedArray[i][j] = serializedObject;
            }
        }

        // Convert the jagged array to a JSON string
        return JsonConvert.SerializeObject(serializedArray);
    }

    static void SaveJsonToFile(string json, string filePath)
    {
        // Save the JSON string to the specified file path
        File.WriteAllText(filePath, json);
        Debug.Log($"JSON saved to {filePath}");
    }
    private  ITiledObject[,] ConvertListTo2DArray(List<List<TileInfo>> list)
    {
        int rows = list.Count;
        int cols = list.Max(row => row.Count);
        _selectedXsize = rows;
        _selectedYsize = cols;
        _tiles = new ITiledObject[_selectedXsize, _selectedYsize];
        ITiledObject[,] array = new ITiledObject[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                array[i, j] = new Tile();
                array[i , j].TileType = list[i][j].TileType;
                array[i, j].Coordinate = new Vector2Int(i, j);
            }
        }

        return array;
    }
    
    
    private void LoadTextAsset(TextAsset textAsset)
    {
        try
        {
            string content = AssetDatabase.GetAssetPath(textAsset);
            string json = File.ReadAllText(content);
            List<List<TileInfo>> tiledObjectsList = JsonConvert.DeserializeObject<List<List<TileInfo>>>(json);
            _tiles = ConvertListTo2DArray(tiledObjectsList);
            foreach (var item in _tiles)
            {
                Debug.Log(item.TileType);
            }
        }
        catch (Exception ex)
        {
           Debug.Log($"Error reading or deserializing JSON file: {ex.Message}");
        }
    }

}

public class TileInfo
{
    public TileType TileType;
    public Vector2Int Coordinate;
}
