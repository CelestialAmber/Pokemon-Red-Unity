using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PokemonDataJSON : MonoBehaviour {
    public VersionManager versionManager;
    void Awake(){
        //this should also be a class list
        PokemonData.TypeEffectiveness = Serializer.JSONtoObject<Dictionary<Types,Dictionary<Types,float>>>("typeEffectiveness.json");
        PokemonData.pokemonData = Serializer.JSONtoObject<List<PokemonDataEntry>>("pokemonData.json");
        PokemonData.moves = Serializer.JSONtoObject<List<MoveData>>("moveData.json");
        PokemonData.itemData = Serializer.JSONtoObject<List<ItemDataEntry>>("itemData.json");
        //also should be changed
        PokemonData.shopItemsLists = Serializer.JSONtoObject<Dictionary<string,string[]>>("shopItemsData.json");
    }
    public static void InitVersion(){
        if(GameData.instance.version == Version.Red){
            PokemonData.encounters = Serializer.JSONtoObject<List<EncounterData>>("encounterDataRed.json");
        }
        else PokemonData.encounters = Serializer.JSONtoObject<List<EncounterData>>("encounterDataBlue.json");
    }
}

