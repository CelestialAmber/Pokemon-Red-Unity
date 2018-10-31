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
    public string type;
    public Move(string name)
    {
        this.name = name;

        maxpp = PokemonData.GetMove(name).maxpp;
        pp = maxpp;
        type = PokemonData.GetMove(name).type;
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
        //If the Pokemon hasnt been registered as caught before, register it
        if(!GameData.pokedexlist[PokemonData.MonToID(pokename) - 1].caught){
        RegisterInDex();
        types = new string[2];
        types[0] = PokemonData.PokemonTypes[pokename][0];
         types[1] = PokemonData.PokemonTypes[pokename][1];
        }

    }
    public void RegisterInDex()
    {
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
    public string[] types;
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
    public GameCursor cursor;
    public int selectedMon;
    public int numberofPokemon;
    public int MoveID;
    public MainMenu moon;
    public bool switching;
    public Sprite[] switchMenuSprites;
    public Image switchMenuImage;
    public Image[] healthbars = new Image[6];
    public GameObject[] fieldMoveObj;
    public CustomText[] fieldMoveText;
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
   public int switchMenuOffset,switchMenuOffsetX;
    // Use this for initialization
    public void Initialize()
    {
        switchMenuOffset = 0;
        if (GameData.party.Count == 0)
        {
            currentMenu = mainwindow;
            moon.currentmenu = moon.thismenu;
            this.gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < GameData.party.Count; i++)
        {
            GameData.party[i].SetExpToLevel();
            GameData.party[i].RecalculateStats();
            GameData.party[i].UpdateMovesToLevel();
            GameData.party[i].ownerid = GameData.trainerID;
            GameData.party[i].owner = GameData.playerName;
            int pixelCount = Mathf.RoundToInt((float)GameData.party[i].currenthp * 48 / (float)GameData.party[i].hp);
            healthbars[i] = partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateMainMenu();

        }
        UpdateScreen();
       Dialogue.instance.Deactivate();
       Dialogue.instance.displaysimmediate = true;
        StartCoroutine(Dialogue.instance.text("Choose a Pokemon."));
    }

    public void UpdateScreen()
    {
        int index = 0;
        foreach (Pokemon pokemon in GameData.party)
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
        cursor.SetActive(true);
        for (int l = 0; l < 6; l++)
        {
            partyslots[l].SetActive(false);
        }
        for (int i = 0; i < GameData.party.Count; i++)
        {
            if (i == 0)
            {

                numberofPokemon = 0;
            }
            if (GameData.party[i].pokename != "")
            {
                partyslots[i].SetActive(true);
                numberofPokemon++;
            }

        }
        UpdateMenus();
    }
    public void UpdateSwitch(){

        cursor.SetPosition(96 - 16 * switchMenuOffsetX, 40 - 16 * selectedOption + switchMenuOffset * 16);
        UpdateMenus();
    }
    public void UpdateStats1(){
        cursor.SetActive(false);
        stats1portrait.sprite = GameData.frontMonSprites[PokemonData.MonToID(GameData.party[selectedMon].pokename) - 1];
        int id = PokemonData.MonToID(GameData.party[selectedMon].pokename);
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = GameData.party[selectedMon].attack.ToString();
        speedtext.text = GameData.party[selectedMon].speed.ToString();
        specialtext.text = GameData.party[selectedMon].special.ToString();
        defensetext.text = GameData.party[selectedMon].defense.ToString();
        montype1.text = GameData.party[selectedMon].types[0].ToUpper();
        string type2 = GameData.party[selectedMon].types[1].ToUpper();
        montype2.text = type2 != "" ? ("TYPE2/" + "\n " + type2) : "";
        monnametext.text = GameData.party[selectedMon].name;
        owneridtext.text = GameData.party[selectedMon].ownerid.ToString();
        ownernametext.text = GameData.party[selectedMon].owner;
        int pixelCount = Mathf.RoundToInt((float)GameData.party[selectedMon].currenthp * 48 / (float)GameData.party[selectedMon].hp);
        stat1bar.fillAmount = (float)pixelCount / 48;
        monhptext.text = (GameData.party[selectedMon].currenthp > 99 ? "" : GameData.party[selectedMon].currenthp > 9 ? " " : "  ") + GameData.party[selectedMon].currenthp + " " + GameData.party[selectedMon].hp;
        monleveltext.text = ((GameData.party[selectedMon].level < 100) ? "È" : "") + GameData.party[selectedMon].level.ToString();
        switch (GameData.party[selectedMon].status)
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
            if (GameData.party[selectedMon].moves.Count - 1 >= i)
            {
                Move move = GameData.party[selectedMon].moves[i];
                movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP "  + (move.pp < 10 ? " " : "") + move.pp +  "/" + (move.maxpp < 10 ? " " : "") + move.maxpp;
            }
            else
                movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = GameData.frontMonSprites[PokemonData.MonToID(GameData.party[selectedMon].pokename) - 1];
        monname2text.text = GameData.party[selectedMon].name;
        exptext.text = GameData.party[selectedMon].experience.ToString();
        explefttoleveltext.text = (GameData.party[selectedMon].ExpToNextLevel() - GameData.party[selectedMon].experience).ToString();
        nextleveltext.text = (GameData.party[selectedMon].level < 99 ? "È" + (GameData.party[selectedMon].level + 1).ToString() : 100.ToString());
        int id = PokemonData.MonToID(GameData.party[selectedMon].pokename);
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
    void DisableFieldText(){
        switchMenuOffset = 0;
        switchMenuOffsetX = 0;
        for(int i = 0; i < 4; i++){
            fieldMoveObj[i].SetActive(false);
        }
    }
    void Update()
    {
       
        if (currentMenu == switchstats)
        {




            if (Inputs.pressed("down"))
            {
                selectedOption++;
                MathE.Clamp(ref selectedOption, 0, 2 + switchMenuOffset);
                UpdateSwitch();
            }
            if (Inputs.pressed("up"))
            {
                selectedOption--;
                MathE.Clamp(ref selectedOption, 0, 2 + switchMenuOffset);
                UpdateSwitch();
            }


        }
        if (currentMenu == mainwindow)
        {
            cursor.SetActive(true);
            highlightedmon = GameData.party[selectedOption];
            partyAnimTimer += Time.deltaTime;
            float hpratio = (float)highlightedmon.currenthp / (float)highlightedmon.hp;
            float animLoopTime = hpratio > .5f ? .2f : hpratio > .21f ? .533f : 1.0666f;
            if (partyAnimTimer > animLoopTime)
            {
                partyAnimTimer = 0;
            }

            foreach (Pokemon pokemon in GameData.party)
            {
                Transform slottransform = partyslots[GameData.party.IndexOf(pokemon)].transform;
                if (GameData.party.IndexOf(pokemon) != selectedOption)
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.PokemonPartySprite[pokemon.pokename]].anim[0];
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
                moon.disableB = true;
                 Dialogue.instance.displaysimmediate = false;
               Dialogue.instance.Deactivate();
                Inputs.Enable("start");
                moon.currentmenu = moon.thismenu;
                this.gameObject.SetActive(false);

            }
            else if(currentMenu == switchstats){
                DisableFieldText();
                selectedOption = selectedMon;
                currentMenu = mainwindow;
                UpdateMainMenu();

            }
        }
        if (Inputs.pressed("a"))
        {
            if (currentMenu == mainwindow)
            {
                if (!switching)
                {
                    int textOffsetLength = 6;
                    switchMenuOffsetX = 0;
                    Pokemon selectedPokemon = GameData.party[selectedMon];
                    int selectedMenu = 0;
                    selectedMon = selectedOption;
                    for(int i = 0; i < 4; i++){
                        if(selectedPokemon.moves.Count > i){
                        Move move = selectedPokemon.moves[(selectedPokemon.moves.Count-1)-i];
                    if(isFieldMove(move)) {
                        if(move.name.Length > 6 && selectedMenu != 8){
                         selectedMenu = 4;
                       textOffsetLength = 8;
                     } 
                   if(move.name.Length > 8){
                          selectedMenu = 8;
                      textOffsetLength = 10;
                      }
                        fieldMoveObj[3 - switchMenuOffset].SetActive(true);
                        fieldMoveText[3 - switchMenuOffset].text = move.name.ToUpper();
                        for(int j = 0; j < textOffsetLength - move.name.Length; j++){
                          fieldMoveText[3 - switchMenuOffset].text += " ";  
                        }
                           switchMenuOffset++;
                    }
                        }
                    }
                    switchMenuOffsetX = selectedMenu/4;
                    selectedMenu += switchMenuOffset;
                    switchMenuImage.sprite = switchMenuSprites[selectedMenu];
                    
                    selectedOption = 0;
                    currentMenu = switchstats;
                    UpdateSwitch();
                }
                else
                {
                    StartCoroutine(Switch());
                    UpdateScreen();
                    switching = false;
                }
         
            }
           else if (currentMenu == switchstats)
            {
                    if (selectedOption == switchMenuOffset)
                    {
                        
                        
                        DisableFieldText();
                       Dialogue.instance.Deactivate();
                        currentMenu = stats1;
                        UpdateStats1();


                    }
                    else if (selectedOption == switchMenuOffset + 1)
                    {
                        DisableFieldText();
                        switching = true;
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;
                        UpdateMainMenu();


                    }
                    else if (selectedOption == switchMenuOffset + 2)
                    {
                        DisableFieldText();
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;
                        UpdateMainMenu();


                    }
            
                

            }
             else if (currentMenu == stats1)
            {
         
                    currentMenu = stats2;
                    UpdateStats2();


                
      
            }
            else if (currentMenu == stats2)
            {

                
                    
                   Dialogue.instance.Deactivate();
                   Dialogue.instance.displaysimmediate = true;
                    StartCoroutine(Dialogue.instance.text("Choose a Pokemon."));
                    selectedOption = selectedMon;
                    currentMenu = mainwindow;
                    UpdateMainMenu();

            }



        }

    }
    void UseMove()
    {




    }
    
    IEnumerator Switch()
    {
        //Swap selected Pokemon.
        Pokemon pokemon = GameData.party[selectedOption];
        GameData.party[selectedOption] = GameData.party[selectedMon];
        GameData.party[selectedMon] = pokemon;
        yield return null;

    }

    IEnumerator AnimateOurHealth(int newHealth, int i)
    {
        if (newHealth < 0) newHealth = 0;
        if (newHealth > GameData.party[i].hp) newHealth = GameData.party[i].hp;
        int result = Mathf.RoundToInt(newHealth - GameData.party[i].currenthp);
        WaitForSeconds wait = new WaitForSeconds(5 / GameData.party[i].hp);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            GameData.party[i].currenthp += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)GameData.party[i].currenthp * 48 / (float)GameData.party[i].hp);
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateScreen();
        }
        yield return null;

    }
    public bool hasFieldMove(Pokemon pokemon){
        foreach(Move move in pokemon.moves){
   if(GameData.fieldMoves.Contains(move.name)) return true;
        }
        return false;
    }
    public bool isFieldMove(Move move){
   if(GameData.fieldMoves.Contains(move.name)) return true;
    else return false;
    }




}

