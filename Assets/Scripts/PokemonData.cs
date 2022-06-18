using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[System.Serializable]
public class MoveData{
    public MoveData(string name, int power, Types type, int accuracy, int maxpp,string effect){
        this.name = name;
        this.power = power;
        this.type = type;
        this.accuracy = accuracy;
        this.maxpp = maxpp;
        this.effect = effect;
    }
    public string name;
    public int power;
    public Types type;
    public int accuracy;
    public int maxpp;
    public string effect;

}

[System.Serializable]
public class LevelUpMove{
    public Moves move;
    public int level;

    public LevelUpMove(Moves move, int level){
        this.move = move;
        this.level = level;
    }
}

//Enum for the different evolution methods
public enum EvolutionMethod {
  Level,
  Item,
  Trade
}

[System.Serializable]
public class PokemonEvolution{
    public PokemonEnum pokemon;
    public EvolutionMethod method;
	/*
	Contains different values depending on the evolution method
	(level up: level, item: evo item id, trade: n/a)
	*/
	/*
	evolution item ids:
	0: Fire Stone
	1: Thunder Stone
	2: Water Stone
	3: Leaf Stone
	4: Moon Stone
	
	*/
    public int param;
}

//Class for encounter tables.
[System.Serializable]
public class EncounterData{
    public int encounterChance;
    public Tuple<PokemonEnum,int>[] slots;
}

public class FishingGroup{
    public Tuple<string,int>[] slots;
    public FishingGroup(Tuple<string,int>[] slots) {
        this.slots = slots;
    } 
}


[System.Serializable]
public class PokemonDataEntry {
    public string name;
    public int id;
	public string category;
    public int heightFeet, heightInches;
    //weight in pounds
    public float weight;
    public string[] descriptionText;
    /*
    Sprite for party menu

    Sprite types:
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
    public int partySprite;
    public int[] baseStats = new int[5];
    public int baseExp;
    public int expGroup;
    public List<PokemonEvolution> evolutions;
    public Types[] types = new Types[2];
    public int[] tmhmLearnset;
    public LevelUpMove[] levelupLearnset;
}

[System.Serializable]
public class ItemDataEntry {
	public string name;
	public int price; //item's buy price (the sell price is just half the buy price)
}


public class PokemonData
{
    public static MoveData GetMove(int moveToGet){
        //Debug.Log("Requesting move " + "\"" + moveToGet + "\"");
        if(moveToGet < moves.Count && moveToGet != (int)Moves.None) return moves[moveToGet - 1];
        //If the index is out of range, throw an exception.
        throw new IndexOutOfRangeException("The move index is out of range.");
    }

    public static string GetTypeName(Types type){
        if(type == Types.None) return "";
        else return typeNames[(int)type - 1];
    }

    public static string GetItemName(ItemsEnum item){
        return itemData[(int)item].name;
    }

    public static int GetItemPrice(ItemsEnum item){
        return itemData[(int)item].price;
    }

    public static Moves TMHMToMove(int tmhmIndex){
        return TMHMMoves[tmhmIndex];
    }

    //Format: name, power, type, accuracy, max pp, effect
    public static List<MoveData> moves = new List<MoveData>();


    public static string IndexToMon(int index){
        return pokemonData[index - 1].name;
    }

    public static List<PokemonDataEntry> pokemonData = new List<PokemonDataEntry>();
    
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

    //TODO: These really should be put into a json file
     
    //fishing group for good rod
    public FishingGroup goodRodFishingGroup = new FishingGroup(new Tuple<string,int>[]{new Tuple<string, int>("Goldeen",10),new Tuple<string, int>("Poliwag",10)});
    
    //groups for the super rod
    public static List<FishingGroup> superRodFishingGroups = new List<FishingGroup>(new FishingGroup[]{ 
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",15), new Tuple<string,int>("Poliwag",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Poliwag",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Krabby",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Poliwhirl",23), new Tuple<string,int>("Slowpoke",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Shellder",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Dratini",15), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Psyduck",15), new Tuple<string,int>("Slowpoke",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Tentacool",5), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Staryu",15), new Tuple<string,int>("Horsea",15), new Tuple<string,int>("Shellder",15), new Tuple<string,int>("Goldeen",15)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Slowbro",23), new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Kingler",23), new Tuple<string,int>("Seadra",23)}),
        new FishingGroup(new Tuple<string,int>[]{new Tuple<string,int>("Seaking",23), new Tuple<string,int>("Krabby",15), new Tuple<string,int>("Goldeen",15), new Tuple<string,int>("Magikarp",15)}),
    });



    public static List<ItemDataEntry> itemData = new List<ItemDataEntry>();
public static Dictionary<Types,Dictionary<Types,float>> TypeEffectiveness = new Dictionary<Types, Dictionary<Types, float>>();
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

public static Moves[] TMHMMoves = {
    Moves.MegaPunch,
    Moves.RazorWind,
    Moves.SwordsDance,
    Moves.Whirlwind,
    Moves.MegaKick,
    Moves.Toxic,
    Moves.HornDrill,
    Moves.BodySlam,
    Moves.TakeDown,
    Moves.DoubleEdge,
    Moves.Bubblebeam,
    Moves.WaterGun,
    Moves.IceBeam,
    Moves.Blizzard,
    Moves.HyperBeam,
    Moves.PayDay,
    Moves.Submission,
    Moves.Counter,
    Moves.SeismicToss,
    Moves.Rage,
    Moves.MegaDrain,
    Moves.Solarbeam,
    Moves.DragonRage,
    Moves.Thunderbolt,
    Moves.Thunder,
    Moves.Earthquake,
    Moves.Fissure,
    Moves.Dig,
    Moves.Psychic,
    Moves.Teleport,
    Moves.Mimic,
    Moves.DoubleTeam,
    Moves.Reflect,
    Moves.Bide,
    Moves.Metronome,
    Moves.Selfdestruct,
    Moves.EggBomb,
    Moves.FireBlast,
    Moves.Swift,
    Moves.SkullBash,
    Moves.Softboiled,
    Moves.DreamEater,
    Moves.SkyAttack,
    Moves.Rest,
    Moves.ThunderWave,
    Moves.Psywave,
    Moves.Explosion,
    Moves.RockSlide,
    Moves.TriAttack,
    Moves.Substitute,
    Moves.Cut,
    Moves.Fly,
    Moves.Surf,
    Moves.Strength,
    Moves.Flash
};

}