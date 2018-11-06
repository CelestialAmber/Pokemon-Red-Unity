using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PokedexEntry
{
    public PokedexEntry(bool seen, bool caught){
        this.seen = seen;
        this.caught = caught;
    }
    public bool seen;
    public bool caught;
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
}
public class Get 
{
    public static void Init(){
        Get.bio = fetchObject("UI/Menus/BioMenu").GetComponent<ViewBio>();
        Get.pokedex = fetchObject("UI/Menus/Pokedex").GetComponent<Pokedex>();
        Get.bag = fetchObject("UI/Menus/Bag").GetComponent<Bag>();
        Get.menu = fetchObject("UI/Menus/StartMenu").GetComponent<MainMenu>();
        Get.player = fetchObject("Player").GetComponent<Player>();
        Get.pokeMenu = fetchObject("UI/Menus/PokemonMenu").GetComponent<PokemonMenu>();
        Get.items = fetchObject("PokemonDataManager").GetComponent<Items>();
    }
    public static GameObject fetchObject(string path){
        return GameObject.Find(path);
    }
    public static ViewBio bio;
    public static Pokedex pokedex;
    public static Bag bag;
    public static MainMenu menu;
    public static Player player;
    public static PokemonMenu pokeMenu;
    public static Items items;

}


public class Inputs
{
    public static void Init(){

        string[] controllerInfo = Input.GetJoystickNames();
        joyconConnected = false;
        foreach(string s in controllerInfo){
        if (s.Contains("Joy-Con")) //if one of the connected controllers is a Joy-Con, then set the Joy-Con as being connected
        {
            joyconConnected = true;
        }
        }
    }
   
    public static bool joyconConnected;
    public static bool dialogueCheck;
    public static void Disable(string button){
        buttonDisabled[keyindices[button]] = true;
    }
    
    public static void Enable(string button)
    {
        buttonDisabled[keyindices[button]] = false;
    }
    public static bool[] buttonDisabled = new bool[8];
    public static List<KeyCode> inputs = new List<KeyCode>(new KeyCode[]{
        KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.X,
            KeyCode.Z,
            KeyCode.Return,
            KeyCode.RightShift
    }
    );
    public static Dictionary<string, int> keyindices = new Dictionary<string, int>(){
    {"up", 0},
    {"down",1},
    {"left",2},
    {"right",3},
    {"b",4},
    {"a",5},
    {"start",6},
    {"select",7}
    };
    public static bool pressed(string button){
        if (DebugConsole.isActive) return false;

        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (Input.GetKeyDown(Inputs.inputs[index])) return true;

        else return false;

    }
    public static bool pressedDpad(){
        return pressed("up") || pressed("down") || pressed("left") || pressed("right");
    }
    public static void disableDpad(){
        Disable("up");
        Disable("down");
        Disable("left");
        Disable("right");

    }
     public static void enableDpad(){
        Enable("up");
        Enable("down");
        Enable("left");
        Enable("right");

    }
    public static bool held(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (Input.GetKey(Inputs.inputs[index])) return true;

        else return false;

    }
    public static bool released(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (Input.GetKeyUp(Inputs.inputs[index])) return true;
        
        else return false;

    }
}

//Class containing all the core data of the game.
public class GameData  {
    public static List<string> fieldMoves = new List<string>(new string[]{"Teleport","Fly","Cut","Surf","Dig","Strength","Flash","Softboiled"});
     public static List<Pokemon> party = new List<Pokemon>();
    public static Sprite[] frontMonSprites, backMonSprites;
    public static bool isPaused;
    public static void Init()
    {
        frontMonSprites = Resources.LoadAll<Sprite>("frontmon");
        backMonSprites = Resources.LoadAll<Sprite>("backmon");
        if(pokedexlist.Count != 151){

            pokedexlist.Clear();
            for (int i = 0; i < 151; i++){
                pokedexlist.Add(new PokedexEntry(false, false));
            }
        }
        if (playerName == null) playerName = "RED";
    }
    public static List<PokedexEntry> pokedexlist = new List<PokedexEntry>(151);
    public static bool[] hasBadge = new bool[8];
    public static int money;
    public static int coins;
    public static int trainerID;
    public static int textChoice, animationChoice, battleChoice;
    public static string playerName, rivalName;
    public const int mapWidth = 400;
    public const int mapHeight = 520;
    public static int screenTileWidth, screenTileHeight;
    public static int hours, minutes, seconds;
}

