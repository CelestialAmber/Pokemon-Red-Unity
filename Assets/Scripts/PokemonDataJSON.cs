using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.IO;
using System.Net;
using System;
using System.Text;
using Newtonsoft.Json;
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
    public static List<MoveData> moves = new List<MoveData>(
        new MoveData[]{
            new MoveData("Pound",40,"Normal",100,35,"noEffect"),
        new MoveData("Karate Chop",50,"Normal",100,25,"noEffect"),
        new MoveData("Doubleslap",15,"Normal",85,10,"twoFiveEffect"),
        new MoveData("Comet Punch",18,"Normal",85,15,"twoFiveEffect"),
        new MoveData("Mega Punch",80,"Normal",85,20,"noEffect"),
        new MoveData("Pay Day",40,"Normal",100,20,"payDayEffect"),
        new MoveData("Fire Punch",75,"Fire",100,15,"burnSideEffect1"),
        new MoveData("Ice Punch",75,"Ice",100,15,"freezeSideEffect"),
        new MoveData("Thunderpunch",75,"Electric",100,15,"paralyzeSideEffect1"),
        new MoveData("Scratch",40,"Normal",100,35,"noEffect"),
        new MoveData("Vicegrip",55,"Normal",100,30,"noEffect"),
        new MoveData("Guillotine",1,"Normal",30,5,"ohkoEffect"),
        new MoveData("Razor Wind",80,"Normal",75,10,"chargeEffect"),
        new MoveData("Swords Dance",0,"Normal",100,30,"attackUp2Effect"),
        new MoveData("Cut",50,"Normal",95,30,"noEffect"),
        new MoveData("Gust",40,"Normal",100,35,"noEffect"),
        new MoveData("Wing Attack",35,"Flying",100,35,"noEffect"),
        new MoveData("Whirlwind",0,"Normal",85,20,"switchTeleportEffect"),
        new MoveData("Fly",70,"Flying",95,15,"flyEffect"),
        new MoveData("Bind",15,"Normal",75,20,"trappingEffect"),
        new MoveData("Slam",80,"Normal",75,20,"noEffect"),
        new MoveData("Vine Whip",35,"Grass",100,10,"noEffect"),
        new MoveData("Stomp",65,"Normal",100,20,"flinchSideEffect2"),
        new MoveData("Double Kick",30,"Fighting",100,30,"doubleAttackEffect"),
        new MoveData("Mega Kick",120,"Normal",75,5,"noEffect"),
        new MoveData("Jump Kick",70,"Fighting",95,25,"jumpKickEffect"),
        new MoveData("Rolling Kick",60,"Fighting",85,15,"flinchSideEffect2"),
        new MoveData("Sand-Attack",0,"Normal",100,15,"accuracyDown1Effect"),
        new MoveData("Headbutt",70,"Normal",100,15,"flinchSideEffect2"),
        new MoveData("Horn Attack",65,"Normal",100,25,"noEffect"),
        new MoveData("Fury Attack",15,"Normal",85,20,"twoFiveEffect"),
        new MoveData("Horn Drill",1,"Normal",30,5,"ohkoEffect"),
        new MoveData("Tackle",35,"Normal",95,35,"noEffect"),
        new MoveData("Body Slam",85,"Normal",95,35,"paralyzeSideEffect2"),
        new MoveData("Wrap",15,"Normal",85,20,"trappingEffect"),
        new MoveData("Take Down",90,"Normal",85,20,"recoilEffect"),
        new MoveData("Thrash",90,"Normal",85,20,"thrashEffect"),
        new MoveData("Double-Edge",100,"Normal",100,15,"recoilEffect"),
        new MoveData("Tail Whip",0,"Normal",100,30,"defenseDown1Effect"),
        new MoveData("Poison Sting",15,"Poison",100,35,"poisonSideEffect1"),
        new MoveData("Twineedle",25,"Bug",100,20,"twinNeedleEffect"),
        new MoveData("Pin Missle",14,"Bug",85,20,"twoFiveEffect"),
        new MoveData("Leer",0,"Normal",100,30,"defenseDown1Effect"),
        new MoveData("Bite",60,"Normal",100,25,"flinchSideEffect1"),
        new MoveData("Growl",0,"Normal",100,40,"attackDown1Effect"),
        new MoveData("Roar",0,"Normal",100,20,"switchTeleportEffect"),
        new MoveData("Sing",0,"Normal",55,15,"sleepEffect"),
        new MoveData("Supersonic",0,"Normal",55,20,"confusionEffect"),
        new MoveData("Sonicboom",1,"Normal",90,20,"specialDamageEffect"),
        new MoveData("Disable",0,"Normal",55,20,"disableEffect"),
        new MoveData("Acid",40,"Poison",100,30,"defenseDownSideEffect"),
        new MoveData("Ember",40,"Fire",100,25,"burnSideEffect1"),
        new MoveData("Flamethrower",95,"Fire",100,15,"burnSideEffect1"),
        new MoveData("Mist",0,"Ice",100,30,"mistEffect"),
        new MoveData("Water Gun",40,"Water",100,25,"noEffect"),
        new MoveData("Hydro Pump",120,"Water",80,5,"noEffect"),
        new MoveData("Surf",95,"Water",100,15,"noEffect"),
        new MoveData("Ice Beam",95,"Ice",100,10,"freezeSideEffect"),
        new MoveData("Blizzard",120,"Ice",90,5,"freezeSideEffect"),
        new MoveData("Psybeam",65,"Psychic",100,20,"confusionSideEffect"),
        new MoveData("Bubblebeam",65,"Water",100,20,"speedDownSideEffect"),
        new MoveData("Aurora Beam", 65, "Ice",100,20,"attackDownSideEffect"),
        new MoveData("Hyper Beam",150,"Normal",90,5,"hyperBeamEffect"),
        new MoveData("Peck",35,"Flying",100,35,"noEffect"),
        new MoveData("Drill Peck",80,"Flying",100,20,"noEffect"),
        new MoveData("Submission",80,"Fighting",80,25,"recoilEffect"),
        new MoveData("Low Kick",50,"Fighting",90,20,"flinchSideEffect2"),
        new MoveData("Counter",1,"Fighting",100,20,"noEffect"),
        new MoveData("Seismic Toss",1,"Fighting",100,20,"specialDamageEffect"),
        new MoveData("Strength",80,"Normal",100,15,"noEffect"),
        new MoveData("Absorb",20,"Grass",100,20,"drainHpEffect"),
        new MoveData("Mega Drain",40,"Grass",100,10,"drainHpEffect"),
        new MoveData("Leech Seed",0,"Grass",90,10,"leechSeedEffect"),
        new MoveData("Growth",0,"Normal",100,40,"specialUp1Effect"),
        new MoveData("Razor Leaf",55,"Grass",95,25,"noEffect"),
        new MoveData("Solarbeam",120,"Grass",100,10,"chargeEffect"),
        new MoveData("Poisonpowder",0,"Poison",75,35,"poisonEffect"),
        new MoveData("Stun Spore",0,"Grass",75,30,"paralyzeEffect"),
        new MoveData("Sleep Powder",0,"Grass",75,15,"sleepEffect"),
        new MoveData("Petal Dance",70,"Grass",100,20,"thrashEffect"),
        new MoveData("String Shot",0,"Bug",95,40,"speedDown1Effect"),
        new MoveData("Dragon Rage",1,"Dragon",100,10,"specialDamageEffect"),
        new MoveData("Fire Spin",15,"Fire",70,15,"trappingEffect"),
        new MoveData("Thundershock",40,"Electric",100,30,"paralyzeSideEffect1"),
        new MoveData("Thunderbolt",95,"Electric",100,15,"paralyzeSideEffect1"),
        new MoveData("Thunder Wave",0,"Electric",100,20,"paralyzeEffect"),
        new MoveData("Thunder",120,"Electric",70,10,"paralyzeSideEffect1"),
        new MoveData("Rock Throw",50,"Rock",65,15,"noEffect"),
        new MoveData("Earthquake",100,"Ground",100,10,"noEffect"),
        new MoveData("Fissure",1,"Ground",30,5,"ohkoEffect"),
        new MoveData("Dig",100,"Ground",100,10,"chargeEffect"),
        new MoveData("Toxic",0,"Poison",85,10,"poisionEffect"),
        new MoveData("Confusion",50,"Psychic",100,25,"confusionSideEffect"),
        new MoveData("Psychic",90,"Psychic",100,10,"specialDownSideEffect"),
        new MoveData("Hypnosis",0,"Psychic",60,20,"sleepEffect"),
        new MoveData("Meditate",0,"Psychic",100,40,"attackUp1Effect"),
        new MoveData("Agility",0,"Psychic",100,30,"speedUp2Effect"),
        new MoveData("Quick Attack",40,"Normal",100,30,"noEffect"),
        new MoveData("Rage",20,"Normal",100,20,"rageEffect"),
        new MoveData("Teleport",0,"Psychic",100,20,"switchTeleportEffect"),
        new MoveData("Night Shade",0,"Ghost",100,15,"specialDamageEffect"),
        new MoveData("Mimic",0,"Normal",100,10,"mimicEffect"),
        new MoveData("Screech",0,"Normal",85,40,"defenseDown2Effect"),
        new MoveData("Double Team",0,"Normal",100,45,"evasionUp1Effect"),
        new MoveData("Recover",0,"Normal",100,20,"healEffect"),
        new MoveData("Harden",0,"Normal",100,30,"defenseUp1Effect"),
        new MoveData("Minimize",0,"Normal",100,20,"evasionUp1Effect"),
        new MoveData("Smokescreen",0,"Normal",100,20,"accuracyDown1Effect"),
        new MoveData("Confuse Ray",0,"Ghost",100,10,"confusionEffect"),
        new MoveData("Withdraw",0,"Water",100,40,"defenseUp1Effect"),
        new MoveData("Defense Curl",0,"Normal",100,40,"defenseUp1Effect"),
        new MoveData("Barrier",0,"Psychic",100,30,"defenseUp2Effect"),
        new MoveData("Light Screen",0,"Psychic",100,30,"lightScreenEffect"),
        new MoveData("Haze",0,"Ice",100,30,"hazeEffect"),
        new MoveData("Reflect",0,"Psychic",100,20,"reflectEffect"),
        new MoveData("Focus Energy",0,"Normal",100,30,"focusEffect"),
        new MoveData("Bide",0,"Normal",100,10,"bideEffect"),
        new MoveData("Metronome",0,"Normal",100,10,"metronomeEffect"),
        new MoveData("Mirror Move",0,"Flying",100,20,"mirrorMoveEffect"),
        new MoveData("Selfdestruct",130,"Normal",100,5,"explodeEffect"),
        new MoveData("Egg Bomb",100,"Normal",75,10,"noEffect"),
        new MoveData("Lick",20,"Ghost",100,30,"paralyzeSideEffect2"),
        new MoveData("Smog",20,"Poison",70,20,"poisonSideEffect2"),
        new MoveData("Sludge",65,"Poison",100,20,"poisonSideEffect2"),
        new MoveData("Bone Club",65,"Ground",85,20,"flinchSideEffect1"),
        new MoveData("Fire Blast",120,"Fire",85,5,"burnSideEffect2"),
        new MoveData("Waterfall",80,"Water",100,15,"noEffect"),
        new MoveData("Clamp",35,"Water",75,10,"trappingEffect"),
        new MoveData("Swift",60,"Normal",100,20,"swiftEffect"),
        new MoveData("Skull Bash",100,"Normal",100,15,"chargeEffect"),
        new MoveData("Spike Cannon",20,"Normal",100,15,"twoFiveEffect"),
        new MoveData("Constrict",10,"Normal",100,35,"trappingEffect"),
        new MoveData("Amnesia",0,"Psychic",100,20,"specialUp2Effect"),
        new MoveData("Kinesis",0,"Psychic",80,15,"accuracyDown1Effect"),
        new MoveData("Softboiled",0,"Normal",100,10,"healEffect"),
        new MoveData("Hi Jump Kick",85,"Fighting",90,20,"jumpKickEffect"),
        new MoveData("Glare",0,"Normal",75,30,"paralyzeEffect"),
        new MoveData("Dream Eater",100,"Psychic",100,15,"dreamEaterEffect"),
        new MoveData("Poison Gas",0,"Poison",55,40,"poisonEffect"),
        new MoveData("Barrage",15,"Normal",85,20,"twoFiveEffect"),
        new MoveData("Leech Life",20,"Bug",100,15,"drainHpEffect"),
        new MoveData("Lovely Kiss",0,"Normal",75,10,"sleepEffect"),
        new MoveData("Sky Attack",140,"Flying",90,5,"chargeEffect"),
        new MoveData("Transform",0,"Normal",100,10,"transformEffect"),
        new MoveData("Bubble",20,"Water",100,30,"speedDownSideEffect"),
        new MoveData("Dizzy Punch",70,"Normal",100,10,"noEffect"),
        new MoveData("Spore",0,"Grass",100,15,"sleepEffect"),
        new MoveData("Flash",0,"Normal",70,20,"accuracyDown1Effect"),
        new MoveData("Psywave",1,"Psychic",80,15,"specialDamageEffect"),
        new MoveData("Splash",0,"Normal",100,40,"splashEffect"),
        new MoveData("Acid Armor",0,"Poison",100,40,"defenseUp2Effect"),
        new MoveData("Crabhammer",90,"Water",85,10,"noEffect"),
        new MoveData("Explosion",170,"Normal",100,5,"explodeEffect"),
        new MoveData("Fury Swipes",18,"Normal",80,15,"twoFiveEffect"),
        new MoveData("Bonemerang",50,"Ground",90,10,"doubleAttackEffect"),
        new MoveData("Rest",0,"Psychic",100,10,"healEffect"),
        new MoveData("Rock Slide",75,"Rock",90,10,"noEffect"),
        new MoveData("Hyper Fang",80,"Normal",90,15,"flinchSideEffect1"),
        new MoveData("Sharpen",0,"Normal",100,30,"attackUp1Effect"),
        new MoveData("Conversion",0,"Normal",100,30,"conversionEffect"),
        new MoveData("Tri Attack",80,"Normal",100,10,"noEffect"),
        new MoveData("Super Fang",1,"Normal",90,10,"superFangEffect"),
        new MoveData("Slash",70,"Normal",100,20,"noEffect"),
        new MoveData("Substitute",0,"Normal",100,10,"substituteEffect"),
        new MoveData("Struggle",50,"Normal",100,10,"recoilEffect")
    });
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
    public static Dictionary<string, int> PokemonPartySprite = new Dictionary<string, int>(){
        {"Bulbasaur",4},
        {"Ivysaur",4},
        {"Venusaur",4},
        {"Charmander",0},
        {"Charmeleon",0},
        {"Charizard",0},
        {"Squirtle",2},
        {"Wartortle",2},
        {"Blastoise",2},
        {"Caterpie",5},
        {"Metapod",5},
        {"Butterfree",5},
        {"Weedle",5},
        {"Kakuna",5},
        {"Beedrill",5},
        {"Pidgey",1},
        {"Pidgeotto",1},
        {"Pidgeot",1},
        {"Rattata",7},
        {"Raticate",7},
        {"Spearow",1},
        {"Fearow",1},
        {"Ekans",6},
        {"Arbok",6},
        {"Pikachu",3},
        {"Raichu",3},
        {"Sandshrew",0},
        {"Sandslash",0},
        {"Nidoran♀",0},
        {"Nidorina",0},
        {"Nidoqueen",0},
        {"Nidoran♂",0},
        {"Nidorino",0},
        {"Nidoking",0},
        {"Clefairy",3},
        {"Clefable",3},
        {"Vulpix",7},
        {"Ninetales",7},
        {"Jigglypuff",3},
        {"Wigglytuff",3},
        {"Zubat",0},
        {"Golbat",0},
        {"Oddish",4},
        {"Gloom",4},
        {"Vileplume",4},
        {"Paras",5},
        {"Parasect",5},
        {"Venonat",5},
        {"Venomoth",5},
        {"Diglett",0},
        {"Dugtrio",0},
        {"Meowth",0},
        {"Persian",0},
        {"Psyduck",0},
        {"Golduck",0},
        {"Mankey",0},
        {"Primeape",0},
        {"Growlithe",7},
        {"Arcanine",7},
        {"Poliwag",0},
        {"Poliwhirl",0},
        {"Poliwrath",0},
        {"Abra",0},
        {"Kadabra",0},
        {"Alakazam",0},
        {"Machop",0},
        {"Machoke",0},
        {"Machamp",0},
        {"Bellsprout",4},
        {"Weepinbell",4},
        {"Victreebel",4},
        {"Tentacool",2},
        {"Tentacruel",2},
        {"Geodude",0},
        {"Graveler",0},
        {"Golem",0},
        {"Ponyta",7},
        {"Rapidash",7},
        {"Slowpoke",7},
        {"Slowbro",0},
        {"Magnemite",8},
        {"Magneton",8},
        {"Farfetch'd",1},
        {"Doduo",1},
        {"Dodrio",1},
        {"Seel",2},
        {"Dewgong",2},
        {"Grimer",0},
        {"Muk",0},
        {"Shellder",9},
        {"Cloyster",9},
        {"Gastly",0},
        {"Haunter",0},
        {"Gengar",0},
        {"Onix",6},
        {"Drowzee",0},
        {"Hypno",0},
        {"Krabby",2},
        {"Kingler",2},
        {"Voltorb",8},
        {"Electrode",8},
        {"Exeggcute",4},
        {"Exeggutor",4},
        {"Cubone",0},
        {"Marowak",0},
        {"Hitmonlee",0},
        {"Hitmonchan",0},
        {"Lickitung",0},
        {"Koffing",0},
        {"Weezing",0},
        {"Rhyhorn",7},
        {"Rhydon",0},
        {"Chansey",3},
        {"Tangela",4},
        {"Kangaskhan",0},
        {"Horsea",2},
        {"Seadra",2},
        {"Goldeen",2},
        {"Seaking",2},
        {"Staryu",9},
        {"Starmie",9},
        {"Mr. Mime",0},
        {"Scyther",5},
        {"Jynx",0},
        {"Electabuzz",0},
        {"Magmar",0},
        {"Pinsir",5},
        {"Tauros",7},
        {"Magikarp",2},
        {"Gyarados",6},
        {"Lapras",2},
        {"Ditto",0},
        {"Eevee",7},
        {"Vaporeon",7},
        {"Jolteon",7},
        {"Flareon",7},
        {"Porygon",0},
        {"Omanyte",9},
        {"Omastar",9},
        {"Kabuto",9},
        {"Kabutops",9},
        {"Aerodactyl",1},
        {"Snorlax",0},
        {"Articuno",1},
        {"Zapdos",1},
        {"Moltres",1},
        {"Dratini",6},
        {"Dragonair",6},
        {"Dragonite",6},
        {"Mewtwo",0},
        {"Mew",0},
        {"Linq",10}
    };
    public static Dictionary<string, string[]> PokemonTypes = new Dictionary<string, string[]>(){
        {"Bulbasaur",new string[]{"Grass","Poison"}},
        {"Ivysaur",new string[]{"Grass","Poison"}},
        {"Venusaur",new string[]{"Grass","Poison"}},
        {"Charmander",new string[]{"Fire",""}},
        {"Charmeleon",new string[]{"Fire",""}},
        {"Charizard",new string[]{"Fire","Flying"}},
        {"Squirtle",new string[]{"Water",""}},
        {"Wartortle",new string[]{"Water",""}},
        {"Blastoise",new string[]{"Water",""}},
        {"Caterpie",new string[]{"Bug",""}},
        {"Metapod",new string[]{"Bug",""}},
        {"Butterfree",new string[]{"Bug","Flying"}},
        {"Weedle",new string[]{"Bug","Poison"}},
        {"Kakuna",new string[]{"Bug","Poison"}},
        {"Beedrill",new string[]{"Bug","Poison"}},
        {"Pidgey",new string[]{"Normal","Flying"}},
        {"Pidgeotto",new string[]{"Normal","Flying"}},
        {"Pidgeot",new string[]{"Normal","Flying"}},
        {"Rattata",new string[]{"Normal",""}},
        {"Raticate",new string[]{"Normal",""}},
        {"Spearow",new string[]{"Normal","Flying"}},
        {"Fearow",new string[]{"Normal","Flying"}},
        {"Ekans",new string[]{"Poison",""}},
        {"Arbok",new string[]{"Poison",""}},
        {"Pikachu",new string[]{"Electric",""}},
        {"Raichu",new string[]{"Electric",""}},
        {"Sandshrew",new string[]{"Ground",""}},
        {"Sandslash",new string[]{"Ground",""}},
        {"Nidoran♀",new string[]{"Poison",""}},
        {"Nidorina",new string[]{"Poison",""}},
        {"Nidoqueen",new string[]{"Poison","Ground"}},
        {"Nidoran♂",new string[]{"Poison",""}},
        {"Nidorino",new string[]{"Poison",""}},
        {"Nidoking",new string[]{"Poison","Ground"}},
        {"Clefairy",new string[]{"Normal",""}},
        {"Clefable",new string[]{"Normal",""}},
        {"Vulpix",new string[]{"Fire",""}},
        {"Ninetales",new string[]{"Fire",""}},
        {"Jigglypuff",new string[]{"Normal",""}},
        {"Wigglytuff",new string[]{"Normal",""}},
        {"Zubat",new string[]{"Poison","Flying"}},
        {"Golbat",new string[]{"Poison","Flying"}},
        {"Oddish",new string[]{"Grass","Poison"}},
        {"Gloom",new string[]{"Grass","Poison"}},
        {"Vileplume",new string[]{"Grass","Poison"}},
        {"Paras",new string[]{"Bug","Grass"}},
        {"Parasect",new string[]{"Bug","Grass"}},
        {"Venonat",new string[]{"Bug","Poison"}},
        {"Venomoth",new string[]{"Bug","Poison"}},
        {"Diglett",new string[]{"Ground",""}},
        {"Dugtrio",new string[]{"Ground",""}},
        {"Meowth",new string[]{"Normal",""}},
        {"Persian",new string[]{"Normal",""}},
        {"Psyduck",new string[]{"Water",""}},
        {"Golduck",new string[]{"Water",""}},
        {"Mankey",new string[]{"Fighting",""}},
        {"Primeape",new string[]{"Fighting",""}},
        {"Growlithe",new string[]{"Fire",""}},
        {"Arcanine",new string[]{"Fire",""}},
        {"Poliwag",new string[]{"Water",""}},
        {"Poliwhirl",new string[]{"Water",""}},
        {"Poliwrath",new string[]{"Water","Fighting"}},
        {"Abra",new string[]{"Psychic",""}},
        {"Kadabra",new string[]{"Psychic",""}},
        {"Alakazam",new string[]{"Psychic",""}},
        {"Machop",new string[]{"Fighting",""}},
        {"Machoke",new string[]{"Fighting",""}},
        {"Machamp",new string[]{"Fighting",""}},
        {"Bellsprout",new string[]{"Grass","Poison"}},
        {"Weepinbell",new string[]{"Grass","Poison"}},
        {"Victreebel",new string[]{"Grass","Poison"}},
        {"Tentacool",new string[]{"Water","Poison"}},
        {"Tentacruel",new string[]{"Water","Poison"}},
        {"Geodude",new string[]{"Rock","Ground"}},
        {"Graveler",new string[]{"Rock","Ground"}},
        {"Golem",new string[]{"Rock","Ground"}},
        {"Ponyta",new string[]{"Fire",""}},
        {"Rapidash",new string[]{"Fire","Fire"}},
        {"Slowpoke",new string[]{"Water","Psychic"}},
        {"Slowbro",new string[]{"Water","Psychic"}},
        {"Magnemite",new string[]{"Electric",""}},
        {"Magneton",new string[]{"Electric",""}},
        {"Farfetch'd",new string[]{"Normal","Flying"}},
        {"Doduo",new string[]{"Normal","Flying"}},
        {"Dodrio",new string[]{"Normal","Flying"}},
        {"Seel",new string[]{"Water",""}},
        {"Dewgong",new string[]{"Water","Ice"}},
        {"Grimer",new string[]{"Poison",""}},
        {"Muk",new string[]{"Poison",""}},
        {"Shellder",new string[]{"Water",""}},
        {"Cloyster",new string[]{"Water","Ice"}},
        {"Gastly",new string[]{"Ghost","Poison"}},
        {"Haunter",new string[]{"Ghost","Poison"}},
        {"Gengar",new string[]{"Ghost","Poison"}},
        {"Onix",new string[]{"Rock","Ground"}},
        {"Drowzee",new string[]{"Psychic",""}},
        {"Hypno",new string[]{"Psychic",""}},
        {"Krabby",new string[]{"Water",""}},
        {"Kingler",new string[]{"Water",""}},
        {"Voltorb",new string[]{"Electric",""}},
        {"Electrode",new string[]{"Electric",""}},
        {"Exeggcute",new string[]{"Grass","Psychic"}},
        {"Exeggutor",new string[]{"Grass","Psychic"}},
        {"Cubone",new string[]{"Ground",""}},
        {"Marowak",new string[]{"Ground",""}},
        {"Hitmonlee",new string[]{"Fighting",""}},
        {"Hitmonchan",new string[]{"Fighting",""}},
        {"Lickitung",new string[]{"Normal",""}},
        {"Koffing",new string[]{"Poison",""}},
        {"Weezing",new string[]{"Poison",""}},
        {"Rhyhorn",new string[]{"Ground","Rock"}},
        {"Rhydon",new string[]{"Ground","Rock"}},
        {"Chansey",new string[]{"Normal",""}},
        {"Tangela",new string[]{"Grass",""}},
        {"Kangaskhan",new string[]{"Normal",""}},
        {"Horsea",new string[]{"Water",""}},
        {"Seadra",new string[]{"Water",""}},
        {"Goldeen",new string[]{"Water",""}},
        {"Seaking",new string[]{"Water",""}},
        {"Staryu",new string[]{"Water",""}},
        {"Starmie",new string[]{"Water","Psychic"}},
        {"Mr. Mime",new string[]{"Psychic",""}},
        {"Scyther",new string[]{"Bug","Flying"}},
        {"Jynx",new string[]{"Ice","Psychic"}},
        {"Electabuzz",new string[]{"Electric",""}},
        {"Magmar",new string[]{"Fire",""}},
        {"Pinsir",new string[]{"Bug",""}},
        {"Tauros",new string[]{"Normal",""}},
        {"Magikarp",new string[]{"Water",""}},
        {"Gyarados",new string[]{"Water","Flying"}},
        {"Lapras",new string[]{"Water","Ice"}},
        {"Ditto",new string[]{"Normal",""}},
        {"Eevee",new string[]{"Normal",""}},
        {"Vaporeon",new string[]{"Water",""}},
        {"Jolteon",new string[]{"Electric",""}},
        {"Flareon",new string[]{"Fire",""}},
        {"Porygon",new string[]{"Normal",""}},
        {"Omanyte",new string[]{"Rock","Water"}},
        {"Omastar",new string[]{"Rock","Water"}},
        {"Kabuto",new string[]{"Rock","Water"}},
        {"Kabutops",new string[]{"Rock","Water"}},
        {"Aerodactyl",new string[]{"Rock","Flying"}},
        {"Snorlax",new string[]{"Normal",""}},
        {"Articuno",new string[]{"Ice","Flying"}},
        {"Zapdos",new string[]{"Electric","Flying"}},
        {"Moltres",new string[]{"Fire","Flying"}},
        {"Dratini",new string[]{"Dragon",""}},
        {"Dragonair",new string[]{"Dragon",""}},
        {"Dragonite",new string[]{"Dragon","Flying"}},
        {"Mewtwo",new string[]{"Psychic",""}},
        {"Mew",new string[]{"Psychic",""}},
        {"Linq",new string[]{"Normal","Psychic"}}
    };
    public static Dictionary<string, int> PokemonExpGroup = new Dictionary<string, int>(){
        {"Bulbasaur",1},
        {"Ivysaur",1},
        {"Venusaur",1},
        {"Charmander",1},
        {"Charmeleon",1},
        {"Charizard",1},
        {"Squirtle",1},
        {"Wartortle",1},
        {"Blastoise",1},
        {"Caterpie",2},
        {"Metapod",2},
        {"Butterfree",2},
        {"Weedle",2},
        {"Kakuna",2},
        {"Beedrill",2},
        {"Pidgey",1},
        {"Pidgeotto",1},
        {"Pidgeot",1},
        {"Rattata",2},
        {"Raticate",2},
        {"Spearow",2},
        {"Fearow",2},
        {"Ekans",2},
        {"Arbok",2},
        {"Pikachu",2},
        {"Raichu",2},
        {"Sandshrew",2},
        {"Sandslash",2},
        {"Nidoran♀",1},
        {"Nidorina",1},
        {"Nidoqueen",1},
        {"Nidoran♂",1},
        {"Nidorino",1},
        {"Nidoking",1},
        {"Clefairy",3},
        {"Clefable",3},
        {"Vulpix",2},
        {"Ninetales",2},
        {"Jigglypuff",3},
        {"Wigglytuff",3},
        {"Zubat",2},
        {"Golbat",2},
        {"Oddish",1},
        {"Gloom",1},
        {"Vileplume",1},
        {"Paras",2},
        {"Parasect",2},
        {"Venonat",2},
        {"Venomoth",2},
        {"Diglett",2},
        {"Dugtrio",2},
        {"Meowth",2},
        {"Persian",2},
        {"Psyduck",2},
        {"Golduck",2},
        {"Mankey",2},
        {"Primeape",2},
        {"Growlithe",0},
        {"Arcanine",0},
        {"Poliwag",1},
        {"Poliwhirl",1},
        {"Poliwrath",1},
        {"Abra",1},
        {"Kadabra",1},
        {"Alakazam",1},
        {"Machop",1},
        {"Machoke",1},
        {"Machamp",1},
        {"Bellsprout",1},
        {"Weepinbell",1},
        {"Victreebel",1},
        {"Tentacool",0},
        {"Tentacruel",0},
        {"Geodude",1},
        {"Graveler",1},
        {"Golem",1},
        {"Ponyta",2},
        {"Rapidash",2},
        {"Slowpoke",2},
        {"Slowbro",2},
        {"Magnemite",2},
        {"Magneton",2},
        {"Farfetch'd",2},
        {"Doduo",2},
        {"Dodrio",2},
        {"Seel",2},
        {"Dewgong",2},
        {"Grimer",2},
        {"Muk",2},
        {"Shellder",0},
        {"Cloyster",0},
        {"Gastly",1},
        {"Haunter",1},
        {"Gengar",1},
        {"Onix",2},
        {"Drowzee",2},
        {"Hypno",2},
        {"Krabby",2},
        {"Kingler",2},
        {"Voltorb",2},
        {"Electrode",2},
        {"Exeggcute",0},
        {"Exeggutor",0},
        {"Cubone",2},
        {"Marowak",2},
        {"Hitmonlee",2},
        {"Hitmonchan",2},
        {"Lickitung",2},
        {"Koffing",2},
        {"Weezing",2},
        {"Rhyhorn",0},
        {"Rhydon",0},
        {"Chansey",3},
        {"Tangela",2},
        {"Kangaskhan",2},
        {"Horsea",2},
        {"Seadra",2},
        {"Goldeen",2},
        {"Seaking",2},
        {"Staryu",0},
        {"Starmie",0},
        {"Mr. Mime",2},
        {"Scyther",2},
        {"Jynx",2},
        {"Electabuzz",2},
        {"Magmar",2},
        {"Pinsir",0},
        {"Tauros",0},
        {"Magikarp",0},
        {"Gyarados",0},
        {"Lapras",0},
        {"Ditto",2},
        {"Eevee",2},
        {"Vaporeon",2},
        {"Jolteon",2},
        {"Flareon",2},
        {"Porygon",2},
        {"Omanyte",2},
        {"Omastar",2},
        {"Kabuto",2},
        {"Kabutops",2},
        {"Aerodactyl",0},
        {"Snorlax",0},
        {"Articuno",0},
        {"Zapdos",0},
        {"Moltres",0},
        {"Dratini",0},
        {"Dragonair",0},
        {"Dragonite",0},
        {"Mewtwo",0},
        {"Mew",1},
        {"Linq",0}
    };
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
    public static Dictionary<string, int> PokemonToIndex = new Dictionary<string, int>(){
        {"Bulbasaur",1},
        {"Ivysaur",2},
        {"Venusaur",3},
        {"Charmander",4},
        {"Charmeleon",5},
        {"Charizard",6},
        {"Squirtle",7},
        {"Wartortle",8},
        {"Blastoise",9},
        {"Caterpie",10},
        {"Metapod",11},
        {"Butterfree",12},
        {"Weedle",13},
        {"Kakuna",14},
        {"Beedrill",15},
        {"Pidgey",16},
        {"Pidgeotto",17},
        {"Pidgeot",18},
        {"Rattata",19},
        {"Raticate",20},
        {"Spearow",21},
        {"Fearow",22},
        {"Ekans",23},
        {"Arbok",24},
        {"Pikachu",25},
        {"Raichu",26},
        {"Sandshrew",27},
        {"Sandslash",28},
        {"Nidoran♀",29},
        {"Nidorina",30},
        {"Nidoqueen",31},
        {"Nidoran♂",32},
        {"Nidorino",33},
        {"Nidoking",34},
        {"Clefairy",35},
        {"Clefable",36},
        {"Vulpix",37},
        {"Ninetales",38},
        {"Jigglypuff",39},
        {"Wigglytuff",40},
        {"Zubat",41},
        {"Golbat",42},
        {"Oddish",43},
        {"Gloom",44},
        {"Vileplume",45},
        {"Paras",46},
        {"Parasect",47},
        {"Venonat",48},
        {"Venomoth",49},
        {"Diglett",50},
        {"Dugtrio",51},
        {"Meowth",52},
        {"Persian",53},
        {"Psyduck",54},
        {"Golduck",55},
        {"Mankey",56},
        {"Primeape",57},
        {"Growlithe",58},
        {"Arcanine",59},
        {"Poliwag",60},
        {"Poliwhirl",61},
        {"Poliwrath",62},
        {"Abra",63},
        {"Kadabra",64},
        {"Alakazam",65},
        {"Machop",66},
        {"Machoke",67},
        {"Machamp",68},
        {"Bellsprout",69},
        {"Weepinbell",70},
        {"Victreebel",71},
        {"Tentacool",72},
        {"Tentacruel",73},
        {"Geodude",74},
        {"Graveler",75},
        {"Golem",76},
        {"Ponyta",77},
        {"Rapidash",78},
        {"Slowpoke",79},
        {"Slowbro",80},
        {"Magnemite",81},
        {"Magneton",82},
        {"Farfetch'd",83},
        {"Doduo",84},
        {"Dodrio",85},
        {"Seel",86},
        {"Dewgong",87},
        {"Grimer",88},
        {"Muk",89},
        {"Shellder",90},
        {"Cloyster",91},
        {"Gastly",92},
        {"Haunter",93},
        {"Gengar",94},
        {"Onix",95},
        {"Drowzee",96},
        {"Hypno",97},
        {"Krabby",98},
        {"Kingler",99},
        {"Voltorb",100},
        {"Electrode",101},
        {"Exeggcute",102},
        {"Exeggutor",103},
        {"Cubone",104},
        {"Marowak",105},
        {"Hitmonlee",106},
        {"Hitmonchan",107},
        {"Lickitung",108},
        {"Koffing",109},
        {"Weezing",110},
        {"Rhyhorn",111},
        {"Rhydon",112},
        {"Chansey",113},
        {"Tangela",114},
        {"Kangaskhan",115},
        {"Horsea",116},
        {"Seadra",117},
        {"Goldeen",118},
        {"Seaking",119},
        {"Staryu",120},
        {"Starmie",121},
        {"Mr. Mime",122},
        {"Scyther",123},
        {"Jynx",124},
        {"Electabuzz",125},
        {"Magmar",126},
        {"Pinsir",127},
        {"Tauros",128},
        {"Magikarp",129},
        {"Gyarados",130},
        {"Lapras",131},
        {"Ditto",132},
        {"Eevee",133},
        {"Vaporeon",134},
        {"Jolteon",135},
        {"Flareon",136},
        {"Porygon",137},
        {"Omanyte",138},
        {"Omastar",139},
        {"Kabuto",140},
        {"Kabutops",141},
        {"Aerodactyl",142},
        {"Snorlax",143},
        {"Articuno",144},
        {"Zapdos",145},
        {"Moltres",146},
        {"Dratini",147},
        {"Dragonair",148},
        {"Dragonite",149},
        {"Mewtwo",150},
        {"Mew",151},
        {"Linq",152}
    };
    public static Dictionary<string, int> TMHMtoIndex = new Dictionary<string, int>(){
        {"Mega Punch",1},
        {"Razor Wind",2},
        {"Swords Dance",3},
        {"Whirlwind",4},
        {"Mega Kick",5},
        {"Toxic",6},
        {"Horn Drill",7},
        {"Body Slam",8},
        {"Take Down",9},
        {"Double-Edge",10},
        {"Bubblebeam",11},
        {"Water Gun",12},
        {"Ice Beam",13},
        {"Blizzard",14},
        {"Hyper Beam",15},
        {"Pay Day",16},
        {"Submission",17},
        {"Counter",18},
        {"Seismic Toss",19},
        {"Rage",20},
        {"Mega Drain",21},
        {"Solarbeam",22},
        {"Dragon Rage",23},
        {"Thunderbolt",24},
        {"Thunder",25},
        {"Earthquake",26},
        {"Fissure",27},
        {"Dig",28},
        {"Psychic",29},
        {"Teleport",30},
        {"Mimic",31},
        {"Double Team",32},
        {"Reflect",33},
        {"Bide",34},
        {"Metronome",35},
        {"Selfdestruct",36},
        {"Egg Bomb",37},
        {"Fire Blast",38},
        {"Swift",39},
        {"Skull Bash",40},
        {"Softboiled",41},
        {"Dream Eater",42},
        {"Sky Attack",43},
        {"Rest",44},
        {"Thunder Wave",45},
        {"Psywave",46},
        {"Explosion",47},
        {"Rock Slide",48},
        {"Tri Attack",49},
        {"Substitute",50},
        {"Cut",51},
        {"Fly",52},
        {"Surf",53},
        {"Strength",54},
        {"Flash",55}
            
    };
}

public class PokemonDataJSON : MonoBehaviour
{
    public GameObject container;
     void Awake()
    {

            //Disable the editing view when entering 
            container.SetActive(false);

        FileStream file;
        StreamReader sr;
        EvolutionDictionary evolution = new EvolutionDictionary();
            file = new FileStream(Application.streamingAssetsPath + "/evolutiondata.json", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string evolutiondata = sr.ReadToEnd();
        evolution = JsonConvert.DeserializeObject<EvolutionDictionary>(evolutiondata);
        PokemonStats.evolution.Clear();
        foreach(EvolutionEntry entry in evolution.entries){
            PokemonStats.evolution.Add(entry.pokemon, entry.evolution);
        }
        file.Close();
        BaseStatDictionary baseStat = new BaseStatDictionary();
        file = new FileStream(Application.streamingAssetsPath + "/basestatsdata.json", FileMode.Open, FileAccess.Read);
       sr = new StreamReader(file);
        string basestatdata = sr.ReadToEnd();
        baseStat = JsonConvert.DeserializeObject<BaseStatDictionary>(basestatdata);
        PokemonStats.baseStats.Clear();
        foreach (BaseStatEntry entry in baseStat.entries)
        {
            PokemonStats.baseStats.Add(entry.pokemon, entry.stats);
        }
        file.Close();
        LevelMovesDictionary levelmove = new LevelMovesDictionary();
        file = new FileStream(Application.streamingAssetsPath + "/levelmovesdata.json", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string levelmovesdata = sr.ReadToEnd();
        levelmove = JsonConvert.DeserializeObject<LevelMovesDictionary>(levelmovesdata);
        PokemonStats.levelmoves.Clear();
        foreach (LevelMovesEntry entry in levelmove.entries)
        {
            PokemonStats.levelmoves.Add(entry.pokemon, entry.moves);
        }
        file.Close();
        TMLearnDictionary tMLearn = new TMLearnDictionary();
        file = new FileStream(Application.streamingAssetsPath + "/learnbytmdata.json", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string tmlearndata = sr.ReadToEnd();
        tMLearn = JsonConvert.DeserializeObject<TMLearnDictionary>(tmlearndata);
        PokemonStats.learnbytm.Clear();
        foreach (TMLearnEntry entry in tMLearn.entries)
        {
            PokemonStats.learnbytm.Add(entry.pokemon, entry.moves);
        }
        file.Close();
        EncounterDictionary wgencounter = new EncounterDictionary();
        file = new FileStream(Application.streamingAssetsPath + "/watergrassencounterdata.json", FileMode.Open, FileAccess.Read);
        sr = new StreamReader(file);
        string wgencounterdata = sr.ReadToEnd();
        wgencounter = JsonConvert.DeserializeObject<EncounterDictionary>(wgencounterdata);
        PokemonStats.grasswaterencounters.Clear();
        foreach (EncounterEntry entry in wgencounter.entries)
        {
            PokemonStats.grasswaterencounters.Add(entry.index, entry.table);
        }
        file.Close();
       
    }
}
#if UNITY_EDITOR
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

