using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public Player play;

	public int selectedOption;
	public GameObject[] menuSlots;
	public GameObject cursor;
	public GameObject pokemonmenu, bagmenu, badgesmenu, thismenu, optionsmenu, pokedexmenu;
	public GameObject currentmenu;
	public Options opti;
    public Get get = new Get();
    private bag lag;
    private Pokedex pokedex;
	public bool donewaiting;
	public PokemonData pk;
    public CustomText playername;
	// Use this for initialization
	public void Initialize(){
		donewaiting = true;
		currentmenu = thismenu;
	}
    private void Awake()
    {
        lag = get.Bag();
        pokedex = get.pokedex();
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
            playername.text = Dialogue.Name;
			menuSlots = new GameObject[currentmenu.transform.childCount];

			for (int i = 0; i < currentmenu.transform.childCount; i++) {
				

				menuSlots [i] = currentmenu.transform.GetChild (i).gameObject;
			}

			cursor.SetActive (true);
			cursor.transform.position = menuSlots [selectedOption].transform.position;



            if (Inputs.pressed("down")) {
				selectedOption++;
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
			}
			if (selectedOption < 0) {
				selectedOption = menuSlots.Length - 1;

			}
			if (selectedOption == menuSlots.Length) {
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

                            pokedex.Init();
                            donewaiting = false;
                        }
					}
					if (selectedOption == 1) {
						if (currentmenu == thismenu) {
							currentmenu = pokemonmenu;
							pk.currentMenu = pk.mainwindow;
							pk.selectedOption = 0;
                            pokemonmenu.SetActive(true);
							pk.Initialize ();
							
							donewaiting = false;
						}
					}
					if (selectedOption == 2) {
					

						if (currentmenu == thismenu) {
							lag.selectBag = -1;
							currentmenu = bagmenu;
							bagmenu.SetActive (true);
                            lag.didFirstRunthrough = false;
                            StartCoroutine(bagmenu.GetComponent<bag>().Wait());
							donewaiting = false;

						}
					}
				}
				if (selectedOption == 3) {
					currentmenu = badgesmenu;
					badgesmenu.SetActive (true);
					donewaiting = false;
				}
				if (selectedOption == 4) {

				}
				if (selectedOption == 5) {
					opti.selectedOption = 0;
					if (currentmenu == thismenu) {
						optionsmenu.SetActive (true);
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

				optionsmenu.SetActive (false);
				currentmenu = thismenu;

			}
		}
	}

}
