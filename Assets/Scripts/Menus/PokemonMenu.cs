using System.Collections;
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

        maxpp = PokemonData.GetMove(name).maxpp;
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
        if (PokemonData.MonToID(pokename) > 151) return;
        GameData.pokedexlist[PokemonData.MonToID(pokename) - 1].seen = true;
        GameData.pokedexlist[PokemonData.MonToID(pokename) - 1].caught = true;

    }
    public void RecalculateStats()
    {

        hp = Mathf.FloorToInt(((PokemonData.baseStats[pokename][0] + ivs[0]) * 2 + Mathf.Sqrt(statexp[0]) / 4) * level / 100) + level + 10;
        attack = Mathf.FloorToInt(((PokemonData.baseStats[pokename][1] + ivs[1]) * 2 + Mathf.Sqrt(statexp[1]) / 4) * level / 100) + 5;
        defense = Mathf.FloorToInt(((PokemonData.baseStats[pokename][2] + ivs[2]) * 2 + Mathf.Sqrt(statexp[2]) / 4) * level / 100) + 5;
        speed = Mathf.FloorToInt(((PokemonData.baseStats[pokename][3] + ivs[3]) * 2 + Mathf.Sqrt(statexp[3]) / 4) * level / 100) + 5;
        special = Mathf.FloorToInt(((PokemonData.baseStats[pokename][4] + ivs[4]) * 2 + Mathf.Sqrt(statexp[4]) / 4) * level / 100) + 5;
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
        switch (PokemonData.PokemonExpGroup[pokename])
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
        if (experience < 0) experience += (int)Mathf.Pow(2,24); //Underflow by 16,777,216;

    }
    public int ExpToNextLevel()
    {
        if (level >= 100) return experience;
        switch (PokemonData.PokemonExpGroup[pokename])
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
        for (int i = 0; i < PokemonData.levelmoves[pokename].Length; i++)
        {
            StrInt movetocheck = PokemonData.levelmoves[pokename][i];
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
public class PokemonMenu : MonoBehaviour
{
    public GameObject mainwindow, switchstats, stats1, stats2;
    public GameObject currentMenu;
    public GameObject[] allmenus;
    public List<GameObject> partyslots;
    public int selectedOption;
    public Cursor cursor;
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
            party.Add(new Pokemon(PokemonData.IndexToMon(Random.Range(1, 152)), Random.Range(1, 101)));
            healthbars[i] = partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
        }
    }
    // Use this for initialization
    public void Initialize()
    {

        if (party.Count == 0)
        {
            currentMenu = mainwindow;
            moon.currentmenu = moon.thismenu;
            this.gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < party.Count; i++)
        {
            party[i].SetExpToLevel();
            party[i].RecalculateStats();
            party[i].UpdateMovesToLevel();
            party[i].ownerid = GameData.trainerID;
            party[i].owner = GameData.playerName;
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
            partyslots[l].SetActive(false);
        }
        for (int i = 0; i < party.Count; i++)
        {
            if (i == 0)
            {

                numberofPokemon = 0;
            }
            if (party[i].pokename != "")
            {
                partyslots[i].SetActive(true);
                numberofPokemon++;
            }

        }
        UpdateMenus();
    }
    public void UpdateSwitch(){
       
        cursor.SetPosition(96, 40 - 16 * selectedOption);
        UpdateMenus();
    }
    public void UpdateStats1(){
        cursor.SetActive(false);
        stats1portrait.sprite = GameData.frontMonSprites[PokemonData.MonToID(party[selectedMon].pokename) - 1];
        int id = PokemonData.MonToID(party[selectedMon].pokename);
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = party[selectedMon].attack.ToString();
        speedtext.text = party[selectedMon].speed.ToString();
        specialtext.text = party[selectedMon].special.ToString();
        defensetext.text = party[selectedMon].defense.ToString();
        montype1.text = PokemonData.PokemonTypes[party[selectedMon].pokename][0].ToUpper();
        string type2 = PokemonData.PokemonTypes[party[selectedMon].pokename][1].ToUpper();
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
        UpdateMenus();
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
                movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP "  + (move.pp < 10 ? " " : "") + move.pp +  "/" + (move.maxpp < 10 ? " " : "") + move.maxpp;
            }
            else
                movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = GameData.frontMonSprites[PokemonData.MonToID(party[selectedMon].pokename) - 1];
        monname2text.text = party[selectedMon].name;
        exptext.text = party[selectedMon].experience.ToString();
        explefttoleveltext.text = (party[selectedMon].ExpToNextLevel() - party[selectedMon].experience).ToString();
        nextleveltext.text = (party[selectedMon].level < 100 ? "È" + (party[selectedMon].level + 1).ToString() : 100.ToString());
        int id = PokemonData.MonToID(party[selectedMon].pokename);
        pokedexno2.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        UpdateMenus();
    }
    void UpdateMenus(){
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
    }
    void Update()
    {
        if (currentMenu == switchstats)
        {
            cursor.SetActive(true);

           

            if (Inputs.pressed("down"))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 2);
                cursor.SetPosition(96, 40 - 16 * selectedOption);
            }
            if (Inputs.pressed("up"))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 2);
                cursor.SetPosition(96, 40 - 16 * selectedOption);
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
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.PokemonPartySprite[pokemon.pokename]].anim[1];
                }
                else
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.PokemonPartySprite[pokemon.pokename]].anim[Mathf.RoundToInt(partyAnimTimer / animLoopTime)];
                }
            }

            cursor.SetActive(true);
            cursor.SetPosition(0,128 - 16 * selectedOption);



            if (Inputs.pressed("down"))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, numberofPokemon - 1);
            }
            if (Inputs.pressed("up"))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, numberofPokemon - 1);
            }


        }

        if (Inputs.pressed("b"))
        {
            if (currentMenu == mainwindow)
            {

                d.Deactivate();
                Inputs.Enable("start");
                moon.currentmenu = moon.thismenu;
                this.gameObject.SetActive(false);

            }
            if(currentMenu == switchstats){
                selectedOption = selectedMon;
                UpdateMainMenu();
                currentMenu = mainwindow;
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
                    UpdateSwitch();
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
                        UpdateMainMenu();
                        currentMenu = mainwindow;

                    }
                    if (selectedOption == 2)
                    {
                        selectedOption = selectedMon;
                        UpdateMainMenu();
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
                    d.displaysimmediate = true;
                    StartCoroutine(d.text("Choose a Pokemon."));
                    selectedOption = selectedMon;
                    UpdateMainMenu();
                    currentMenu = mainwindow;

                }
                StartCoroutine(Wait());
            }



        }
        UpdateMenus();
    }
    void UseMove()
    {
        



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
[CustomEditor(typeof(PokemonMenu))]
public class PokemonDebugEditor : Editor
{
    public int minLevel, maxLevel;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PokemonMenu me = (PokemonMenu)target;
        minLevel = EditorGUILayout.IntSlider("Min Level", minLevel, 1, maxLevel);
        maxLevel = EditorGUILayout.IntSlider("Max Level", maxLevel, minLevel, 100);
        if (GUILayout.Button("Generate Party"))
        {
            me.party.Clear();
            for (int i = 0; i < 6; i++)
            {
                me.party.Add(new Pokemon(PokemonData.IndexToMon(Random.Range(1, 152)), Random.Range(minLevel, maxLevel + 1)));
                me.healthbars[i] = me.partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();

            }
            for (int i = 0; i < me.party.Count; i++)
            {
                me.party[i].currenthp /= 2;
                me.party[i].ownerid = GameData.trainerID;
                me.party[i].owner = GameData.playerName;
            }
            me.UpdateScreen();
        }
        if (GUILayout.Button("Set First Party Mon as Linq"))
        {
            me.party.RemoveAt(0);
            me.party.Insert(0, new Pokemon(PokemonData.IndexToMon(152), Random.Range(minLevel, maxLevel + 1)));
        }
    }
}
#endif
