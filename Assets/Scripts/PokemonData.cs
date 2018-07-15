﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Move
{

    public string name;
    public int pp;
    public int maxpp;
    public Move(string name)
    {
        this.name = name;

        maxpp = PokemonStats.GetMove(name).maxpp;
        pp = maxpp;
    }
}
public enum Status
{
    Ok,
    Sleep,
    Burn,
    Poison,
    Paralyzed,
    Frozen,
    Fainted
}

[System.Serializable]
public class Pokemon
{
    public Pokemon(string pokename, int level)
    {
        this.pokename = pokename;
        this.level = level;
        this.name = this.pokename.ToUpper();
        GenerateIvs();
        moves = new List<Move>();
        UpdateMovesToLevel();
        SetExpToLevel();
        RecalculateStats();
        RegisterInDex();
    }
    public void RegisterInDex()
    {
        SaveData.pokedexlist[PokemonStats.MonToID(pokename) - 1].seen = true;
        SaveData.pokedexlist[PokemonStats.MonToID(pokename) - 1].caught = true;

    }
    public void RecalculateStats()
    {

        hp = Mathf.FloorToInt(((PokemonStats.baseStats[pokename][0] + ivs[0]) * 2 + Mathf.Sqrt(statexp[0]) / 4) * level / 100) + level + 10;
        attack = Mathf.FloorToInt(((PokemonStats.baseStats[pokename][1] + ivs[1]) * 2 + Mathf.Sqrt(statexp[1]) / 4) * level / 100) + 5;
        defense = Mathf.FloorToInt(((PokemonStats.baseStats[pokename][2] + ivs[2]) * 2 + Mathf.Sqrt(statexp[2]) / 4) * level / 100) + 5;
        speed = Mathf.FloorToInt(((PokemonStats.baseStats[pokename][3] + ivs[3]) * 2 + Mathf.Sqrt(statexp[3]) / 4) * level / 100) + 5;
        special = Mathf.FloorToInt(((PokemonStats.baseStats[pokename][4] + ivs[4]) * 2 + Mathf.Sqrt(statexp[4]) / 4) * level / 100) + 5;
        currenthp = hp;
    }
    public void GenerateIvs()
    {
        ivs[1] = Random.Range(0, 16);
        ivs[2] = Random.Range(0, 16);
        ivs[3] = Random.Range(0, 16);
        ivs[4] = Random.Range(0, 16);
        ivs[0] = (ivs[1] % 2 == 1 ? 8 : 0) + (ivs[2] % 2 == 1 ? 4 : 0) + (ivs[3] % 2 == 1 ? 2 : 0) + (ivs[4] % 2 == 1 ? 1 : 0);
    }
    public void SetExpToLevel()
    {
        switch (PokemonStats.PokemonExpGroup[pokename])
        {
            case 0: //Slow
                experience = (int)(5 * Mathf.Pow(level, 3) / 4);
                break;
            case 1: //Medium Slow
                experience = (int)((6 / 5) * Mathf.Pow(level, 3) - 15 * Mathf.Pow(level, 2) + 100 * (level) - 140);
                break;
            case 2: //Medium Fast
                experience = (int)(Mathf.Pow(level, 3));
                break;
            case 3: //Fast
                experience = 4 * (int)(Mathf.Pow(level, 3) / 5);
                break;
            default:
                throw new UnityException("Invalid experience index was given.");
        }
        if (experience < 0) experience += 16777216;

    }
    public int ExpToNextLevel()
    {
        if (level >= 100) return experience;
        switch (PokemonStats.PokemonExpGroup[pokename])
        {
            case 0: //Slow
                return (int)(5 * Mathf.Pow(level + 1, 3) / 4);
            case 1: //Medium Slow
                return (int)((6 / 5) * Mathf.Pow(level + 1, 3) - 15 * Mathf.Pow(level + 1, 2) + 100 * (level + 1) - 140);
            case 2: //Medium Fast
                return (int)(Mathf.Pow(level + 1, 3));
            case 3: //Fast
                return 4 * (int)(Mathf.Pow(level + 1, 3) / 5);

        }
        throw new UnityException("Invalid experience index was given.");
    }
    bool AlreadyHasMove(string movename)
    {
        foreach (Move move in moves)
        {
            if (move.name == movename)
                return true;

        }
        return false;

    }
    public void UpdateMovesToLevel()
    {
        for (int i = 0; i < PokemonStats.levelmoves[pokename].Length; i++)
        {
            StrInt movetocheck = PokemonStats.levelmoves[pokename][i];
            if (level >= movetocheck.Int)
            {
                if (!AlreadyHasMove(movetocheck.Name))
                    if (moves.Count < 4)
                    {
                        moves.Add(new Move(movetocheck.Name));
                    }
                    else
                    {
                        moves.RemoveAt(0);
                        moves.Add(new Move(movetocheck.Name));
                    }
            }
        }
        //iterate through all moves learned by level, and adjust the move pool accordingly
    }
    public int hp;
    public int attack;
    public int defense;
    public int speed;
    public int special;
    public int currenthp;
    public Status status;
    public int[] ivs = new int[5];
    public int[] statexp = new int[5];
    public string pokename;
    public int level;
    public string name;
    public int ownerid;
    public string owner;
    public int experience;
    public List<Move> moves = new List<Move>();
}
[System.Serializable]
public class PartyAnim
{
    public List<Sprite> anim;
}
public class PokemonData : MonoBehaviour
{
    public Sprite[] MonImages = new Sprite[151];
    public GameObject mainwindow, switchstats, stats1, stats2;
    public GameObject currentMenu;
    public GameObject[] allmenus;
    public List<GameObject> menuSlots, pokemenuslots, rpkmn;
    public List<GameObject> partyslots, wrpkmn;
    public int selectedOption;
    public GameObject cursor;
    public int selectedMon;
    public int numberofPokemon;
    public int MoveID;
    public MainMenu moon;
    public bool donewaiting;
    public int loadedpokemonID;
    public int loadedMonStatus;
    public bool switching;
    public GameObject battlemenu;
    public BattleManager bm;
    public Player play;
    public Dialogue d;
    public List<Pokemon> party = new List<Pokemon>();
    public int[] loadedMonStats = new int[4];
    public Image[] healthbars = new Image[6];
    //STATS1DATA
    public Image stats1portrait;
    public Image stat1bar;
    public CustomText pokedexNO, attacktext, speedtext, specialtext, defensetext, MonStatustext, monhptext, monnametext, montype1, montype2, owneridtext, ownernametext, monleveltext, monstatustext;
    //STATS2DATA
    public Image stats2portrait;
    public CustomText movetext, exptext, explefttoleveltext, nextleveltext, monname2text, pokedexno2;
    public List<PartyAnim> partyanims;
    Pokemon highlightedmon;
    public float partyAnimTimer = 0;
    private void Awake()
    {
        party.Clear();
        for (int i = 0; i < 6; i++)
        {
            party.Add(new Pokemon(PokemonStats.IndexToMon(Random.Range(1, 152)), Random.Range(1, 101)));
            healthbars[i] = partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }
    }
    // Use this for initialization
    public void Initialize()
    {
        for (int i = 0; i < party.Count; i++)
        {
            party[i].SetExpToLevel();
            party[i].RecalculateStats();
            party[i].UpdateMovesToLevel();
            party[i].ownerid = SaveData.trainerID;
            party[i].owner = Dialogue.Name;
            int pixelCount = Mathf.RoundToInt((float)party[i].currenthp * 48 / (float)party[i].hp);
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateMainMenu();

        }
        UpdateScreen();
        d.Deactivate();
        d.cantscroll = true;
        d.displaysimmediate = true;
        StartCoroutine(d.text("Choose a Pokemon."));
    }

    // Update is called once per frame
    public void StartBattle(int battleID, int battleType)
    {
        play.inBattle = true;
        Player.disabled = true;
        play.overrideRenable = true;
        battlemenu.SetActive(true);
        bm.battleoverlay.sprite = bm.blank;
        bm.battleID = battleID;
        bm.Initialize();
    }
    public void UpdateScreen()
    {
        int index = 0;
        foreach (Pokemon pokemon in party)
        {
            Transform slottransform = partyslots[index].transform;
            int pixelCount = Mathf.RoundToInt((float)pokemon.currenthp * 48 / (float)pokemon.hp);
            slottransform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (float)pixelCount / 48;
            slottransform.GetChild(2).GetComponent<CustomText>().text = pokemon.name;
            slottransform.GetChild(3).GetComponent<CustomText>().text = ((pokemon.level < 100) ? "È" : "") + pokemon.level.ToString();
            slottransform.GetChild(4).GetComponent<CustomText>().text = (pokemon.currenthp > 99 ? "" : pokemon.currenthp > 9 ? " " : "  ") + pokemon.currenthp + (pokemon.hp > 99 ? "/" : pokemon.hp > 9 ? "/ " : "/  ") + pokemon.hp;
            index++;
        }
    }
    public void UpdateMainMenu()
    {

        for (int l = 0; l < 6; l++)
        {
            pokemenuslots[l].SetActive(false);
            partyslots[l].SetActive(false);
        }
        for (int i = 0; i < party.Count; i++)
        {
            if (i == 0)
            {

                numberofPokemon = 0;
                rpkmn.Clear();
                wrpkmn.Clear();
            }
            if (party[i].pokename != "")
            {
                partyslots[i].SetActive(true);
                pokemenuslots[i].SetActive(true);
                numberofPokemon++;
                rpkmn.Add(pokemenuslots[i]);
                wrpkmn.Add(partyslots[i]);
            }

        }
    }
    public void UpdateStats1(){
        cursor.SetActive(false);
        stats1portrait.sprite = MonImages[PokemonStats.MonToID(party[selectedMon].pokename) - 1];
        int id = PokemonStats.MonToID(party[selectedMon].pokename);
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = party[selectedMon].attack.ToString();
        speedtext.text = party[selectedMon].speed.ToString();
        specialtext.text = party[selectedMon].special.ToString();
        defensetext.text = party[selectedMon].defense.ToString();
        montype1.text = PokemonStats.PokemonTypes[party[selectedMon].pokename][0].ToUpper();
        string type2 = PokemonStats.PokemonTypes[party[selectedMon].pokename][1].ToUpper();
        montype2.text = type2 != "" ? ("TYPE2/" + "\n " + type2) : "";
        monnametext.text = party[selectedMon].name;
        owneridtext.text = party[selectedMon].ownerid.ToString();
        ownernametext.text = party[selectedMon].owner;
        int pixelCount = Mathf.RoundToInt((float)party[selectedMon].currenthp * 48 / (float)party[selectedMon].hp);
        stat1bar.fillAmount = (float)pixelCount / 48;
        monhptext.text = (party[selectedMon].currenthp > 99 ? "" : party[selectedMon].currenthp > 9 ? " " : "  ") + party[selectedMon].currenthp + " " + party[selectedMon].hp;
        monleveltext.text = ((party[selectedMon].level < 100) ? "È" : "") + party[selectedMon].level.ToString();
        switch (party[selectedMon].status)
        {
            case Status.Ok:
                monstatustext.text = "OK";
                break;
            case Status.Fainted:
                monstatustext.text = "FNT";
                break;
            case Status.Frozen:
                monstatustext.text = "FRZ";
                break;
            case Status.Burn:
                monstatustext.text = "BRN";
                break;
            case Status.Paralyzed:
                monstatustext.text = "PRZ";
                break;
            case Status.Poison:
                monstatustext.text = "PSN";
                break;
            case Status.Sleep:
                monstatustext.text = "SLP";
                break;
        }

    }
    public void UpdateStats2()
    {
        cursor.SetActive(false);
        string movestr = "";
        for (int i = 0; i < 4; i++)
        {
            if (party[selectedMon].moves.Count - 1 >= i)
            {
                Move move = party[selectedMon].moves[i];
                movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP " + move.pp + "/" + move.maxpp;
            }
            else
                movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = MonImages[PokemonStats.MonToID(party[selectedMon].pokename) - 1];
        monname2text.text = party[selectedMon].name;
        exptext.text = party[selectedMon].experience.ToString();
        explefttoleveltext.text = (party[selectedMon].ExpToNextLevel() - party[selectedMon].experience).ToString();
        nextleveltext.text = (party[selectedMon].level < 100 ? "È" + (party[selectedMon].level + 1).ToString() : 100.ToString());
        int id = PokemonStats.MonToID(party[selectedMon].pokename);
        pokedexno2.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();

    }
    void Update()
    {
        if (currentMenu == stats1)
        {
           

        }
        if (currentMenu == stats2)
        {
           
        }

        if (party.Count == 0)
        {
            currentMenu = mainwindow;
            moon.currentmenu = moon.thismenu;
            this.gameObject.SetActive(false);
            return;
        }


        if (currentMenu == switchstats)
        {
            menuSlots.Resize(currentMenu.transform.childCount);
            menuSlots.Clear();
            for (int i = 0; i < currentMenu.transform.childCount; i++)
            {


                menuSlots.Add(currentMenu.transform.GetChild(i).gameObject);
            }

            cursor.transform.position = menuSlots[selectedOption].transform.position;

            cursor.SetActive(true);

            if (Inputs.pressed("down"))
            {
                selectedOption++;
            }
            if (Inputs.pressed("up"))
            {
                selectedOption--;
            }
            if (selectedOption < 0)
            {
                selectedOption = 0;

            }
            if (selectedOption == menuSlots.Count)
            {
                selectedOption = menuSlots.Count - 1;

            }


        }
        if (currentMenu == mainwindow)
        {
            cursor.SetActive(true);
            highlightedmon = party[selectedOption];
            partyAnimTimer += Time.deltaTime;
            float hpratio = (float)highlightedmon.currenthp / (float)highlightedmon.hp;
            float animLoopTime = hpratio > .5f ? .2f : hpratio > .21f ? .533f : 1.0666f;
            if (partyAnimTimer > animLoopTime)
            {
                partyAnimTimer = 0;
            }

            foreach (Pokemon pokemon in party)
            {
                Transform slottransform = partyslots[party.IndexOf(pokemon)].transform;
                if (party.IndexOf(pokemon) != selectedOption)
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonStats.PokemonPartySprite[pokemon.pokename]].anim[1];
                }
                else
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonStats.PokemonPartySprite[pokemon.pokename]].anim[Mathf.RoundToInt(partyAnimTimer / animLoopTime)];
                }
            }

            cursor.SetActive(true);
            cursor.transform.position = pokemenuslots[selectedOption].transform.position;



            if (Inputs.pressed("down"))
            {
                selectedOption++;
            }
            if (Inputs.pressed("up"))
            {
                selectedOption--;
            }
            if (selectedOption < 0)
            {
                selectedOption = 0;

            }
            if (selectedOption == numberofPokemon)
            {
                selectedOption = numberofPokemon - 1;

            }


        }
        foreach (GameObject menu in allmenus)
        {
            if (menu != currentMenu)
            {
                menu.SetActive(false);
            }
            else
            {

                menu.SetActive(true);
            }


            if (menu == switchstats && (currentMenu == mainwindow))
            {
                menu.SetActive(false);

            }
            if (menu == mainwindow && (currentMenu == switchstats))
            {
                menu.SetActive(true);

            }

        }
        if (Inputs.pressed("b"))
        {
            if (currentMenu == mainwindow)
            {

                d.Deactivate();
                moon.currentmenu = moon.thismenu;
                this.gameObject.SetActive(false);

            }
        }
        if (Inputs.pressed("a"))
        {
            if (currentMenu == mainwindow)
            {
                if (!switching)
                {
                    selectedMon = selectedOption;
                    selectedOption = 0;
                    currentMenu = switchstats;
                }
                else
                {
                    StartCoroutine(Switch());
                    UpdateScreen();
                    switching = false;
                }
                StartCoroutine(Wait());
            }
            if (currentMenu == switchstats)
            {
                if (donewaiting)
                {

                    if (selectedOption == 0)
                    {
                        d.Deactivate();
                        UpdateStats1();
                        currentMenu = stats1;

                    }
                    if (selectedOption == 1)
                    {

                        switching = true;
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;

                    }
                    if (selectedOption == 2)
                    {
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;

                    }
                    StartCoroutine(Wait());
                }

            }
            if (currentMenu == stats1)
            {
                if (donewaiting)
                {
                    UpdateStats2();
                    currentMenu = stats2;

                }
                StartCoroutine(Wait());
            }
            if (currentMenu == stats2)
            {

                if (donewaiting)
                {
                    d.Deactivate();
                    d.cantscroll = true;
                    d.displaysimmediate = true;
                    StartCoroutine(d.text("Choose a Pokemon."));
                    currentMenu = mainwindow;

                }
                StartCoroutine(Wait());
            }



        }
    }
    void UseMove()
    {
        switch (MoveID)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            case 16:
                break;
            case 17:
                break;
            case 18:
                break;
            case 19:
                break;
            case 20:
                break;
            case 21:
                break;
            case 22:
                break;
            case 23:
                break;
            case 24:
                break;

                //etc




        }




    }
    IEnumerator Wait()
    {
        donewaiting = false;
        yield return new WaitForSeconds(0.1f);
        donewaiting = true;
    }
    IEnumerator Switch()
    {
        //Swap selected Pokemon.
        Pokemon pokemon = party[selectedOption];
        party[selectedOption] = party[selectedMon];
        party[selectedMon] = pokemon;
        yield return null;

    }

    IEnumerator AnimateOurHealth(int newHealth, int i)
    {
        if (newHealth < 0) newHealth = 0;
        if (newHealth > party[i].hp) newHealth = party[i].hp;
        int result = Mathf.RoundToInt(newHealth - party[i].currenthp);
        WaitForSeconds wait = new WaitForSeconds(5 / party[i].hp);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            party[i].currenthp += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)party[i].currenthp * 48 / (float)party[i].hp);
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateScreen();
        }
        yield return null;

    }


}
#if UNITY_EDITOR
[CustomEditor(typeof(PokemonData))]
public class PokemonDebugEditor : Editor
{
    public int minLevel, maxLevel;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PokemonData me = (PokemonData)target;
        minLevel = EditorGUILayout.IntSlider("Min Level", minLevel, 1, maxLevel);
        maxLevel = EditorGUILayout.IntSlider("Max Level", maxLevel, minLevel, 100);
        if (GUILayout.Button("Generate Party"))
        {
            me.party.Clear();
            for (int i = 0; i < 6; i++)
            {
                me.party.Add(new Pokemon(PokemonStats.IndexToMon(Random.Range(1, 152)), Random.Range(minLevel, maxLevel + 1)));
                me.healthbars[i] = me.partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();

            }
            for (int i = 0; i < me.party.Count; i++)
            {
                me.party[i].currenthp /= 2;
                me.party[i].ownerid = SaveData.trainerID;
                me.party[i].owner = Dialogue.Name;
            }
            me.UpdateScreen();
        }
        if (GUILayout.Button("Set First Party Mon as Linq"))
        {
            me.party.RemoveAt(0);
            me.party.Insert(0, new Pokemon(PokemonStats.IndexToMon(152), Random.Range(minLevel, maxLevel + 1)));
        }
    }
}
#endif
