using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PokemonDataJSON : MonoBehaviour {
    public VersionManager versionManager;
    void Awake(){
        PokemonData.TypeEffectiveness = Serializer.JSONtoObject<Dictionary<string,Dictionary<string,float>>>("typeEffectiveness.json");
        PokemonData.evolution = Serializer.JSONtoObject<Dictionary<string,Tuple<string,int>>>("evolutiondata.json");
        PokemonData.baseStats = Serializer.JSONtoObject<Dictionary<string,int[]>>("basestatsdata.json");
        PokemonData.levelmoves = Serializer.JSONtoObject<Dictionary<string, Tuple<string,int>[]>>("levelmovesdata.json");
        PokemonData.learnbytm = Serializer.JSONtoObject<Dictionary<string,string[]>>("learnbytmdata.json");
        PokemonData.moves = Serializer.JSONtoObject<List<MoveData>>("moveData.json");
        PokemonData.PokemonPartySprite = Serializer.JSONtoObject<Dictionary<string, int>>("partySpriteData.json");
        PokemonData.PokemonTypes = Serializer.JSONtoObject<Dictionary<string, string[]>>("pokemonTypeData.json");
        PokemonData.PokemonExpGroup = Serializer.JSONtoObject<Dictionary<string, int>>("expGroupData.json");
        PokemonData.PokemonToIndex = Serializer.JSONtoObject<Dictionary<string, int>>("pokemonIndices.json");
        PokemonData.TMHMtoIndex = Serializer.JSONtoObject<Dictionary<string, int>>("tmHmIndices.json");
        PokemonData.itemPrices = Serializer.JSONtoObject<Dictionary<string, int>>("itemPrices.json");
        PokemonData.shopItemsLists = Serializer.JSONtoObject<Dictionary<string,string[]>>("shopItemsData.json");
    }
    public static void InitVersion(){
        if(GameData.instance.version == Version.Red){
            PokemonData.encounters = Serializer.JSONtoObject<List<EncounterData>>("encounterDataRed.json");
        }
        else PokemonData.encounters = Serializer.JSONtoObject<List<EncounterData>>("encounterDataBlue.json");
    }
}

