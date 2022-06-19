using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewPokemonDataJSONTest : MonoBehaviour
{

    public List<PokemonDataEntry> pokemonData;
    public List<ItemDataEntry> itemData;

    // Start is called before the first frame update
    void Start()
    {
        pokemonData = Serializer.JSONtoObject<List<PokemonDataEntry>>("pokemonData.json");
        itemData = Serializer.JSONtoObject<List<ItemDataEntry>>("itemData.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
