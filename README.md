# Grid LevelEditor
  This is a Repo that i created for gird based level editing .
# Usage :
  you can open Grid Editor via Window => Grid Editor.

  .Grid Editor is capable of creating visualized grid with selected size and allows editing visualized grid.
  
  .Grid Editor can both export your edited levels into json file and enables loading saved json files to rework on previous files.

  .You can arrange your specific types via TileType enum : 
  
    public enum TileType 
    {
    
        empty = 0,
        eatable = 1 ,
        rigid = 2,
        x = 3,
        y=4,
        z=5,
        c = 6,
        v=7,
        b=8,
        n=9,
        o=10,
        p=11,
        
   }

.Also you can edit visual representation of your Types via TileData Scriptable Object.
.Icon of your Tile Types is created by getting preview of your prefabs which shall be attached to slot inside Scriptable Object.
.It is going to updated in next version of Grid Level Editor to enable specific icons for each prefab besides procedurally generated previews.
<img width="820" alt="Screenshot 2024-01-08 at 15 08 58" src="https://github.com/zubeyryesilbas/Grid_Level_Editor/assets/50784242/792621ca-31e2-44e6-8dc4-b8b45fb789a8">
# It enables easy grid based level editing and storing as a Json.
  
<img width="1164" alt="Screenshot 2024-01-08 at 14 43 03" src="https://github.com/zubeyryesilbas/Grid_Level_Editor/assets/50784242/d014dceb-791f-4a9b-8ff6-8f7b9644aadb">

# There will be a lot of improvements on further versions.
