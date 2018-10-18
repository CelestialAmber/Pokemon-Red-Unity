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
	public Options opti;
    public Get get = new Get();
    private Bag bag;
    private Pokedex pokedex;
	public bool donewaiting;
	public PokemonMenu pk;
    public CustomText playername;
    public int slotNumber;
	// Use this for initialization
	public void Initialize(){
		donewaiting = true;
		currentmenu = thismenu;
	}
    public static MainMenu instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bag = Get.bag;
        pokedex = Get.pokedex;
    }
    // Update is called once per frame
    void Update () {
        if (Inputs.released("a")) {

			donewaiting = true;
		}
        if (Inputs.released("b") && !donewaiting)
        {

            donewaiting = true;
        }
		if (currentmenu == null) {
			cursor.SetActive (false);
		} else if(currentmenu == thismenu){
            playername.text = GameData.playerName;


			cursor.SetActive (true);
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
		if (currentmenu == badgesmenu) {

			cursor.SetActive (false);
		}

        if (Inputs.pressed("start")) {
			if (currentmenu == thismenu) {

				cursor.SetActive (false);
				currentmenu = null;
				play.startmenuup = false;


			}


		}
		if (Inputs.pressed("a")) {
			if (currentmenu == badgesmenu && donewaiting) {
				currentmenu = thismenu;
                Inputs.Enable("start");
				badgesmenu.SetActive (false);
				donewaiting = false;

			}

		}
		if (Inputs.pressed("a")) {
			if (donewaiting) {

				if (currentmenu == thismenu) {
					if (selectedOption == 0) {
                        if(currentmenu == thismenu){
                            currentmenu = pokedexmenu;
                            pokedexmenu.SetActive(true);
                            Inputs.Disable("start");
                            pokedex.Init();
                            donewaiting = false;
                        }
					}
					if (selectedOption == 1) {
						if (currentmenu == thismenu) {
							currentmenu = pokemonmenu;
							pk.currentMenu = pk.mainwindow;
                            Inputs.Disable("start");
							pk.selectedOption = 0;
                            pokemonmenu.SetActive(true);
							pk.Initialize ();

							donewaiting = false;
						}
					}
					if (selectedOption == 2) {


						if (currentmenu == thismenu) {
							
							bag.Initialize();
							bag.selectBag = -1;
							currentmenu = Bagmenu;
                            cursor.SetActive(true);
							Bagmenu.SetActive (true);
                            Inputs.Disable("start");
                            StartCoroutine(Bagmenu.GetComponent<Bag>().Wait());
							donewaiting = false;

						}
					}
				}
				if (selectedOption == 3) {
					currentmenu = badgesmenu;
                    Inputs.Disable("start");
                    badgesmenu.GetComponent<Badges>().Init();
					badgesmenu.SetActive (true);
					donewaiting = false;
				}
				if (selectedOption == 4) {

				}
				if (selectedOption == 5) {
					opti.selectedOption = 0;
					if (currentmenu == thismenu) {
						optionsmenu.SetActive (true);
                        Inputs.Disable("start");
						currentmenu = optionsmenu;
						donewaiting = false;


					}
				}
				if (selectedOption == 6) {
					if (currentmenu == thismenu) {
						cursor.SetActive (false);
						currentmenu = null;
						play.startmenuup = false;
						donewaiting = false;


					}
				}

			}


				if (currentmenu == optionsmenu) {
					if (opti.selectedOption == 3) {
                    Inputs.Enable("start");
						optionsmenu.SetActive (false);
						currentmenu = thismenu;
					donewaiting = false;

					}

				}



			donewaiting = false;
		}
		if (Inputs.pressed("b")) {
			if (currentmenu == thismenu && donewaiting) {
                Debug.Log("x press");
				cursor.SetActive (false);
				currentmenu = null;
				play.startmenuup = false;



			}
			if (currentmenu == optionsmenu) {
                Inputs.Enable("start");
				optionsmenu.SetActive (false);
				currentmenu = thismenu;

			}
		}
	}

}
