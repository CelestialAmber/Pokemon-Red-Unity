using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public Player play;

	public int selectedOption;
	public GameObject[] menuSlots;
	public GameCursor cursor;
	public GameObject pokemonmenu, Bagmenu, badgesmenu, thismenu, optionsmenu, pokedexmenu;
	public GameObject currentmenu;
	private Options options;
    private Bag bag;
    private Pokedex pokedex;
	private PokemonMenu pokemonMenu;
    public CustomText playername;
    public int slotNumber;
	// Use this for initialization
	public void Initialize(){
		currentmenu = thismenu;
		cursor.SetActive(true);
	}
    public static MainMenu instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bag = Bag.instance;
        pokedex = Pokedex.instance;
        options = Options.instance;
        pokemonMenu = PokemonMenu.instance;
        


    }
    // Update is called once per frame
    void Update () {
       
		if (currentmenu == null) {
			cursor.SetActive (false);
		} else if(currentmenu == thismenu){
            playername.text = GameData.instance.playerName;
            cursor.SetPosition(88, 120 - 16 * selectedOption);



            if (Inputs.pressed("down")) {
				selectedOption++;
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
			}
			if (selectedOption < 0) {
				selectedOption =  slotNumber - 1;

			}
			if (selectedOption == slotNumber) {
				selectedOption = 0;

			}
		}
		

        if (Inputs.pressed("start")) {
			if (currentmenu == thismenu) {

				Close();

			}


		}
		
		if (Inputs.pressed("a")) {


             if (currentmenu == thismenu)
            {
                SoundManager.instance.PlayABSound();
                if (selectedOption == 0)
                {
                 
                        currentmenu = pokedexmenu;
                        pokedexmenu.SetActive(true);
                        Inputs.Disable("start");
                        pokedex.Init();

                    
                }
                if (selectedOption == 1)
                {
                   
                        currentmenu = pokemonmenu;
                        pokemonMenu.currentMenu = pokemonMenu.mainwindow;
                        Inputs.Disable("start");
                        pokemonMenu.selectedOption = 0;
                        pokemonmenu.SetActive(true);
                        pokemonMenu.Initialize();


                    
                }
                if (selectedOption == 2)
                {

                        bag.Initialize();
                        bag.selectBag = -1;
                        currentmenu = Bagmenu;
                        cursor.SetActive(true);
                        Bagmenu.SetActive(true);
                        Inputs.Disable("start");
                }

                if (selectedOption == 3)
                {
                    currentmenu = badgesmenu;
                    cursor.SetActive(false);
                    Inputs.Disable("start");
                    badgesmenu.GetComponent<Badges>().Init();
                    badgesmenu.SetActive(true);

                }
                if (selectedOption == 4)
                {

                }
                if (selectedOption == 5)
                {
                    options.Init();
          
                        optionsmenu.SetActive(true);
                        Inputs.Disable("start");
                        currentmenu = optionsmenu;

                    
                }
                if (selectedOption == 6)
                {
                   
                        Close();
                    
                }
            }

			


				



			
		}
		if (Inputs.pressed("b")) {
			if (currentmenu == thismenu) {
				SoundManager.instance.PlayABSound();
				Close();


			}
		}
	}
public void Close(){
cursor.SetActive (false);
currentmenu = null;
play.startMenuActive = false;
this.gameObject.SetActive(false);
}
}
