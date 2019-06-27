using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapCollision : MonoBehaviour
{
    public enum ColliderType{
        Up,
        Down,
        Left,
        Right,
        Center
    }
    public ColliderType colliderType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "MapCollider"){
            MapCollider mapCollider = col.GetComponent<MapCollider>();
            MapManager.instance.mapColliders[(int)colliderType] = mapCollider;
            mapCollider.isTouchingCollider[(int)colliderType] = true;
            mapCollider.tilemapObject.SetActive(true);
    }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.tag == "MapCollider"){
            MapCollider mapCollider = col.GetComponent<MapCollider>();
             mapCollider.isTouchingCollider[(int)colliderType] = false;
            for(int i = 0; i < 5; i++) if(mapCollider.isTouchingCollider[i]) return;
            mapCollider.tilemapObject.SetActive(false);
    }
    }
}
