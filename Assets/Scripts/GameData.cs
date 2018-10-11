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
        if (JoyconManager.Instance != null && JoyconManager.Instance.j.Count == 2)
        {
            int leftIndex = JoyconManager.Instance.j[0].isLeft ? 0 : 1;
            leftJoycon = JoyconManager.Instance.j[leftIndex];
            rightJoycon = JoyconManager.Instance.j[leftIndex ^ 1];
            joyconsConnected = true;
        }
        else joyconsConnected = false;

    }
   
    public static Joycon leftJoycon;
    public static Joycon rightJoycon;
    public static bool joyconsConnected;
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
        else if(joyconsConnected){
            switch(index){
                case 0: return leftJoycon.GetButtonDown(Joycon.Button.DPAD_UP);
                case 1: return leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN);
                case 2: return leftJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT);
                case 3: return leftJoycon.GetButtonDown(Joycon.Button.DPAD_RIGHT);
                case 4: return rightJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN);
                case 5: return rightJoycon.GetButtonDown(Joycon.Button.DPAD_RIGHT);
                case 6: return rightJoycon.GetButtonDown(Joycon.Button.PLUS);
                case 7: return leftJoycon.GetButtonDown(Joycon.Button.MINUS);
                default: return false;
            }
        }
        else return false;

    }
    public static bool held(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (Input.GetKey(Inputs.inputs[index])) return true;

        else if (joyconsConnected)
        {
            switch (index)
            {
                case 0: return leftJoycon.GetButton(Joycon.Button.DPAD_UP);
                case 1: return leftJoycon.GetButton(Joycon.Button.DPAD_DOWN);
                case 2: return leftJoycon.GetButton(Joycon.Button.DPAD_LEFT);
                case 3: return leftJoycon.GetButton(Joycon.Button.DPAD_RIGHT);
                case 4: return rightJoycon.GetButton(Joycon.Button.DPAD_DOWN);
                case 5: return rightJoycon.GetButton(Joycon.Button.DPAD_RIGHT);
                case 6: return rightJoycon.GetButton(Joycon.Button.PLUS);
                case 7: return leftJoycon.GetButton(Joycon.Button.MINUS);
                default: return false;
            }
        }
        else return false;

    }
    public static bool released(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (buttonDisabled[index]) return false;
        if (index == 6 && dialogueCheck) return false;
        if (Input.GetKeyUp(Inputs.inputs[index])) return true;
        else if (joyconsConnected)
        {
            switch (index)
            {
                case 0: return leftJoycon.GetButtonUp(Joycon.Button.DPAD_UP);
                case 1: return leftJoycon.GetButtonUp(Joycon.Button.DPAD_DOWN);
                case 2: return leftJoycon.GetButtonUp(Joycon.Button.DPAD_LEFT);
                case 3: return leftJoycon.GetButtonUp(Joycon.Button.DPAD_RIGHT);
                case 4: return rightJoycon.GetButtonUp(Joycon.Button.DPAD_DOWN);
                case 5: return rightJoycon.GetButtonUp(Joycon.Button.DPAD_RIGHT);
                case 6: return rightJoycon.GetButtonUp(Joycon.Button.PLUS);
                case 7: return leftJoycon.GetButtonUp(Joycon.Button.MINUS);
                default: return false;
            }
        }
        else return false;

    }
}
public class GameConstants{
    public const int mapWidth = 400;
    public const int mapHeight = 520;
}
//Class containing all the core data of the game.
public class GameData  {
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
}
