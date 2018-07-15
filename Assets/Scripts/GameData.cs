﻿using System.Collections;
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
    public GameObject fetchObject(string path){
        return GameObject.Find(path);
    }
     
    public  ViewBio Bio(){
            return fetchObject("UI/Menus/BioMenu").GetComponent<ViewBio>();
        
    }
    public  Pokedex pokedex(){
            return fetchObject("UI/Menus/Pokedex").GetComponent<Pokedex>();
    }
    public  bag Bag()
    {

            return fetchObject("UI/Menus/Bag").GetComponent<bag>();

    }
    public MainMenu menu()
    {

        return fetchObject("UI/Menus/StartMenu").GetComponent<MainMenu>();

    }
     public Player player()
    {

        return fetchObject("Player").GetComponent<Player>();

    }
    public PokemonData pokemondata()
    {
        return fetchObject("UI/Menus/PokemonMenu").GetComponent<PokemonData>();
    }
    public MapEditor mapEditor
    {
        get
        {
            return fetchObject("MapEditor").GetComponent<MapEditor>();
        }
    }
}

public class Inputs
{

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
            if (Input.GetKeyDown(Inputs.inputs[index])) return true;
        else return false;

    }
    public static bool held(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (Input.GetKey(Inputs.inputs[index])) return true;
        else return false;

    }
    public static bool released(string button)
    {
        if (DebugConsole.isActive) return false;
        int index = Inputs.keyindices[button];
        if (Input.GetKeyUp(Inputs.inputs[index])) return true;
        else return false;

    }
}
//Class containing all the save data of the game.
public class SaveData  {
    public static void Init()
    {
        if(pokedexlist.Count != 152){

            pokedexlist.Clear();
            for (int i = 0; i < 152; i++){
                pokedexlist.Add(new PokedexEntry(false, false));
            }
        }
    }
    public static List<PokedexEntry> pokedexlist = new List<PokedexEntry>(152);
    public static bool[] hasBadge = new bool[8];
    public static int money;
    public static int coins;
    public static int trainerID;
}
