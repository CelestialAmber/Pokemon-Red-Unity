using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PokemonDataCombiner : MonoBehaviour
{
    public string[] PokemonNames = {
    "BULBASAUR",
    "IVYSAUR",
    "VENUSAUR",
    "CHARMANDER",
    "CHARMELEON",
    "CHARIZARD",
    "SQUIRTLE",
    "WARTORTLE",
    "BLASTOISE",
    "CATERPIE",
    "METAPOD",
    "BUTTERFREE",
    "WEEDLE",
    "KAKUNA",
    "BEEDRILL",
    "PIDGEY",
    "PIDGEOTTO",
    "PIDGEOT",
    "RATTATA",
    "RATICATE",
    "SPEAROW",
    "FEAROW",
    "EKANS",
    "ARBOK",
    "PIKACHU",
    "RAICHU",
    "SANDSHREW",
    "SANDSLASH",
    "NIDORAN♀",
    "NIDORINA",
    "NIDOQUEEN",
    "NIDORAN♂",
    "NIDORINO",
    "NIDOKING",
    "CLEFAIRY",
    "CLEFABLE",
    "VULPIX",
    "NINETALES",
    "JIGGLYPUFF",
    "WIGGLYTUFF",
    "ZUBAT",
    "GOLBAT",
    "ODDISH",
    "GLOOM",
    "VILEPLUME",
    "PARAS",
    "PARASECT",
    "VENONAT",
    "VENOMOTH",
    "DIGLETT",
    "DUGTRIO",
    "MEOWTH",
    "PERSIAN",
    "PSYDUCK",
    "GOLDUCK",
    "MANKEY",
    "PRIMEAPE",
    "GROWLITHE",
    "ARCANINE",
    "POLIWAG",
    "POLIWHIRL",
    "POLIWRATH",
    "ABRA",
    "KADABRA",
    "ALAKAZAM",
    "MACHOP",
    "MACHOKE",
    "MACHAMP",
    "BELLSPROUT",
    "WEEPINBELL",
    "VICTREEBEL",
    "TENTACOOL",
    "TENTACRUEL",
    "GEODUDE",
    "GRAVELER",
    "GOLEM",
    "PONYTA",
    "RAPIDASH",
    "SLOWPOKE",
    "SLOWBRO",
    "MAGNEMITE",
    "MAGNETON",
    "FARFETCH'D",
    "DODUO",
    "DODRIO",
    "SEEL",
    "DEWGONG",
    "GRIMER",
    "MUK",
    "SHELLDER",
    "CLOYSTER",
    "GASTLY",
    "HAUNTER",
    "GENGAR",
    "ONIX",
    "DROWZEE",
    "HYPNO",
    "KRABBY",
    "KINGLER",
    "VOLTORB",
    "ELECTRODE",
    "EXEGGCUTE",
    "EXEGGUTOR",
    "CUBONE",
    "MAROWAK",
    "HITMONLEE",
    "HITMONCHAN",
    "LICKITUNG",
    "KOFFING",
    "WEEZING",
    "RHYHORN",
    "RHYDON",
    "CHANSEY",
    "TANGELA",
    "KANGASKHAN",
    "HORSEA",
    "SEADRA",
    "GOLDEEN",
    "SEAKING",
    "STARYU",
    "STARMIE",
    "MR. MIME",
    "SCYTHER",
    "JYNX",
    "ELECTABUZZ",
    "MAGMAR",
    "PINSIR",
    "TAUROS",
    "MAGIKARP",
    "GYARADOS",
    "LAPRAS",
    "DITTO",
    "EEVEE",
    "VAPOREON",
    "JOLTEON",
    "FLAREON",
    "PORYGON",
    "OMANYTE",
    "OMASTAR",
    "KABUTO",
    "KABUTOPS",
    "AERODACTYL",
    "SNORLAX",
    "ARTICUNO",
    "ZAPDOS",
    "MOLTRES",
    "DRATINI",
    "DRAGONAIR",
    "DRAGONITE",
    "MEWTWO",
    "MEW",
    "MISSINGNO."
    };

    public int[] BaseExp = {64,141,208,65,142,209,66,143,210,53,72,160,52,71,159,55,113,172,57,116,58,162,62,147,82,122,93,163,59,117,194,60,118,195,68,129,63,178,76,109,54,171,78,132,184,70,128,75,138,81,153,69,148,80,174,74,149,91,213,77,131,185,73,145,186,88,146,193,84,151,191,105,205,86,134,177,152,192,99,164,89,161,94,96,158,100,176,90,157,97,203,95,126,190,108,102,165,115,206,103,150,98,212,87,124,139,140,127,114,173,135,204,255,166,175,83,155,111,170,106,207,136,187,137,156,167,200,211,20,214,219,61,92,196,197,198,130,120,199,119,201,202,154,215,216,217,67,144,218,220,64,143};



    public void CombinePokemonData(){
        Dictionary<Types,Dictionary<Types,float>> TypeEffectiveness = Serializer.JSONtoObject<Dictionary<Types,Dictionary<Types,float>>>("typeEffectiveness.json");
        Dictionary<PokemonEnum,Tuple<PokemonEnum,int>> evolutionData = Serializer.JSONtoObject<Dictionary<PokemonEnum,Tuple<PokemonEnum,int>>>("evolutiondata.json");
        Dictionary<PokemonEnum,int[]> baseStats = Serializer.JSONtoObject<Dictionary<PokemonEnum,int[]>>("basestatsdata.json");
        Dictionary<PokemonEnum, Tuple<Moves,int>[]> levelmovesData = Serializer.JSONtoObject<Dictionary<PokemonEnum, Tuple<Moves,int>[]>>("levelmovesdata.json");
        Dictionary<PokemonEnum,Moves[]> learnbytm = Serializer.JSONtoObject<Dictionary<PokemonEnum,Moves[]>>("learnbytmdata.json");
        Dictionary<PokemonEnum, int> PokemonPartySprite = Serializer.JSONtoObject<Dictionary<PokemonEnum, int>>("partySpriteData.json");
        Dictionary<PokemonEnum, Types[]> PokemonTypes = Serializer.JSONtoObject<Dictionary<PokemonEnum, Types[]>>("pokemonTypeData.json");
        Dictionary<PokemonEnum, int> PokemonExpGroup = Serializer.JSONtoObject<Dictionary<PokemonEnum, int>>("expGroupData.json");

        PokemonDataEntry[] newPokemonData = new PokemonDataEntry[152];

        for(int i = 0; i < 152; i++){
            PokemonDataEntry entry = new PokemonDataEntry();
            int id = i + 1;

            entry.name = PokemonNames[i];
            entry.id = id;
            entry.partySprite = PokemonPartySprite[(PokemonEnum)id];
            entry.baseStats = baseStats[(PokemonEnum)id];
            entry.baseExp = BaseExp[i];
            entry.expGroup = PokemonExpGroup[(PokemonEnum)id];
            Tuple<PokemonEnum,int> evolution = evolutionData[(PokemonEnum)id];
            entry.evolution = new PokemonEvolution(evolution.Item1, evolution.Item2);
            entry.types = PokemonTypes[(PokemonEnum)id];

            Moves[] tmhmlearnset = learnbytm[(PokemonEnum)id];
            int[] learnsetNew = new int[tmhmlearnset.Length];

            for(int j = 0; j < tmhmlearnset.Length; j++){
                learnsetNew[j] = Array.IndexOf(PokemonData.TMHMMoves,tmhmlearnset[j]) + 1;
            }

            entry.tmhmLearnset = learnsetNew;

            Tuple<Moves, int>[] levelmoves = levelmovesData[(PokemonEnum)id];
            entry.levelupLearnset = new LevelUpMove[levelmoves.Length];
            
            for(int j = 0; j < levelmoves.Length; j++){
                entry.levelupLearnset[j] = new LevelUpMove(levelmoves[j].Item1, levelmoves[j].Item2);
            }

            Debug.Log(entry.name);

            newPokemonData[i] = entry;
        }

        Serializer.objectToJSON("pokemonDataNew.json", newPokemonData);
    }


    public void TestPokemonEnumToJSON(){
         Dictionary<PokemonEnum,Tuple<PokemonEnum,int>> evolutionTest  = new Dictionary<PokemonEnum, Tuple<PokemonEnum, int>>();
         evolutionTest.Add(PokemonEnum.Bulbasaur, new Tuple<PokemonEnum, int>(PokemonEnum.Ivysaur,16));
         Serializer.objectToJSON("evolutiontest.json", evolutionTest);
    }

}

