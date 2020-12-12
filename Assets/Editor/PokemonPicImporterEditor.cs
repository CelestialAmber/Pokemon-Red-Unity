using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PokemonPicImporter))]
public class PokemonPicImporterEditor : Editor {
    public PokemonPicImporter picImport;
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        picImport = (PokemonPicImporter)target;
        if(GUILayout.Button("Load Pic file")){
            picImport.LoadPicFile();
        }
    }
}
