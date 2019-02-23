using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
#endif
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
                encounterInfo.isWater = false;
                break;
            case "Water":
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
        GameObject mapContainer = new GameObject("Map " + currentMap);
        mapContainer.transform.SetParent(container.transform);
        mapContainer.transform.position = new Vector2(GameData.mapWidth/2, GameData.mapHeight/2);
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
                SpawnTile(new Vector2(-map.width + 2*(i % map.width)  + j % 2, map.height -2 * (i / map.width) - Mathf.FloorToInt((float)j/4 + 0.51f)),mapContainer);
            }
        }


    }
   

  
   

    public void setCurrentMap(int index){
        currentMap = index;
       
    } 

}

