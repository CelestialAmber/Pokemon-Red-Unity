using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[RequireComponent(typeof(BoxCollider2D))]
public class MapCollider : MonoBehaviour
{
    
    public bool canUseBike = true; //can the player use the bike on this map?
    public Map mapArea;

    public Tilemap waterTilemap, overworldTilemap, grassTilemap, ledgeTilemap;

    public GameObject tilemapObject;
    
    public Collider2D lastCol, col;

    public bool[] isTouchingCollider = new bool[5];


void Start(){
    col = GetComponent<BoxCollider2D>();
    for(int i = 0; i < 5; i++) isTouchingCollider[i] = false;
}

void OnEnterMapCollision(string tag){
        switch(tag){
            case "PlayerMapColLeft":
            MapManager.instance.mapColliders[2] = this;
            break;
            case "PlayerMapColRight":
            MapManager.instance.mapColliders[3] = this;
            break;
            case "PlayerMapColUp":
            MapManager.instance.mapColliders[0] = this;
            break;
            case "PlayerMapColDown":
            MapManager.instance.mapColliders[1] = this;
            break;
            case "PlayerMapColCenter":
            MapManager.instance.mapColliders[4] = this;
            break;
        }
        lastCol = col;
        tilemapObject.SetActive(true);
}

 public void UpdateTilemapObjects(){
        tilemapObject = transform.parent.GetChild(0).gameObject;
        waterTilemap = tilemapObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        grassTilemap = tilemapObject.transform.GetChild(1).GetChild(0).GetComponent<Tilemap>();
        overworldTilemap = tilemapObject.transform.GetChild(2).GetChild(0).GetComponent<Tilemap>();
        ledgeTilemap = tilemapObject.transform.GetChild(3).GetChild(0).GetComponent<Tilemap>();
    }




    
}





