using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PartyAnim //maybe make this into a struct instead? (and other similar small classes with only variables) do more research and experimentation w/ structs in the future
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
            MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
            this.gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < GameData.instance.party.Count; i++)
        {
            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].currentHP * 48 / (float)GameData.instance.party[i].maxHP);
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
            int pixelCount = Mathf.RoundToInt((float)pokemon.currentHP * 48 / (float)pokemon.maxHP);
            slottransform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = (float)pixelCount / 48;
            slottransform.GetChild(2).GetComponent<CustomText>().text = pokemon.nickname;
            slottransform.GetChild(3).GetComponent<CustomText>().text = ((pokemon.level < 100) ? "<LEVEL>" : "") + pokemon.level.ToString();
            slottransform.GetChild(4).GetComponent<CustomText>().text = (pokemon.currentHP > 99 ? "" : pokemon.currentHP > 9 ? " " : "  ") + pokemon.currentHP + (pokemon.maxHP > 99 ? "/" : pokemon.maxHP > 9 ? "/ " : "/  ") + pokemon.maxHP;
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
        stats1portrait.sprite = GameData.instance.frontMonSprites[GameData.instance.party[selectedMon].id - 1];
        int id = GameData.instance.party[selectedMon].id;
        pokedexNO.text = (id > 99 ? "" : id > 9 ? "0" : "00") + id.ToString();
        attacktext.text = GameData.instance.party[selectedMon].attack.ToString();
        speedtext.text = GameData.instance.party[selectedMon].speed.ToString();
        specialtext.text = GameData.instance.party[selectedMon].special.ToString();
        defensetext.text = GameData.instance.party[selectedMon].defense.ToString();
        Types type1 = GameData.instance.party[selectedMon].types[0], type2 = GameData.instance.party[selectedMon].types[1];
        montype1.text = PokemonData.GetTypeName(type1);
        string type2String = PokemonData.GetTypeName(type2);
        montype2.text = type2String != "" ? ("TYPE2/" + "\n " + type2String) : "";
        monnametext.text = GameData.instance.party[selectedMon].nickname;
        owneridtext.text = GameData.instance.party[selectedMon].ownerID.ToString();
        ownernametext.text = GameData.instance.party[selectedMon].owner;
        int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[selectedMon].currentHP * 48 / (float)GameData.instance.party[selectedMon].maxHP);
        stat1bar.fillAmount = (float)pixelCount / 48;
        monhptext.text = (GameData.instance.party[selectedMon].currentHP > 99 ? "" : GameData.instance.party[selectedMon].currentHP > 9 ? " " : "  ") + GameData.instance.party[selectedMon].currentHP + " " + GameData.instance.party[selectedMon].maxHP;
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
            if (GameData.instance.party[selectedMon].SlotHasMove(i))
            {
                Move move = GameData.instance.party[selectedMon].moves[i];
                movestr += (i > 0 ? "\n" : "") + move.name.ToUpper() + "\n" + "         " + "PP "  + (move.pp < 10 ? " " : "") + move.pp +  "/" + (move.maxpp < 10 ? " " : "") + move.maxpp;
            }
            else
                movestr += (i > 0 ? "\n" : "") + "-" + "\n" + "         " + "--";
        }
        movetext.text = movestr;
        stats2portrait.sprite = GameData.instance.frontMonSprites[GameData.instance.party[selectedMon].id - 1];
        monname2text.text = GameData.instance.party[selectedMon].nickname;
        
        exptext.text = TruncateExpNumber(GameData.instance.party[selectedMon].experience.ToString());
        explefttoleveltext.text =  TruncateExpNumber((GameData.instance.party[selectedMon].ExpToNextLevel().ToString()));
        nextleveltext.text = (GameData.instance.party[selectedMon].level < 99 ? "<LEVEL>" + (GameData.instance.party[selectedMon].level + 1).ToString() : 100.ToString());
        int id = GameData.instance.party[selectedMon].id;
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
            float hpratio = (float)highlightedmon.currentHP / (float)highlightedmon.maxHP;
            float animLoopTime = hpratio > .5f ? 10 : hpratio > .21f ? 32 : 64; //the animation takes 10,32, or 64 frames
            if (partyAnimTimer == animLoopTime)
            {
                partyAnimTimer = 0;
            }

            foreach (Pokemon pokemon in GameData.instance.party)
            {
                Transform slottransform = partyslots[GameData.instance.party.IndexOf(pokemon)].transform;
                if (GameData.instance.party.IndexOf(pokemon) != selectedOption){
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.pokemonData[pokemon.id - 1].partySprite].anim[0];
                }else{
                    slottransform.GetChild(0).GetComponent<Image>().sprite = partyanims[PokemonData.pokemonData[pokemon.id - 1].partySprite].anim[Mathf.FloorToInt(2f*partyAnimTimer / animLoopTime)];
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
                MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
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
                        if(selectedPokemon.SlotHasMove(i)){
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
                        SoundManager.instance.PlayCry(GameData.instance.party[selectedMon].id - 1);
                       Dialogue.instance.Deactivate();
                        currentMenu = stats1;
                        UpdateStats1();


                    }
                    else if (selectedOption == switchMenuOffset + 1)
                    {
                        switching = true;
                        selectedOption = selectedMon;
                        currentMenu = mainwindow;
                        StartCoroutine(Dialogue.instance.text("Move #MON&lwhere?"));
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
            else if (currentMenu == stats1){
                    currentMenu = stats2;
                    UpdateStats2();
            }
            else if (currentMenu == stats2){
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

    IEnumerator UseFieldMove(string whatFieldMove){
        string monName = GameData.instance.party[selectedMon].nickname;

        if(whatFieldMove == "Cut"){
            if(Player.instance.facingTree){
                currentMenu = mainwindow;
                UpdateMenus();
                CloseMenu();
                Player.instance.Cut(monName);
                this.gameObject.SetActive(false);
                yield return 0;
            }else{
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text("There isn't&lanything to CUT!");
                //TODO: implement cutting grass
            }
        }
        if(whatFieldMove == "Surf"){
            Player.instance.UpdateFacedTile();
            if(Player.instance.facedTile.hasTile && Player.instance.facedTile.isWater){
                SoundManager.instance.PlaySong(17);
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text(GameData.instance.playerName + " got on&l"+ monName + "!");
                Player.instance.Surf();
                CloseMenu();
                this.gameObject.SetActive(false);
                yield return 0;
            }else{
                currentMenu = mainwindow;
                UpdateMainMenu();
                yield return Dialogue.instance.text("No SURFing on&l" + monName + " here!");
            }
        }
        if(whatFieldMove == "Softboiled"){
            while(selectedOption == selectedMon){
                selectingPokemon = true;
                currentMenu = mainwindow;
                UpdateMainMenu();
                Dialogue.instance.fastText = true;
                Dialogue.instance.keepTextOnScreen = true;
                Dialogue.instance.needButtonPress = false;
                StartCoroutine(Dialogue.instance.text("Use item on which&l#MON?"));

                while(selectingPokemon){
                    yield return new WaitForSeconds(0.01f);
                    if(Inputs.pressed("b")) yield return 0;
                    if(Inputs.pressed("a")) break;
                }

                selectingPokemon = false;
                Pokemon pokemon = GameData.instance.party[selectedOption];

                if(selectedOption != selectedMon){
                    if(pokemon.currentHP != pokemon.maxHP){
                        int amount = GameData.instance.party[selectedMon].maxHP / 5;
                        yield return AnimateOurHealth(-amount,selectedMon);
                        yield return AnimateOurHealth(amount,selectedOption);
                        yield return Dialogue.instance.text(pokemon.nickname + "&lrecovered by " + amount + "!");
                    }else{
                        yield return NoEffectText();
                    }
                }
            }
        }

        currentMenu = mainwindow;
        UpdateMainMenu();
    }


    IEnumerator NoEffectText(){
        yield return Dialogue.instance.text("It won't have any&leffect.");
    }
    
    IEnumerator Switch(){
        //Swap selected Pokemon.
        Pokemon pokemon = GameData.instance.party[selectedOption];
        GameData.instance.party[selectedOption] = GameData.instance.party[selectedMon];
        GameData.instance.party[selectedMon] = pokemon;
        yield return null;
    }

    IEnumerator AnimateOurHealth(int amount, int i){
        if (amount + GameData.instance.party[i].currentHP < 0) amount = GameData.instance.party[i].currentHP;
        if (amount + GameData.instance.party[i].currentHP > GameData.instance.party[i].maxHP) amount = GameData.instance.party[i].maxHP - GameData.instance.party[i].currentHP;
        int result = amount;
        WaitForSeconds wait = new WaitForSeconds(5 / GameData.instance.party[i].maxHP);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            GameData.instance.party[i].currentHP += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)GameData.instance.party[i].currentHP * 48 / (float)GameData.instance.party[i].maxHP);
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
        MainMenu.instance.currentmenu = MainMenu.instance.thismenu;
    }
}