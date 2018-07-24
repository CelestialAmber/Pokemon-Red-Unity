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
public class EncounterEntry
{
    public EncounterEntry(int index, StrInt[] table)
    {
        this.index = index;
        this.table = table;
    }
    public int index;
    public StrInt[] table;
}
[System.Serializable]
public class EncounterDictionary
{
    public List<EncounterEntry> entries = new List<EncounterEntry>();
}
[System.Serializable]
public class EvolutionEntry{
    public EvolutionEntry(string pokemon, StrInt evolution)
    {
        this.pokemon = pokemon;
        this.evolution = evolution;
    }
    public string pokemon;
    public StrInt evolution;
}
[System.Serializable]
public class EvolutionDictionary
{
    public List<EvolutionEntry> entries = new List<EvolutionEntry>();
}
[System.Serializable]
public class TMLearnEntry
{
    public TMLearnEntry(string pokemon, string[] moves)
    {
        this.pokemon = pokemon;
        this.moves = moves;
    }
    public string pokemon;
    public string[] moves;
}
[System.Serializable]
public class TMLearnDictionary
{
    public List<TMLearnEntry> entries = new List<TMLearnEntry>();
}
[System.Serializable]
public class BaseStatEntry
{
    public BaseStatEntry(string pokemon, int[] stats){
        this.pokemon = pokemon;
        this.stats = stats;
    }
    public string pokemon;
    public int[] stats;
}
[System.Serializable]
public class BaseStatDictionary
{
    public List<BaseStatEntry> entries  = new List<BaseStatEntry>();
}
[System.Serializable]
public class LevelMovesEntry
{
    public LevelMovesEntry(string pokemon, StrInt[] moves)
    {
        this.pokemon = pokemon;
        this.moves = moves;
    }
    public string pokemon;
    public StrInt[] moves;
}
[System.Serializable]
public class LevelMovesDictionary
{
    public List<LevelMovesEntry> entries = new List<LevelMovesEntry>();
}
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

public class PokemonStats
{
    
    public static MoveData GetMove(string moveToGet){
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
    public static int MonToID(string name){
        return PokemonToIndex[name];
    }

    //format: move name, pokemon name
    public static Dictionary<string, string[]> learnbytm = new Dictionary<string, string[]>();
    //format(HP,Attack,Defense,Speed,Special)
    public static Dictionary<string, int[]> baseStats = new Dictionary<string, int[]>();
    public static Dictionary<string, StrInt[]> levelmoves = new Dictionary<string, StrInt[]>();
    //format: pokemon name as key, outputs pokemon and evolved level
    public static Dictionary<string, StrInt> evolution = new Dictionary<string, StrInt>();
    public static Dictionary<int, StrInt[]> grasswaterencounters = new Dictionary<int, StrInt[]>();
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
    public static string IndexToMon(int index){
        int i = 1;
        foreach(var key in PokemonToIndex.Keys){
            if(i == index){
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
}

public class PokemonDataJSON : MonoBehaviour
{
     void Awake()
    {

        PokemonStats.evolution = Serializer.JSONtoObject<Dictionary<string,StrInt>>("evolutiondata.json");
       PokemonStats.baseStats = Serializer.JSONtoObject<Dictionary<string,int[]>>("basestatsdata.json");
        PokemonStats.levelmoves = Serializer.JSONtoObject<Dictionary<string, StrInt[]>>("levelmovesdata.json");
        PokemonStats.learnbytm = Serializer.JSONtoObject<Dictionary<string,string[]>>("learnbytmdata.json");
        PokemonStats.grasswaterencounters = Serializer.JSONtoObject<Dictionary<int,StrInt[]>>("encounterData.json");
        PokemonStats.moves = Serializer.JSONtoObject<List<MoveData>>("moveData.json");
        PokemonStats.PokemonPartySprite = Serializer.JSONtoObject<Dictionary<string, int>>("partySpriteData.json");
        PokemonStats.PokemonTypes = Serializer.JSONtoObject<Dictionary<string, string[]>>("pokemonTypeData.json");
        PokemonStats.PokemonExpGroup = Serializer.JSONtoObject<Dictionary<string, int>>("expGroupData.json");
        PokemonStats.PokemonToIndex = Serializer.JSONtoObject<Dictionary<string, int>>("pokemonIndices.json");
        PokemonStats.TMHMtoIndex = Serializer.JSONtoObject<Dictionary<string, int>>("tmHmIndices.json");
       
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PokemonDataJSON))]
public class PokemonDataEditor : Editor {
    
    public override void OnInspectorGUI(){
        PokemonDataJSON pokemonDataJSON = (PokemonDataJSON)target;
        DrawDefaultInspector();

    }


}
public class PokemonTMParser : EditorWindow{
    TMLearnDictionary tMLearn = new TMLearnDictionary();
    int currentpokemon, lastpokemon;
    public List<string> moveslist;
    public ReorderableList learnlist;
    Vector2 scrollpos;
    [MenuItem("Window/Pokemon TM Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PokemonTMParser));
    }
    void OnEnable()
    {
        FileStream file;
        StreamReader sr;
        tMLearn = new TMLearnDictionary();
        file = new FileStream(Application.streamingAssetsPath + "/learnbytmdata.json", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string tmlearndata = sr.ReadToEnd();
        tMLearn = JsonConvert.DeserializeObject<TMLearnDictionary>(tmlearndata);
        moveslist = new List<string>();
        moveslist.Clear();
        foreach (string move in tMLearn.entries[currentpokemon].moves)
        {
            moveslist.Add(move);
        }
        learnlist = new ReorderableList(moveslist, typeof(string), true, true, true, true);
        learnlist.onAddCallback = (ReorderableList list) =>{
            
            moveslist.Insert(learnlist.index + 1, "");
        };
        learnlist.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) => {
        rect.y += 2;
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),new GUIContent(PokemonStats.TMHMtoIndex[moveslist[index]].ToString()));
            EditorGUI.LabelField(new Rect(rect.x + 60, rect.y, 60, EditorGUIUtility.singleLineHeight), new GUIContent(moveslist[index]));
    };
        file.Close();
    }
    void OnGUI()
    {
        HandleKeyboard();
        EditorGUILayout.BeginVertical();
        scrollpos = EditorGUILayout.BeginScrollView(scrollpos);
        if (currentpokemon != lastpokemon)
        {
            moveslist.Clear();
            foreach (string move in tMLearn.entries[currentpokemon].moves)
            {
               moveslist.Add(move);
              
            }
            lastpokemon = currentpokemon;
            learnlist = new ReorderableList(moveslist, typeof(string), true, true, true, true);
            learnlist.onAddCallback = (ReorderableList list) => {moveslist.Insert(learnlist.index + 1, "");};
            learnlist.drawElementCallback =
 (Rect rect, int index, bool isActive, bool isFocused) => {
     rect.y += 2;
     EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), new GUIContent(PokemonStats.TMHMtoIndex[moveslist[index]].ToString()));
     EditorGUI.LabelField(new Rect(rect.x + 60, rect.y, 60, EditorGUIUtility.singleLineHeight), new GUIContent(moveslist[index]));
 };
        }
        currentpokemon = EditorGUILayout.IntField("Current Pokemon (" + tMLearn.entries[currentpokemon].pokemon + ")", currentpokemon);
        learnlist.DoLayoutList();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        if (GUILayout.Button("Save Changes to Learnset"))
        {

            tMLearn.entries[currentpokemon].moves = moveslist.ToArray();
        }
if(GUILayout.Button("Save Changes to JSON")){
            string jsondata = JsonConvert.SerializeObject(tMLearn);
            jsondata = JValue.Parse(jsondata).ToString(Formatting.Indented);
            File.WriteAllText(Application.streamingAssetsPath + "/learnbytmdata.json", jsondata);

        }

       
    }
    private void HandleKeyboard()
    {
        Event current = Event.current;
        if (current.type != EventType.KeyDown)
            return;
        
        switch (current.keyCode)
        {
            case KeyCode.C:
                moveslist.RemoveAt(learnlist.index);
                learnlist = new ReorderableList(moveslist, typeof(string), true, true, true, true);
                learnlist.onAddCallback = (ReorderableList list) => { moveslist.Insert(learnlist.index + 1, ""); };
                learnlist.drawElementCallback =
 (Rect rect, int index, bool isActive, bool isFocused) => {
     rect.y += 2;
     EditorGUI.LabelField(new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight), new GUIContent(PokemonStats.TMHMtoIndex[moveslist[index]].ToString()));
     EditorGUI.LabelField(new Rect(rect.x + 80, rect.y, 80, EditorGUIUtility.singleLineHeight), new GUIContent(moveslist[index]));
 };
                Repaint();
                break;
        }
    }

}
#endif
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

