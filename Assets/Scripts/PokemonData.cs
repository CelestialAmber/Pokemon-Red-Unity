using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[System.Serializable]
public class MoveData{
    public MoveData(string name, int power, string type, int accuracy, int maxpp,string effect){
        this.name = name;
        this.power = power;
        this.type = type;
        this.accuracy = accuracy;
        this.maxpp = maxpp;
        this.effect = effect;
    }
    public string name;
    public int power;
    public string type;
    public int accuracy;
    public int maxpp;
    public string effect;

}
//Class for encounter tables.
[System.Serializable]
public class EncounterData{
    public int encounterChance;
    public Tuple<string,int>[] slots;
}

public class FishingGroup{
    public Tuple<string,int>[] slots;
    public FishingGroup(Tuple<string,int>[] slots) {
        this.slots = slots;
    } 
}

public class PokemonData
{

    public static MoveData GetMove(int moveToGet){
        //Debug.Log("Requesting move " + "\"" + moveToGet + "\"");
        if(moveToGet < moves.Count && moveToGet != (int)Moves.None) return moves[moveToGet - 1];
        //If the index is out of range, throw an exception.
        throw new IndexOutOfRangeException("The move index is out of range.");
    }

    //Format: name, power, type, accuracy, max pp, effect
    public static List<MoveData> moves = new List<MoveData>();

    public static int MonToID(string name){
        return PokemonToIndex[name];
    }

    //format: move name, pokemon name
    public static Dictionary<string, string[]> learnbytm = new Dictionary<string, string[]>();
    //format(HP,Attack,Defense,Speed,Special)
    public static Dictionary<string, int[]> baseStats = new Dictionary<string, int[]>();
    public static Dictionary<string, Tuple<string,int>[]> levelmoves = new Dictionary<string, Tuple<string,int>[]>();
    //format: pokemon name as key, outputs pokemon and evolved level
    public static Dictionary<string, Tuple<string,int>> evolution = new Dictionary<string, Tuple<string,int>>();
    public static List<EncounterData> encounters = new List<EncounterData>();
    /* Encounter Table Indices:
    0:Diglett Cave
    1:Mansion 1
    2:Mansion 2
    3:Mansion 3
    4:Mansion B1
    5:Mt Moon 1
    6:Mt Moon B1
    7:Mt Moon B2
    8:Pokemon Tower 1
    9:Pokemon Tower 2
    10:Pokemon Tower 3
    11:Pokemon Tower 4
    12:Pokemon Tower 5
    13:Power Plant
    14:Rock Tunnel 1
    15:Rock Tunnel 2
    16:Route 1
    17:Route 2
    18:Route 3
    19:Route 4
    20:Route 5
    21:Route 6
    22:Route 7
    23:Route 8
    24:Route 9
    25:Route 10
    26:Route 11
    27:Route 12
    28:Route 13
    29:Route 14
    30:Route 15
    31:Route 16
    32:Route 17
    33:Route 18
    34:Route 21
    35:Route 22
    36:Route 23
    37:Route 24
    38:Route 25
    39:Safari Zone 1
    40:Safari Zone 2
    41:Safari Zone 3
    42:Safari Zone Center
    43:Seafoam Island 1
    44:Seafoam Island B1
    45:Seafoam Island B2
    46:Seafoam Island B3
    47:Seafoam Island B4
    48:Unknown Dungeon 1
    49:Unknown Dungeon 2
    50:Unknown Dungeon B1
    51:Victory Road 1
    52:Victory Road 2
    53:Victory Road 3
    54:Viridian Forest
    55:Water Pokemon
     */
     
     //fishing group for good rod
     public FishingGroup goodRodFishingGroup = new FishingGroup(new Tuple<string,int>[]{new Tuple<string, int>("Goldeen",10),new Tuple<string, int>("Poliwag",10)});
//groups for the super rod
public static List<FishingGroup> superRodFishingGroups = new List<FishingGroup>(new FishingGroup[]{ 
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",15), new Tuple<string,int>("Poliwag",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Poliwag",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Krabby",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Shellder",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Poliwhirl",23), new Tuple<string,int>("Slowpoke",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Dratini",15), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Slowpoke",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",5), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Staryu",15), new Tuple<string,int>("Horsea",15), new Tuple<string,int>("Shellder",15), new Tuple<string,int>("Goldeen",15)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Slowbro",23), new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Kingler",23), new Tuple<string,int>("Seadra",23)}),
new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
    });

    /*
List of index of the party sprite for each Pokemon.
0:Generic Sprite
1:Bird Sprite
2:Water Sprite
3:Clefairy Sprite
4:Grass Sprite
5:Bug Sprite
6:Dragon Sprite
7:Dog Sprite
8:Pokeball Sprite
9:Fossil Sprite
10:Missingno Sprite
*/
    public static Dictionary<string, int> PokemonPartySprite = new Dictionary<string, int>();
    public static Dictionary<string, string[]> PokemonTypes = new Dictionary<string, string[]>();
    public static Dictionary<string, int> PokemonExpGroup = new Dictionary<string, int>();
    public static string IndexToMon(int index)
    {
        int i = 1;
        foreach (var key in PokemonToIndex.Keys)
        {
            if (i == index)
            {
                return key;
            }
            i++;
        }
        return "";
    }
    public static string IndexToMonUpper(int index)
    {
        int i = 1;
        foreach (var key in PokemonToIndex.Keys)
        {
            if (i == index)
            {
                return key.ToUpper();
            }
            i++;
        }
        return "";
    }
    public static Dictionary<string, int> PokemonToIndex = new Dictionary<string, int>();
    public static Dictionary<string, int> TMHMtoIndex = new Dictionary<string, int>();
    public static Dictionary<string, int> itemPrices = new Dictionary<string, int>();
public static Dictionary<string,Dictionary<string,float>> TypeEffectiveness = new Dictionary<string, Dictionary<string, float>>();
public static Dictionary<string,string[]> shopItemsLists = new Dictionary<string, string[]>();

public static string[] typeNames = {
    "NORMAL",
    "FIGHTING",
    "FLYING",
    "POISON",
    "FIRE",
    "WATER",
    "GRASS",
    "ELECTRIC",
    "PSYCHIC",
    "ICE",
    "GROUND",
    "ROCK",
    "BIRD",
    "BUG",
    "GHOST",
    "DRAGON"
};

}