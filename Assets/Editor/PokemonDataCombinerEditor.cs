using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PokemonDataCombiner))]
public class PokemonDataCombinerEditor : Editor {
    public PokemonDataCombiner dataCombiner;

    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        dataCombiner = (PokemonDataCombiner)target;
        if(GUILayout.Button("Combine Pokemon data json files")){
            dataCombiner.CombinePokemonData();
            //dataCombiner.TestPokemonEnumToJSON();
        }
    }
}

