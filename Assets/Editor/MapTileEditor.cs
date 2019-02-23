using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;
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
        if (GUILayout.Button("Save Map to file"))
        {
            GridTile[,] loadedTiles = Serializer.Load2D<GridTile>(Application.streamingAssetsPath + "/map.txt");

            List<GameObject> foundtiles = new List<GameObject>();

            int index = 0;
            //look into each map for their tiles;
            foreach (Transform mapchild in me.container.transform)
            {
                foreach (Transform child in mapchild)
                {
                    foundtiles.Add(child.gameObject);
                }
            }
            index = 0;
            foreach (GameObject child in foundtiles)
            {
                int x = (int)child.transform.position.x;
                int y = (int)child.transform.position.y;
                loadedTiles[x, y] = new GridTile(null, null, null, false, 0, 0, "", false, false, false, 1, "", false, false);

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
                gridTile.isInteractable = child.GetComponent<TileProperties>();
                gridTile.tileWarp = child.GetComponent<TileWarp>() ? child.GetComponent<TileWarp>().info : null;
                gridTile.tiledata = child.GetComponent<TileProperties>() ? child.GetComponent<TileProperties>().data : null;
                if (gridTile.isInteractable)
                {
                    gridTile.hasItem = gridTile.tiledata.hasItem;
                }
                if (gridTile.isAnimated)
                {
                    tempname = AssetDatabase.GetAssetPath(child.GetComponent<SpriteRenderer>().sprite);
                    if (tempname.Contains("_"))
                    {
                        tempname = tempname.Substring(0, tempname.IndexOf('_'));
                    }
                    gridTile.frames = Resources.LoadAll<Sprite>(tempname.Replace("Assets/Resources/", "").Replace(".png", "")).Length;

                }


                EncounterTile encounterTile = null;
                if (child.GetComponent<EncounterTile>())
                {
                    encounterTile = child.GetComponent<EncounterTile>();
                }


                if (encounterTile != null)
                {
                    gridTile.encounterInfo = encounterTile.info;
                    switch (child.tag)
                    {
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
            Serializer.Save2D<GridTile>(Application.streamingAssetsPath + "/map.txt", loadedTiles);

        }
        if (GUILayout.Button("Save Empty map to file"))
        {
            GridTile[,] nullMap = new GridTile[GameData.mapWidth, GameData.mapHeight];
            Serializer.Save2D<GridTile>(Application.streamingAssetsPath + "/map.txt", nullMap);
        }
        if (GUILayout.Button("Load editor data"))
        {
            me.tSetIndexes = Serializer.JSONtoObject<TilesetIndex>("blockIndexData.json");
            me.maps = Serializer.JSONtoObject<MapData[]>("romMapData.json");
            me.tilepool = Serializer.JSONtoObject<List<Tile>>("tilePoolData.json");

        }
        if (GUILayout.Button("Save tilepool"))
        {

            Serializer.objectToJSON("tilePoolData.json", me.tilepool);

        }
        if (GUILayout.Button("Add all tiles"))
        {
            me.tilepool.Clear();
            for (int i = 0; i < 871; i++)
            {
                me.tilepool.Add(new Tile("", "", false));
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
                if (path.Contains("/Water"))
                {
                    tag = "Water";
                }
                string thisIndex = tile.name;
                int index = int.Parse(thisIndex.Replace("tile", ""));



                if (tile.width > 16 || tile.height > 16) isAnimated = true;
                me.tilepool[index] = new Tile(AssetDatabase.GetAssetPath(tile), tag, isAnimated);


            }
        }
        if (GUILayout.Button("Spawn map"))
        {

            me.SpawnMap();
        }





    }


}