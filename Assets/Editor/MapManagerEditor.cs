using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor {
    public MapManager mapManager;
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        mapManager = (MapManager)target;
        if(GUILayout.Button("Set tilemaps")){
          MapCollider[] mapColliders = mapManager.GetComponentsInChildren<MapCollider>(true);
          foreach(MapCollider mapCollider in mapColliders)mapCollider.UpdateTilemapObjects();
          
        }
    }
}
