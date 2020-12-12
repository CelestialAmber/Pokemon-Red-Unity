using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PokemonDataNew{
    public string name;
    public int[] baseStats;
    public int baseExp;
    public Tuple<string,int> evolution;
    public Moves[] tmhmLearnset;
    public Tuple<Moves,int>[] levelupLearnset;
    
}


public class NewPokemonDataJSONTest : MonoBehaviour
{

    public PokemonDataNew pokemonData;

    // Start is called before the first frame update
    void Start()
    {
        pokemonData = Serializer.JSONtoObject<PokemonDataNew>("pokemonCombinedData.json");
        for(int i = 0; i < pokemonData.levelupLearnset.Length; i++){
            Debug.Log(pokemonData.levelupLearnset[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
