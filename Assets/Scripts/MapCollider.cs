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
    
    public Collider2D  col;


void Start(){
    col = GetComponent<BoxCollider2D>();
}



 public void UpdateTilemapObjects(){
        tilemapObject = transform.parent.GetChild(0).gameObject;
        waterTilemap = tilemapObject.transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        grassTilemap = tilemapObject.transform.GetChild(1).GetChild(0).GetComponent<Tilemap>();
        overworldTilemap = tilemapObject.transform.GetChild(2).GetChild(0).GetComponent<Tilemap>();
        ledgeTilemap = tilemapObject.transform.GetChild(3).GetChild(0).GetComponent<Tilemap>();
    }




    
}





