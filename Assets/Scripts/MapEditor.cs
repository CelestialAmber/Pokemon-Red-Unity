using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;
#endif

//Properties of tile, including collision, and others
public class Tile {
    public string path;
    public string tag;
    public bool isAnimated;
   
    public Tile(string s, string t, bool isanim){
        path = s;
        tag = t;
        isAnimated = isanim;
    }

}



public class MapData{
    public int mapNum;
    public long headerAddr;
    public int bank;
    public int tilesetNumber;
    public int originalHeight, originalWidth;
    public int height, width;
    public int mapPointer;
    public int textPointer;
    public int scriptPointer;
    public int connection;
    public int[,] connectionData;
    public int objectDataPointer;
    public int[] tileMap;
   
 

}

public class BlockIndex{
    public string[] indices;
}
public class TilesetIndex
{
    public BlockIndex[] indices = new BlockIndex[24];
}

#if UNITY_EDITOR
[ExecuteInEditMode]
public class MapEditor : MonoBehaviour
{
    public GameObject container;
    int currentTileIndex = 0;
    public GameObject template;
    public List<Tile> tilepool = new List<Tile>();
    public int currentMap;
    public TilesetIndex tSetIndexes;
    public MapData[] maps = new MapData[0];
    public static Color[] colors = {
        new Color(1, 1, 1, 1), //0xff
    new Color(.5625f, .5625f, .5625f, 1), //0x90
    new Color(.25f, .25f, .25f, 1), //0x40
    new Color(0,0,0,1) //0x00
};


    public void SpawnTile(Vector2 snappos,GameObject parent){
        //Spawn an editing GameObject that represents a tile.
        GameObject go;

            go = Instantiate(template, parent.transform, true);

        if (currentTileIndex == 31) currentTileIndex = 24;
                go.transform.localPosition = snappos;
                go.tag = tilepool[currentTileIndex].tag;
        go.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>(tilepool[currentTileIndex].path.Replace("Assets/Resources/", "").Replace(".png", ""))[0];
       EncounterInfo encounterInfo = go.GetComponent<EncounterTile>().info;
        switch(go.tag){
            case "TallGrass":
                go.transform.Translate(0, 0, -1);
                encounterInfo.isWater = false;
                break;
            case "Water":
                go.transform.Translate(0, 0, -1);
                encounterInfo.isWater = true;
                break;
            default:
                DestroyImmediate(go.GetComponent<EncounterTile>());
                break;
        }

                if (!tilepool[currentTileIndex].isAnimated)
                {
            DestroyImmediate(go.GetComponent<AnimatedTile>());
        }
                

    }
    public void SpawnMap(){
 
        //Spawn the map with the current index stored in currentMap
        GameObject maPContainer = new GameObject("Map " + currentMap);
        maPContainer.transform.SetParent(container.transform);
        maPContainer.transform.position = new Vector2(GameData.mapWidth/2, GameData.mapHeight/2);
        MapData map  = maps[currentMap];
        string indicelist;
        string i1, i2, i3, i4;
        //Create string variables to retrieve the values from the string;
        for (int i = 0; i < map.tileMap.Length; i++){
            //point any duplicate tileset number to the preexisting tileset
            int tilesetNumber = map.tilesetNumber;
            switch (tilesetNumber)
            {
                case 4:
                    tilesetNumber = 1;
                    break;
                case 6:
                    tilesetNumber = 2;
                    break;
                case 7:
                    tilesetNumber = 5;
                    break;
                case 10:
                    tilesetNumber = 9;
                    break;
                case 12:
                    tilesetNumber = 9;
                    break;
            }
            //is a tileset slot missing?
            if (tSetIndexes.indices[tilesetNumber].indices.Length >= map.tileMap[i]) {
               
                indicelist = tSetIndexes.indices[tilesetNumber].indices[map.tileMap[i]];
            }
          //set it as a null tile to not cause errors
            else indicelist = "0, 0, 0, 0";
            if (indicelist == "") indicelist = "0, 0, 0, 0";
            i1 = indicelist.Substring(0, indicelist.IndexOf(","));
            indicelist = indicelist.Substring(indicelist.IndexOf(" ") + 1);
             i2 = indicelist.Substring(0, indicelist.IndexOf(","));
            indicelist =  indicelist.Substring(indicelist.IndexOf(" ") + 1);
            i3 = indicelist.Substring(0, indicelist.IndexOf(","));
            indicelist = indicelist.Substring(indicelist.IndexOf(" ") + 1);
            i4 = indicelist.Substring(0);
            int[] indices = new int[] { int.Parse(i1), int.Parse(i2), int.Parse(i3), int.Parse(i4) };
            for (int j = 0; j < 4; j++){
                currentTileIndex = indices[j];
                //account for the map's dimensions to center it with the parent's transform
                SpawnTile(new Vector2(-map.width + 2*(i % map.width)  + j % 2, map.height -2 * (i / map.width) - Mathf.FloorToInt((float)j/4 + 0.51f)),maPContainer);
            }
        }


    }
   

  
   

    public void setCurrentMap(int index){
        currentMap = index;
       
    } 

}
[CustomEditor(typeof(MapEditor))]
public class MapTileEditor : Editor
{
    public int curmap = 0;
    //assume that the atlas's size will be what it is manually set to;
    public Vector2 atlasSize = new Vector2(1024, 512);


public override void OnInspectorGUI()
{
  DrawDefaultInspector();
  MapEditor me = (MapEditor)target;
        //iterate through the gameobjects in the map container, and parse their values;
        if(GUILayout.Button("Save Map to file")){
            GridTile[,] loadedTiles  = Serializer.Load2D<GridTile>(Application.streamingAssetsPath + "/map.txt");

            List<GameObject> foundtiles = new List<GameObject>();

            int index = 0;
            //look into each map for their tiles;
            foreach(Transform mapchild in me.container.transform ){
                foreach (Transform child in mapchild)
                {
                    foundtiles.Add(child.gameObject);
                }
            }
            index = 0;
            foreach(GameObject child in foundtiles){
                int x = (int)child.transform.position.x;
                int y = (int)child.transform.position.y;
                    loadedTiles[x,y] = new GridTile(null, null, null, false, 0, 0, "", false, false, false, 1, "",false,false);
                
                GridTile gridTile = loadedTiles[x, y];
                string tempname;
                tempname = child.GetComponent<SpriteRenderer>().sprite.name;
                if (tempname.Contains("_"))
                {
                    tempname = tempname.Substring(0, tempname.IndexOf('_'));
                }
                gridTile.mainSprite = tempname;
                gridTile.tag = child.tag;
                if (gridTile.tag.Contains("Wall")) loadedTiles[x, y].isWall = true;
                gridTile.isWarp = child.GetComponent<TileWarp>();
                gridTile.isAnimated = child.GetComponent<AnimatedTile>();
                gridTile.isInteractable = child.GetComponent<TileProperties>() ;
                gridTile.tileWarp = child.GetComponent<TileWarp>() ? child.GetComponent<TileWarp>().info : null;
                gridTile.tiledata = child.GetComponent<TileProperties>() ? child.GetComponent<TileProperties>().data : null;
                if (gridTile.isInteractable)
                {
                    gridTile.hasItem = gridTile.tiledata.hasItem;
                }
                if (gridTile.isAnimated){
                    tempname = AssetDatabase.GetAssetPath(child.GetComponent<SpriteRenderer>().sprite);
                    if (tempname.Contains("_"))
                    {
                        tempname = tempname.Substring(0, tempname.IndexOf('_'));
                    }
                    gridTile.frames = Resources.LoadAll<Sprite>(tempname.Replace("Assets/Resources/", "").Replace(".png","")).Length;
                   
                }

             
                EncounterTile encounterTile = null;
                if (child.GetComponent<EncounterTile>())
                {
                    encounterTile = child.GetComponent<EncounterTile>();
                }


                if (encounterTile != null)
                {
                    gridTile.encounterInfo = encounterTile.info;
                    switch(child.tag){
                        case "TallGrass":
                            gridTile.hasGrass = true;
                            break;
                        case "Water":
                            gridTile.isWater = true;
                            break;
                    }


                   }



                gridTile.posx = x;
                gridTile.posy = y;        
                index++;
            }
            Serializer.Save2D<GridTile>(Application.streamingAssetsPath + "/map.txt",loadedTiles);
           
        }
        if(GUILayout.Button("Save Empty map to file")){
            GridTile[,] nullMap = new GridTile[GameData.mapWidth, GameData.mapHeight];
            Serializer.Save2D<GridTile>(Application.streamingAssetsPath + "/map.txt",nullMap);
        }
        if (GUILayout.Button("Load editor data"))
        {
            me.tSetIndexes = Serializer.JSONtoObject<TilesetIndex>("blockIndexData.json");
            me.maps = Serializer.JSONtoObject<MapData[]>("romMapData.json");
            me.tilepool = Serializer.JSONtoObject<List<Tile>>("tilePoolData.json");
  
        }
         if (GUILayout.Button("Save tilepool"))
        {
          
           Serializer.objectToJSON("tilePoolData.json",me.tilepool);
  
        }
        if (GUILayout.Button("Add all tiles"))
        {
            me.tilepool.Clear();
            for(int i = 0; i < 871; i++){
                me.tilepool.Add(new Tile("","",false));
            }
             
            foreach (Texture2D tile in Resources.LoadAll<Texture2D>("interiortiles/"))
            {
                string path = AssetDatabase.GetAssetPath(tile);
                string tag = "Untagged";
                bool isAnimated = false;
                if (path.Contains("/Collision"))
                {
                    tag = "WallObject";
                }
                if (path.Contains("/TallGrass"))
                {
                    tag = "TallGrass";
                }
                if (path.Contains("/Ledges"))
                {
                    if (path.Contains("/Left"))
                    {
                        tag = "LedgeLeft";
                    }
                    if (path.Contains("/Right"))
                    {
                        tag = "LedgeRight";
                    }
                    if (path.Contains("/Down"))
                    {
                        tag = "LedgeDown";
                    }
                }
                if(path.Contains("/Water")){
                    tag = "Water";
                }
                string thisIndex = tile.name;
                int index = int.Parse(thisIndex.Replace("tile", ""));
                
                
                
                if (tile.width > 16 || tile.height > 16) isAnimated = true;
                me.tilepool[index] = new Tile(AssetDatabase.GetAssetPath(tile), tag, isAnimated);


            }
        }
        if(GUILayout.Button("Spawn map")){
            
           me.SpawnMap();
        }
       
      
       

     
  }


}
#endif
