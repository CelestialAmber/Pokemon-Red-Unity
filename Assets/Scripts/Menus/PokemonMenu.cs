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
    public Move()
    {
        this.name = "";

        maxpp = 0;
        pp = 0;
        type = null;
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
    public Pokemon(string name, int level, bool isWildPokemon)
    {
        this.name = name;
        this.level = level;
        this.isWildPokemon = isWildPokemon;
        this.nickname = this.name.ToUpper();
        GenerateIvs();
        moves = new Move[4];
        for(int i = 0; i < 4; i++){
            moves[i] = new Move();
        }
        UpdateMovesToLevel();
        SetExpToLevel();
        RecalculateStats();
        //If the Pokemon hasnt been registered as caught before, register it
        if (!GameData.instance.pokedexlist[PokemonData.MonToID(name) - 1].caught)
        {
            RegisterInDex();
            types = new string[2];
            types[0] = PokemonData.PokemonTypes[name][0];
            types[1] = PokemonData.PokemonTypes[name][1];
        }
        if(!isWildPokemon){

            ownerid = GameData.instance.trainerID;
            owner = GameData.instance.playerName;
        }

    }

    public void RegisterInDex()
    {

        GameData.instance.pokedexlist[PokemonData.MonToID(name) - 1].seen = true;
        if(!isWildPokemon) GameData.instance.pokedexlist[PokemonData.MonToID(name) - 1].caught = true;

    }
    public void RecalculateStats()
    {

        maxhp = Mathf.FloorToInt(((PokemonData.baseStats[name][0] + ivs[0]) * 2 + Mathf.Sqrt(evs[0]) / 4) * level / 100) + level + 10;
        attack = Mathf.FloorToInt(((PokemonData.baseStats[name][1] + ivs[1]) * 2 + Mathf.Sqrt(evs[1]) / 4) * level / 100) + 5;
        defense = Mathf.FloorToInt(((PokemonData.baseStats[name][2] + ivs[2]) * 2 + Mathf.Sqrt(evs[2]) / 4) * level / 100) + 5;
        speed = Mathf.FloorToInt(((PokemonData.baseStats[name][3] + ivs[3]) * 2 + Mathf.Sqrt(evs[3]) / 4) * level / 100) + 5;
        special = Mathf.FloorToInt(((PokemonData.baseStats[name][4] + ivs[4]) * 2 + Mathf.Sqrt(evs[4]) / 4) * level / 100) + 5;
        currenthp = maxhp;
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
        experience = CalculateExp(level).UnderflowUInt24(); //Underflow by 16,777,216;
    }
    public int ExpToNextLevel()
    {
        if (level >= 100) return experience;
        int result = CalculateExp(level + 1).UnderflowUInt24(); //Underflow by 16,777,216;
         return (result - experience).UnderflowUInt24();
        
    }
    public int CalculateExp(int levelNum){
        switch (PokemonData.PokemonExpGroup[name])
        {
            case 0: //Slow
                return Mathf.FloorToInt(5 * Mathf.Pow(levelNum, 3) / 4f);
            case 1: //Medium Slow
                return Mathf.FloorToInt((6f / 5f) * Mathf.Pow(levelNum, 3) - 15 * Mathf.Pow(levelNum, 2) + 100 * (levelNum) - 140);
            case 2: //Medium Fast
                return Mathf.FloorToInt(Mathf.Pow(levelNum, 3));
            case 3: //Fast
                return Mathf.FloorToInt(4 * Mathf.Pow(levelNum, 3) / 5f);
            default:
                throw new UnityException("Invalid experience index was given.");

        }
    }
    int CalculateLevelFromExp(){
        for(int i = 0; i <= 100; i++){ //iterate through each level to find the level from the current exp
            int exp = CalculateExp(i);
            if(exp > experience) return i - 1; //if the current level exp is higher, return the previous level
        }
        return 100; //if the current experience is higher than the level 100 experience, return 100
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
    Move MoveAtCurrentLevel(){
        foreach(System.Tuple<string,int> move in PokemonData.levelmoves[name]){
            if(move.Item2 == level) return new Move(move.Item1);
        }
        return null;
    }
    public void UpdateMovesToLevel()
    {
        for (int i = 0; i < PokemonData.levelmoves[name].Length; i++)
        {
            System.Tuple<string,int> movetocheck = PokemonData.levelmoves[name][i];
            if (level >= movetocheck.Item2)
            {
                if (!AlreadyHasMove(movetocheck.Item1))
                    if (numberOfMoves < 4)
                    {
                        moves[numberOfMoves] = new Move(movetocheck.Item1);
                    }
                    else
                    {
                        moves[0] = new Move(movetocheck.Item1);
                    }
            }
        }
        //iterate through all moves learned by level, and adjust the move pool accordingly
    }
    public void AddMove(string moveName){
        if(numberOfMoves == 4){
             Debug.Log("Cannot add a new move, there are already 4 moves");
        }
             for(int i = 0; i < 4; i++){

                 if(!slotHasMove(i)){

                  moves[i] = new Move(moveName);
                 numberOfMoves++;
                  break;
                 }
                
             }
            return;
    }
    public void SetMove(string moveName,int moveIndex){
        if(moveIndex > 3 || moveIndex < 0) throw new UnityException("Invalid move index");
            moves[moveIndex] = new Move(moveName);

    }
    public bool slotHasMove(int index){
        return moves[index].name != "";
    }
    public int maxhp;
    public int attack;
    public int defense;
    public int speed;
    public int special;
    public int currenthp;
    public bool isWildPokemon;
    public Status status;
    public int[] ivs = new int[5];
    public int[] evs = new int[5];
    public string name;
    public int level;
    public string nickname;
    public int ownerid;
    public string owner;
    public int experience;
    public string[] types;
    public Move[] moves;
    public int numberOfMoves;
   
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
    public bool switching, selectingPokemon;
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
   public string[] fieldMoveNames = new string[4];
   public static PokemonMenu instance;
    // Use this for initialization
    void Awake(){
        instance = this;
    }
   
    public void Initialize()
    {
        switchMenuOffset = 0;
        if (GameData.instance.party.Count == 0)
        {
            currentMenu = mainwindow;
            moon.currentmenu = moon.thismenu;
            this.gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < GameData.instance.party.Count; i++)
        {
            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].currenthp * 48 / (float)GameData.instance.party[i].maxhp);
            healthbars[i] = partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateMainMenu();

        }
        UpdateScreen();
       Dialogue.instance.Deactivate();
       Dialogue.instance.fastText = true;
       Dialogue.instance.keepTextOnScreen = true;
       Dialogue.instance.needButtonPress = false;
        StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
        Dialogue.instance.finishedText = true;
    }

    public void UpdateScreen()
    {
        int index = 0;
        foreach (Pokemon pokemon in GameData.instance.party)
        {
            Transform slottransform = partyslots[index].transform;
            int pixelCount = Mathf.RoundToInt((float)pokemon.currenthp * 48 / (float)pokemon.maxhp);
            slottransform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (float)pixelCount / 48;
            slottransform.GetChild(2).GetComponent<CustomText>().text = pokemon.nickname;
            slottransform.GetChild(3).GetComponent<CustomText>().text = ((pokemon.level < 100) ? "<LEVEL>" : "") + pokemon.level.ToString();
            slottransform.GetChild(4).GetComponent<CustomText>().text = (pokemon.currenthp > 99 ? "" : pokemon.currenthp > 9 ? " " : "  ") + pokemon.currenthp + (pokemon.maxhp > 99 ? "/" : pokemon.maxhp > 9 ? "/ " : "/  ") + pokemon.maxhp;
            index++;
        }
    }
    public void UpdateMainMenu()
    {
        selectedOption = selectedMon;
        cursor.SetActive(true);
        for (int l = 0; l < 6; l++)
        {
            partyslots[l].SetActive(false);
        }
        for (int i = 0; i < GameData.instance.party.Count; i++)
        {
            if (i == 0)
            {

                numberofPokemon = 0;
            }
            if (GameData.instance.party[i].name != "")
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
        stats1portrait.sprite = GameData.instance.frontMonSprites[PokemonData.MonToID(GameData.instance.party[selectedMon].name) - 1];
        int id = PokemonData.MonToID(GameData.instance.party[selectedMon].name);
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = GameData.instance.party[selectedMon].attack.ToString();
        speedtext.text = GameData.instance.party[selectedMon].speed.ToString();
        specialtext.text = GameData.instance.party[selectedMon].special.ToString();
        defensetext.text = GameData.instance.party[selectedMon].defense.ToString();
        montype1.text = GameData.instance.party[selectedMon].types[0].ToUpper();
        string type2 = GameData.instance.party[selectedMon].types[1].ToUpper();
        montype2.text = type2 != "" ? ("TYPE2/" + "\n " + type2) : "";
        monnametext.text = GameData.instance.party[selectedMon].nickname;
        owneridtext.text = GameData.instance.party[selectedMon].ownerid.ToString();
        ownernametext.text = GameData.instance.party[selectedMon].owner;
        int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[selectedMon].currenthp * 48 / (float)GameData.instance.party[selectedMon].maxhp);
        stat1bar.fillAmount = (float)pixelCount / 48;
        monhptext.text = (GameData.instance.party[selectedMon].currenthp > 99 ? "" : GameData.instance.party[selectedMon].currenthp > 9 ? " " : "  ") + GameData.instance.party[selectedMon].currenthp + " " + GameData.instance.party[selectedMon].maxhp;
        monleveltext.text = ((GameData.instance.party[selectedMon].level < 100) ? "<LEVEL>" : "") + GameData.instance.party[selectedMon].level.ToString();
        switch (GameData.instance.party[selectedMon].status)
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
            if (GameData.instance.party[selectedMon].slotHasMove(i))
            {
                Move move = GameData.instance.party[selectedMon].moves[i];
                movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP "  + (move.pp < 10 ? " " : "") + move.pp +  "/" + (move.maxpp < 10 ? " " : "") + move.maxpp;
            }
            else
                movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = GameData.instance.frontMonSprites[PokemonData.MonToID(GameData.instance.party[selectedMon].name) - 1];
        monname2text.text = GameData.instance.party[selectedMon].nickname;
        
        exptext.text = TruncateExpNumber(GameData.instance.party[selectedMon].experience.ToString());
        explefttoleveltext.text =  TruncateExpNumber((GameData.instance.party[selectedMon].ExpToNextLevel().ToString()));
        nextleveltext.text = (GameData.instance.party[selectedMon].level < 99 ? "<LEVEL>" + (GameData.instance.party[selectedMon].level + 1).ToString() : 100.ToString());
        int id = PokemonData.MonToID(GameData.instance.party[selectedMon].name);
        pokedexno2.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        UpdateMenus();
    }
    string TruncateExpNumber(string num){
         if(num.Length > 6){
             return num.Substring(num.Length - 6); //truncate the number to the last 6 digits
        } else return num;
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
        if(currentMenu != switchstats) DisableFieldText();
    }
    void DisableFieldText(){
        
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
            highlightedmon = GameData.instance.party[selectedOption];
            partyAnimTimer += 1;
            float hpratio = (float)highlightedmon.currenthp / (float)highlightedmon.maxhp;
            float animLoopTime = hpratio > .5f ? 10 : hpratio > .21f ? 32 : 64; //the animation takes 10,32, or 64 frames
            if (partyAnimTimer == animLoopTime)
            {
                partyAnimTimer = 0;
            }

            foreach (Pokemon pokemon in GameData.instance.party)
            {
                Transform slottransform = partyslots[GameData.instance.party.IndexOf(pokemon)].transform;
                if (GameData.instance.party.IndexOf(pokemon) != selectedOption)
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.PokemonPartySprite[pokemon.name]].anim[0];
                }
                else
                {
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.PokemonPartySprite[pokemon.name]].anim[Mathf.FloorToInt(2f*partyAnimTimer / animLoopTime)];
                }
            }

          
            cursor.SetPosition(0,128 - 16 * selectedOption);


            if(Dialogue.instance.finishedText || selectingPokemon){
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


        }

        if (Inputs.pressed("b") && Dialogue.instance.finishedText)
        {
            SoundManager.instance.PlayABSound();
            if (currentMenu == mainwindow)
            {
                Inputs.instance.DisableForSeconds("b", 0.2f);
                Dialogue.instance.fastText = false;
               Dialogue.instance.Deactivate();
                Inputs.Enable("start");
                moon.currentmenu = moon.thismenu;
                this.gameObject.SetActive(false);
            
            }
            else if(currentMenu == switchstats){
                selectedOption = selectedMon;
                currentMenu = mainwindow;
                UpdateMainMenu();

            }
        }
        if (Inputs.pressed("a")&& Dialogue.instance.finishedText)
        {
            SoundManager.instance.PlayABSound();
            if (currentMenu == mainwindow)
            {
                if (!switching)
                {

                    int textOffsetLength = 4;
                    switchMenuOffsetX = 0;
                    switchMenuOffset = 0;
                    int numberOfFieldMoves = 0;
                    int selectedMenu = 0;
                    selectedMon = selectedOption;
                    Pokemon selectedPokemon = GameData.instance.party[selectedMon];
                    for(int i = 0; i < 4; i++){
                        fieldMoveNames[i] = "";
                        fieldMoveObj[i].SetActive(false);
                    }
                    for(int i = 0; i < 4; i++){
                        if(selectedPokemon.slotHasMove(i)){
                        Move move = selectedPokemon.moves[i];
                    if(isFieldMove(move)) {
                        numberOfFieldMoves++;
                        if(move.name.Length > 6 && selectedMenu != 8){
                         selectedMenu = 4;
                       textOffsetLength = 2;
                     } 
                   if(move.name.Length > 8){
                          selectedMenu = 8;
                      textOffsetLength = 0;
                      }
                        fieldMoveNames[switchMenuOffset] = move.name;
                        fieldMoveObj[3-switchMenuOffset].SetActive(true);
                         for(int j = 0; j < textOffsetLength; j++){
                          fieldMoveText[3 - switchMenuOffset].text += " ";  
                        }
                        
                           switchMenuOffset++;
                    }
                        }
                    }
                    for(int i = 0; i < numberOfFieldMoves; i++){
                        fieldMoveText[(4 - switchMenuOffset) + i].text += fieldMoveNames[i].ToUpper();
                       
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
                    Dialogue.instance.Deactivate();
                    StartCoroutine(Switch());
                    UpdateScreen();
                    StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
                    Dialogue.instance.finishedText = true;
                    switching = false;
                }
         
            }
           else if (currentMenu == switchstats)
            {

                if(selectedOption < switchMenuOffset){
                StartCoroutine(UseFieldMove(fieldMoveNames[selectedOption]));

                }
                    if (selectedOption == switchMenuOffset)
                    {
                        SoundManager.instance.SetMusicLow();
                        SoundManager.instance.PlayCry(PokemonData.MonToID(GameData.instance.party[selectedMon].name) - 1);
                       Dialogue.instance.Deactivate();
                        currentMenu = stats1;
                        UpdateStats1();


                    }
                    else if (selectedOption == switchMenuOffset + 1)
                    {
                        switching = true;
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;
                        StartCoroutine(Dialogue.instance.text("Move #MON\\lwhere?"));
                    Dialogue.instance.finishedText = true;
                        UpdateMainMenu();


                    }
                    else if (selectedOption == switchMenuOffset + 2)
                    {
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

                
                    SoundManager.instance.SetMusicNormal();
                   Dialogue.instance.Deactivate();
                   Dialogue.instance.fastText = true;
                   Dialogue.instance.keepTextOnScreen = true;
                   Dialogue.instance.needButtonPress = false;
                    StartCoroutine(Dialogue.instance.text("Choose a POKéMON."));
                    Dialogue.instance.finishedText = true;
                    selectedOption = selectedMon;
                    currentMenu = mainwindow;
                    UpdateMainMenu();

            }



        }

    }
    IEnumerator UseFieldMove(string whatFieldMove)
    {
        string monName = GameData.instance.party[selectedMon].nickname;

if(whatFieldMove == "Cut"){
if(Player.instance.facingTree){
    currentMenu = mainwindow;
    UpdateMenus();
    CloseMenu();
    Player.instance.Cut(monName);
    this.gameObject.SetActive(false);
     yield return 0;
}else {
    currentMenu = mainwindow;
    UpdateMainMenu();
yield return Dialogue.instance.text("There isn't\\lanything to CUT!");
}

}
if(whatFieldMove == "Surf"){
    Player.instance.UpdateFacedTile();
    if(Player.instance.facedTile.hasTile && Player.instance.facedTile.isWater){
    SoundManager.instance.PlaySong(17);
    currentMenu = mainwindow;
    UpdateMainMenu();
    yield return Dialogue.instance.text(GameData.instance.playerName + " got on\\l"+ monName + "!");
    Player.instance.Surf();
         CloseMenu();
    this.gameObject.SetActive(false);
    yield return 0;
    }else{
        currentMenu = mainwindow;
        UpdateMainMenu();
yield return Dialogue.instance.text("No SURFing on\\l" + monName + " here!");

    }
   
}
if(whatFieldMove == "Softboiled"){
     SoftboiledLoop:
selectingPokemon = true;
currentMenu = mainwindow;
UpdateMainMenu();
Dialogue.instance.fastText = true;
Dialogue.instance.keepTextOnScreen = true;
Dialogue.instance.needButtonPress = false;
StartCoroutine(Dialogue.instance.text("Use item on which\\l#MON?"));
while(selectingPokemon){
   
    yield return new WaitForSeconds(0.01f);
    if(Inputs.pressed("b")) yield return 0;
    if(Inputs.pressed("a")) break;
}
selectingPokemon = false;
Pokemon pokemon = GameData.instance.party[selectedOption];
if(selectedOption != selectedMon){
if(pokemon.currenthp != pokemon.maxhp){
    int amount = GameData.instance.party[selectedMon].maxhp / 5;
   yield return AnimateOurHealth(-amount,selectedMon);
    yield return AnimateOurHealth(amount,selectedOption);
    yield return Dialogue.instance.text(pokemon.nickname +"\\lrecovered by " + amount + "!");
}else {
yield return NoEffectText();
}
}else goto SoftboiledLoop;

}
currentMenu = mainwindow;
UpdateMainMenu();


    }
    IEnumerator NoEffectText(){

        yield return Dialogue.instance.text("It won't have any\\leffect.");
    
    }
    
    IEnumerator Switch()
    {
        //Swap selected Pokemon.
        Pokemon pokemon = GameData.instance.party[selectedOption];
        GameData.instance.party[selectedOption] = GameData.instance.party[selectedMon];
        GameData.instance.party[selectedMon] = pokemon;
        yield return null;

    }

    IEnumerator AnimateOurHealth(int amount, int i)
    {
        if (amount + GameData.instance.party[i].currenthp < 0) amount = GameData.instance.party[i].currenthp;
        if (amount + GameData.instance.party[i].currenthp > GameData.instance.party[i].maxhp) amount = GameData.instance.party[i].maxhp - GameData.instance.party[i].currenthp;
        int result = amount;
        WaitForSeconds wait = new WaitForSeconds(5 / GameData.instance.party[i].maxhp);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            GameData.instance.party[i].currenthp += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].currenthp * 48 / (float)GameData.instance.party[i].maxhp);
            healthbars[i].fillAmount = (float)pixelCount / 48;
            UpdateScreen();
        }
        yield return null;

    }
    public bool hasFieldMove(Pokemon pokemon){
        foreach(Move move in pokemon.moves){
   if(GameData.instance.fieldMoves.Contains(move.name)) return true;
        }
        return false;
    }
    public bool isFieldMove(Move move){
   if(GameData.instance.fieldMoves.Contains(move.name)) return true;
    else return false;
    }
    public void CloseMenu(){
        Inputs.instance.DisableForSeconds("b", 0.2f);
        Dialogue.instance.fastText = false;
    Dialogue.instance.Deactivate();
    Inputs.Enable("start");
     moon.currentmenu = moon.thismenu;

    }




}

