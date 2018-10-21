using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.IO;
using System;
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json.Linq;

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
    public StrInt[] slots;
}


//Save/Loader for Tile Pool
public class Serializer
{
    //Should test WWW loading code be enabled, or default to file loading?
    public static bool wwwLoad = true;

    public static T[,] Load2D<T>(string filename) where T : class
    {


        try
        {


            using (Stream stream = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T[,];
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return default(T[,]);
    }

    public static void Save2D<T>(string filename, T[,] data) where T : class
    {
        using (Stream stream = File.OpenWrite(filename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }


    public static void objectToJSON (string fileName,object type){
        string data =  JValue.Parse(JsonConvert.SerializeObject(type)).ToString(Formatting.Indented);
        File.WriteAllText(Application.streamingAssetsPath + "/" + fileName, data);
    }
    public static T JSONtoObject<T> (string fileName){
        FileStream file;
        StreamReader sr;
        file = new FileStream(Application.streamingAssetsPath + "/" + fileName, FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string tmlearndata = sr.ReadToEnd();
        file.Close();
        return JsonConvert.DeserializeObject<T>(tmlearndata);
    }

}

public class PokemonData
{

    public static MoveData GetMove(string moveToGet)
    {
        //Debug.Log("Requesting move " + "\"" + moveToGet + "\"");
        //Iterate through the array and find the move by its index.
        foreach (MoveData move in moves)
        {
            if (move.name == moveToGet) return move;
        }
        //If it doesn't find the move, throw an exception.
        throw new UnityException("Requested move does not exist.");
    }
    //Format: name, power, type, accuracy, max pp, effect
    public static List<MoveData> moves = new List<MoveData>();
    public static int MonToID(string name)
    {
        return PokemonToIndex[name];
    }

    //format: move name, pokemon name
    public static Dictionary<string, string[]> learnbytm = new Dictionary<string, string[]>();
    //format(HP,Attack,Defense,Speed,Special)
    public static Dictionary<string, int[]> baseStats = new Dictionary<string, int[]>();
    public static Dictionary<string, StrInt[]> levelmoves = new Dictionary<string, StrInt[]>();
    //format: pokemon name as key, outputs pokemon and evolved level
    public static Dictionary<string, StrInt> evolution = new Dictionary<string, StrInt>();
    public static List<EncounterData> grasswaterencounters = new List<EncounterData>();
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
}

public class PokemonDataJSON : MonoBehaviour
{
     void Awake()
    {

        PokemonData.evolution = Serializer.JSONtoObject<Dictionary<string,StrInt>>("evolutiondata.json");
       PokemonData.baseStats = Serializer.JSONtoObject<Dictionary<string,int[]>>("basestatsdata.json");
        PokemonData.levelmoves = Serializer.JSONtoObject<Dictionary<string, StrInt[]>>("levelmovesdata.json");
        PokemonData.learnbytm = Serializer.JSONtoObject<Dictionary<string,string[]>>("learnbytmdata.json");
        PokemonData.grasswaterencounters = Serializer.JSONtoObject<List<EncounterData>>("encounterData.json");
        PokemonData.moves = Serializer.JSONtoObject<List<MoveData>>("moveData.json");
        PokemonData.PokemonPartySprite = Serializer.JSONtoObject<Dictionary<string, int>>("partySpriteData.json");
        PokemonData.PokemonTypes = Serializer.JSONtoObject<Dictionary<string, string[]>>("pokemonTypeData.json");
        PokemonData.PokemonExpGroup = Serializer.JSONtoObject<Dictionary<string, int>>("expGroupData.json");
        PokemonData.PokemonToIndex = Serializer.JSONtoObject<Dictionary<string, int>>("pokemonIndices.json");
        PokemonData.TMHMtoIndex = Serializer.JSONtoObject<Dictionary<string, int>>("tmHmIndices.json");
        PokemonData.itemPrices = Serializer.JSONtoObject<Dictionary<string, int>>("itemPrices.json");


       
    }
}

[System.Serializable]
public class StrInt
{
    public string Name;
    public int Int;
    public StrInt(string first, int second)
    {
        Name = first;
        Int = second;
    }
    public object this[int index]
    {
        get
        {
            return FetchValue(index);
        }

    }
    object FetchValue(int index)
    {
        switch (index)
        {
            case 0:
                return Name;
            case 1:
                return Int;
            default:
                throw new IndexOutOfRangeException("Index is not 0 or 1.");
        }
    }
    public bool Equals(StrInt str)
    {
        return str.Name.Equals(Name) && str.Int.Equals(Int);
    }
    public override bool Equals(object o)
    {
        return this.Equals(o as StrInt);
    }
    public override int GetHashCode()
    {
        return Name.GetHashCode() ^ Int.GetHashCode();
    }
}

