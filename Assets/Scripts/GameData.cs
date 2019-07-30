using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class PokedexEntry
{
    public PokedexEntry(bool seen, bool caught){
        this.seen = seen;
        this.caught = caught;
    }
    public bool seen;
    public bool caught;
}
public enum Starter
{
    Bulbasaur,
    Charmander,
    Squirtle
}
public static class IntExtensions{

    public static string ZeroFormat(this int input, string zeroformat)
    {
        switch(zeroformat){
            case "0x":
                return (input > 9 ? "" : "0") + input;
            case "00x":
                return (input > 99 ? "" : input > 9 ? "0" : "00") + input;
            case "000x":
                return (input > 999 ? "" : input > 99 ? "0" : input > 9 ? "00" : "000") + input;
        }
       throw new UnityException("Incorrect format");

    }
     public static string SpaceFormat(this int input, int format)
    {
        switch(format){

            case 2:
                return (input < 10 ? " " : "") + input;
            case 3:
                return (input < 10 ? "  " : input < 100 ? " ": "") + input;
        }
       throw new UnityException("Incorrect format");

    }
    public static int UnderflowUInt24(this int input){ //underflow function for the exp bug
        if(input < 0){
             input += (int)Mathf.Pow(2,24);
        }
        else if(input >= Mathf.Pow(2,24)) input %= (int)Mathf.Pow(2,24);
        return input;
    }
}



[System.Serializable]
public class SaveData
{
    //class for holding save data
    public int dummy;
    public Starter chosenStarter;
    public static SaveData Create()
    {
        SaveData saveData = new SaveData();
        return saveData;
    }
}
//Class containing all the core data of the game.
public class GameData : MonoBehaviour{
    private static GameData m_instance;
    public static GameData instance {
        get{
            if(m_instance == null) m_instance = FindObjectOfType(typeof(GameData)) as GameData;
            return m_instance;
        }
    }
    public List<string> fieldMoves = new List<string>(new string[] { "Teleport", "Fly", "Cut", "Surf", "Dig", "Strength", "Flash", "Softboiled" });
    public List<Pokemon> party = new List<Pokemon>();
    public void AddPokemonToParty(string name,int level)
    {
        party.Add(new Pokemon(name, level, false));
    }
    [HideInInspector]
    public Sprite[] frontMonSprites, backMonSprites;
    public bool isPaused, inGame, atTitleScreen;
    public SaveData saveData;
    public void Init()
    {
        frontMonSprites = Resources.LoadAll<Sprite>("frontmon");
        backMonSprites = Resources.LoadAll<Sprite>("backmon");
        pokedexlist = new List<PokedexEntry>();
            for (int i = 0; i < 151; i++) {
                pokedexlist.Add(new PokedexEntry(false, false));
            }
        if (playerName == "") playerName = "RED";

    }
    public List<PokedexEntry> pokedexlist = new List<PokedexEntry>(151);
    public bool[] hasBadge = new bool[8];
    public int money;
    public int coins;
    public int trainerID;
    public int textChoice, animationChoice, battleChoice;
    public string playerName, rivalName;
    public int hours, minutes, seconds;
    public Starter chosenStarter;
    public bool hasMetBill; //should Bill's PC use his name?

    public Version version;

    public FontAtlas fontAtlas;

    public void Save()
    {
        //update the game variables to the save data
        saveData = SaveData.Create();
        SaveGameData(Application.persistentDataPath + "/save.sav", saveData); //save
    }
    //encounter table indices for all maps
    [HideInInspector]
    public int[] MapGrassEncounterTableIndices = {
        -1, //Pallet Town
        -1, //Oak's Lab
        16, //Route 1
        -1, //Viridian City
        -1, //Poke Center
        -1, //Poke Mart
        -1, //Pokemon Gym
        35, //Route 22
        36, //Route 23
        51, //VR 1
        52, //VR 2
        53, //VR 3
        -1, //Indigo Plateau
        -1, //Lorelei
        -1, //Bruno
        -1, //Agatha
        -1, //Hall of Fame Room
        17, //Route 2
        54, //Viridian Forest
        0, //Diglett Cave
        -1, //Pewter City
        18, //Route 3
        3, //Mt. Moon 1
        4, //Mt Moon B1
        5, //Mt Moon B2
        19, //Route 4
        -1, //Cerulean City
        37, //Route 24
        38, //Route 25
        20, //Route 5
        -1, //Underground Road
        21, //Route 6
        -1, //Vermillion City
        -1, //S.S. Anne
        26, //Route 11
        24, //Route 9
        25, //Route 10
        14, //Rock Tunnel 1
        15, //Rock Tunnel 2
        13, //Power Plant
        -1, //Lavender Town
        -1, //Pokemon Tower 1
        -1, //Pokemon Tower 2
        8, //Pokemon Tower 3
        9, //Pokemon Tower 4
        10, //Pokemon Tower 5
        11, //Pokemon Tower 6
        12, //Pokemon Tower 7
        23, //Route 8
        22, //Route 7
        -1, //Celadon City
        -1, //Game Corner
        -1,//Rocket Hideout
        31, //Route 16
        32, //Route 17
        33, //Route 18
        -1, //Fuchsia City
        42, //Safari Zone Center
        39, //Safari Zone East
        40, //Safari Zone North
        41, //Safari Zone West
        -1, //Safari Zone House
        30, //Route 15
        29, //Route 14
        28, //Route 13
        27, //Route 12
        -1, //Saffron City
        -1, //Silph Co.
        -1, //Sea Route 19
        43, //Seafoam Islands 1
        44, //Seafoam Islands B1
        45, //Seafoam Islands B2
        46, //Seafoam Islands B3
        47, //Seafoam Islands B4
        -1, //Sea Route 20
        -1, //Cinnabar Island
        1,//Mansion 1
        2,//Mansion 2
        3,//Mansion 3
        4,//Mansion 4
        34, //Sea Route 21
        48,//Unknown 1
        49,//Unknown 2
        50,//Unknown 3
        -1,//Trade Center
        -1,//Colloseum
        -1, //Bill's House
        -1, //Houses
        -1,
        -1


    };
    //List of maps that have water encounters
    [HideInInspector]
    public List<Map> WaterEncounterMaps = new List<Map>(
        new Map[]{
            Map.Route19,
            Map.Route20,
            Map.Route21,
            Map.Route23,
            Map.Route12,
            Map.SeafoamIslands1,
            Map.SeafoamIslands2,
            Map.SeafoamIslands3,
            Map.SeafoamIslands4,
            Map.SeafoamIslands5,
            Map.Unknown1,
            Map.Unknown3,
        }
    );

    public SaveData LoadGameData<SaveData>(string filename)
    {
            FileStream file;
            StreamReader sr;
            file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(file);
            string data = sr.ReadToEnd();
            file.Close();
            return JsonConvert.DeserializeObject<SaveData>(data);
    }

    public void SaveGameData(string filename, SaveData saveData) //method for saving the game
    {

        string data = JValue.Parse(JsonConvert.SerializeObject(saveData)).ToString();
        File.WriteAllText(filename, data);

    }
}

