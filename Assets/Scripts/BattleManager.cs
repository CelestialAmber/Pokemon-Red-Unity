using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class BattleMon{
    public int defense;
    public int attack;
    public int speed;
    public int special;
    public int hp;
    public int maxhp;
    public int level;
    public Status status;
    public string pokename;
    public string name;
}
public enum BattleType{
    Wild,
    Trainer,
    Safari,
    OldMan
}
public class BattleManager : MonoBehaviour {
    public int battleID;

    public List<Pokemon> enemyMons = new List<Pokemon>();
	public Dialogue mylog;
	public PokemonMenu pkmr;
	public GameObject actualscene, ourstats, opponentstats;
	public Animator battletransitionAnim, otheranim;
	public bool donetransition;
	public GameObject ourpokeballs, opponentballs;
	public Image battleoverlay, frontportrait, backportrait;
	public Sprite blank, initial, enemystats, allstats;
	//current loaded your mon stats
	public string friendpokemonnameinslot;
    public Pokemon ourmon;
	public CustomText friendHPtext;
	public CustomText friendmaxHPtext;
	public CustomText friendmonLeveltext;
    public CustomText friendName;
	public int oldfriendhealth;
	public Image friendHPBar;
	public float friendsavedhealthpixels;
	//current loaded enemymon stats

	public string enemypokemonnameinslot;
    public Pokemon enemymon;
	public int oldfoehealth;
	public CustomText foemonLeveltext;
    public CustomText foemonname;
	public Image foeHPBar;
	public float foesavedhealthpixels;
	//
	public bool readytobattle;
	public GameObject currentmenu;
	public GameObject battlemenu, movesmenu;
	public GameObject[] allmenus;
	public Cursor cursor;
	public GameObject[] menuSlots;
	public int selectedOption;
	public int currentLoadedMon;
	// Use this for initialization
	void Start(){
	}
	public void Initialize(){
        enemyMons.Clear();
        switch(battleID){
            case 0:
                enemyMons = new List<Pokemon>(
                    new Pokemon[]{
                        new Pokemon("Bulbasaur",90)
                    }
                    );
                break;
                
        }
		currentLoadedMon = 0;
        foreach (Pokemon pokemon in enemyMons){
            pokemon.RecalculateStats();
        }
        enemymon = enemyMons[0];
        ourmon = pkmr.party[0];
         ourmon.RecalculateStats();
        friendsavedhealthpixels = Mathf.Round (ourmon.currenthp * 48 / ourmon.hp);
        foesavedhealthpixels = Mathf.Round (enemymon.currenthp * 48 / enemymon.hp);
        foeHPBar.fillAmount = (Mathf.Round(enemymon.currenthp * 48 / enemymon.hp))/48;
        friendHPBar.fillAmount = (Mathf.Round(ourmon.currenthp * 48 / ourmon.hp))/48;
        friendName.text = ourmon.name;
		ourstats.SetActive (false);
		opponentstats.SetActive (false);
		ourpokeballs.SetActive(false);
		opponentballs.SetActive (false);
        if (pkmr.party[0].level - enemyMons[0].level <= -3) {
			battletransitionAnim.SetInteger ("transitiontype",1);
		}
        if (pkmr.party[0].level - enemyMons[0].level >= -2) {
			battletransitionAnim.SetInteger ("transitiontype",2);
		}




	}
	void DoneWithTransition(){

		donetransition = true;

	}
	public IEnumerator PokeballShow(){
		battleoverlay.sprite = initial;

		ourpokeballs.SetActive(true);
		opponentballs.SetActive (true);
		yield return StartCoroutine(mylog.text ("TRAINER wants to battle!"));
		yield return StartCoroutine(mylog.done ());

		ourpokeballs.SetActive(false);
		opponentballs.SetActive (false);
		otheranim.SetInteger ("currentpass", 1);

	}
    public IEnumerator WildPokemonAppeared(){

        yield return 0;

    }
	
	// Update is called once per frame
	void Update () {
		
		foreach (GameObject menu in allmenus) {
			if (menu != currentmenu) {
				menu.SetActive (false);
			} else {

				menu.SetActive (true);
			}


		}
		if (donetransition) {


			foemonLeveltext.text = enemymon.level.ToString();
			friendmonLeveltext.text = ourmon.level.ToString ();
			friendHPtext.text = ourmon.currenthp.ToString ();
			friendmaxHPtext.text = ourmon.hp.ToString ();
            foemonname.text = enemymon.name;
			actualscene.SetActive (true);
			if (readytobattle) {
				
				currentmenu = battlemenu;
				if (currentmenu == battlemenu) {

					menuSlots = new GameObject[currentmenu.transform.childCount];

					for (int i = 0; i < currentmenu.transform.childCount; i++) {


						menuSlots [i] = currentmenu.transform.GetChild (i).gameObject;
					}
				
				}
				if (currentmenu == battlemenu) {
					
					cursor.SetActive (true);
					cursor.transform.position = menuSlots [selectedOption].transform.position;




					if (Inputs.pressed("left")) {
						
						if (selectedOption == 1) {
							selectedOption = 0;
							return;
						}

						if (selectedOption == 3) {
							selectedOption = 2;
							return;
						}
					}
                    if (Inputs.pressed("right")) {
						
						if (selectedOption == 0) {
							selectedOption = 1;
							return;
						}

						if (selectedOption == 2) {
							selectedOption = 3;
							return;
						}
					}
                    if (Inputs.pressed("up")) {
						if (selectedOption == 2) {
							selectedOption = 0;
							return;
						}
						if (selectedOption == 3) {
							selectedOption = 1;
							return;
						}




					}
                    if (Inputs.pressed("down")) {
						if (selectedOption == 0) {
							selectedOption = 2;
							return;
						}
						if (selectedOption == 1) {
							selectedOption = 3;
							return;
						}



					}



				}
			} else {

				currentmenu = null;

			}
		}
	
	}
	public void DetermineFrontSprite(){
        frontportrait.overrideSprite = GameData.frontMonSprites[PokemonData.MonToID(enemymon.pokename) - 1];

	}

	public void DetermineBackSprite(){
        backportrait.overrideSprite = GameData.backMonSprites
            [PokemonData.MonToID(ourmon.pokename) - 1];

	}
	public void ActivateOurStats(){
		ourstats.SetActive (true);
		battleoverlay.sprite = allstats;
	}
	public void ActivateTheirStats(){
		opponentstats.SetActive (true);
		battleoverlay.sprite = enemystats;
	}
	//these functions animate health.


    IEnumerator AnimateOurHealth(int amount)
    {
        int newHealth = ourmon.currenthp + amount;
        if (newHealth < 0) newHealth = 0;
        if (newHealth > ourmon.hp) newHealth = ourmon.hp;
        int result = Mathf.RoundToInt(newHealth - ourmon.currenthp);

        WaitForSeconds wait = new WaitForSeconds(5 / ourmon.hp);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            ourmon.currenthp += 1 * Mathf.Clamp(result, -1, 1);
            int pixelCount = Mathf.RoundToInt((float)ourmon.currenthp * 48 / (float)ourmon.hp);
            friendHPBar.fillAmount = (float)pixelCount / 48;

        }
        yield return null;

    }
    IEnumerator AnimateEnemyHealth(int amount)
    {
        int newHealth = enemymon.currenthp + amount;
        if (newHealth < 0) newHealth = 0;
        if (newHealth > enemymon.hp) newHealth = enemymon.hp;
        int result = Mathf.RoundToInt(newHealth - enemymon.currenthp);
        WaitForSeconds wait = new WaitForSeconds(5 / enemymon.hp);

        for (int l = 0; l < Mathf.Abs(result); l++)
        {
            yield return wait;

            enemymon.currenthp += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)enemymon.currenthp * 48 / (float)enemymon.hp);
   foeHPBar.fillAmount = (float)pixelCount / 48;

        }
        yield return null;

    }

    void ShowBall()
    {
        StartCoroutine(PokeballShow());
    }
    void DetermineFront()
    {
        DetermineFrontSprite();
    }
    void DetermineBack()
    {
        DetermineBackSprite();
    }
    void ReadyToBattle()
    {
        
        readytobattle = true;
        selectedOption = 0;
    }
    void ActivateStatsOur()
    {
        ActivateOurStats();
    }
    void ActivateStatsTheir()
    {
        ActivateTheirStats();
    }
	


	}

