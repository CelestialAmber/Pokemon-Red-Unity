using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Singleton<MainMenu> {
	public int selectedOption;
	public GameObject[] menuSlots;
	public GameCursor cursor;
	public GameObject pokemonmenu, Bagmenu, badgesmenu, thismenu, optionsmenu, pokedexmenu;
	public GameObject currentmenu;
	private Options options;
    private Pokedex pokedex;
    public Bag bag;
	private PokemonMenu pokemonMenu;
    public CustomText playername;
    public int slotNumber;



	public void Initialize(){
        thismenu.SetActive(true);
        playername.gameObject.SetActive(true);
		currentmenu = thismenu;
        if(cursor == null) throw new UnityException("The cursor is null!");
		cursor.SetActive(true);
	}

    private void Start(){
        pokedex = Pokedex.instance;
        options = Options.instance;
        pokemonMenu = PokemonMenu.instance;
    }

    // Update is called once per frame
    void Update() { 
		if(currentmenu == null){
			cursor.SetActive(false);
		}
        else if(currentmenu == thismenu){
            playername.text = GameData.instance.playerName;
            cursor.SetPosition(88, 120 - 16 * selectedOption);

            if (InputManager.Pressed(Button.Down)) {
				selectedOption++;
			}
            if (InputManager.Pressed(Button.Up)) {
				selectedOption--;
			}
			if (selectedOption < 0) {
				selectedOption =  slotNumber - 1;

			}
			if (selectedOption == slotNumber) {
				selectedOption = 0;

			}
		}
		

        if (InputManager.Pressed(Button.Start)) {
			if (currentmenu == thismenu) {
				Close();
			}
		}
		
		if(InputManager.Pressed(Button.A)){
            if(currentmenu == thismenu){

                SoundManager.instance.PlayABSound();
                
                if (selectedOption == 0){
                    currentmenu = pokedexmenu;
                    pokedexmenu.SetActive(true);
                    InputManager.Disable(Button.Start);
                    pokedex.Init();  
                }
                if (selectedOption == 1){
                    currentmenu = pokemonmenu;
                    pokemonMenu.currentMenu = pokemonMenu.mainwindow;
                    InputManager.Disable(Button.Start);
                    pokemonMenu.selectedOption = 0;
                    pokemonmenu.SetActive(true);
                    pokemonMenu.Initialize();  
                }
                if (selectedOption == 2){
                    //Bag.instance.Initialize();
                    //Bag.instance.selectBag = -1;
                    bag.Initialize();
                    bag.selectBag = -1;
                    currentmenu = Bagmenu;
                    cursor.SetActive(true);
                    Bagmenu.SetActive(true);
                    InputManager.Disable(Button.Start);
                }

                if (selectedOption == 3){
                    currentmenu = badgesmenu;
                    cursor.SetActive(false);
                    InputManager.Disable(Button.Start);
                    badgesmenu.GetComponent<Badges>().Init();
                    badgesmenu.SetActive(true);

                }
                if (selectedOption == 4){

                }
                if (selectedOption == 5){
                    options.Init();
                    optionsmenu.SetActive(true);
                    InputManager.Disable(Button.Start);
                    currentmenu = optionsmenu;
                }
                if (selectedOption == 6){
                        Close();    
                }
            }	
		}

		if (InputManager.Pressed(Button.B)) {
			if (currentmenu == thismenu) {
				SoundManager.instance.PlayABSound();
				Close();
			}
		}
	}

    public void Close(){
        cursor.SetActive(false);
        thismenu.SetActive(false);
        playername.gameObject.SetActive(false);
        currentmenu = null;
        Player.instance.startMenuActive = false;
        //this.gameObject.SetActive(false);
        this.enabled = false;
    }

}