using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Title : MonoBehaviour {
	public bool HasData;
	public GameObject startmenu;
	public GameObject nodatamenu, datamenu, continuemenu, options;
    public GameCursor cursor;
	public GameObject[] startmenus;
	public GameObject currentMenu;
	public GameObject[] menuSlots;
	public Animator titleanim, redAnim, pokemonAnim;
	public int selectedOption;
	public GameObject tutorialmanager;
	public Options opt;
    public int ChosenPokemon;
    public Sprite[] blueMons, redMons;
    public Image pokemonImage, titleVersionImage;
    public float titleAnimTimer;
    public bool switchingPokemon;
    public Sprite redVersionText, blueVersionText;
    public bool animationsFinished, inStartMenu;
    public AudioClip crashSound, whooshSound;
    public static Title instance;
    public TitlePokemon titlePokemon;
    // Use this for initialization
    void Start () {
		startmenu.SetActive (false);
        Init();
    }
    public void SetAnimationFinished()
    {
        animationsFinished = true;
        SoundManager.instance.PlaySong(19);
    }
    public void InitVersion()
    {
        switch (GameData.instance.version)
        {
            case Version.Red:
                titleVersionImage.sprite = redVersionText;
                break;
            case Version.Blue:
                titleVersionImage.sprite = blueVersionText;
                break;
        }
        ChangePokemon();
    }
    public void Init()
    {
        GameData.instance.atTitleScreen = true;
        titleanim.Play("titleAnim");
    }
    public void ChangePokemon()
    {
        switch (GameData.instance.version)
        {
            case Version.Red:
                pokemonImage.sprite = redMons[ChosenPokemon];
                break;
            case Version.Blue:
                pokemonImage.sprite = blueMons[ChosenPokemon];
                break;
        }
    }
    public void SelectPokemon()
    {
        int lastPokemon = ChosenPokemon;
        ChosenPokemon = Random.Range(0, 16);
        while(ChosenPokemon == lastPokemon) ChosenPokemon = Random.Range(0, 16);
        ChangePokemon();

    }
    public void TitleAnim()
    {
        pokemonAnim.SetTrigger("switchPokemon");
        Invoke("TryTossPokeball", 0.6f);
        
    }
    public void TryTossPokeball()
    {
        if (ChosenPokemon < 3) redAnim.SetTrigger("tossPokeball");
    }

    // Update is called once per frame
    void Update () {
        if (!animationsFinished) return;
		if (currentMenu == nodatamenu || currentMenu == datamenu) {

            int limit = (currentMenu == nodatamenu ? 1 : 2);
		
            if (Inputs.pressed("down")) {
				selectedOption++;
                
			}
            if (Inputs.pressed("up")) {
				selectedOption--;
                
            }
			if (selectedOption < 0) {
				selectedOption = 0;

			}
		    if (selectedOption > limit) {
				selectedOption = limit;

			}
            cursor.SetPosition(8, 120 - selectedOption * 16);
        }
		if(Inputs.pressed("a") || Inputs.pressed("start")){
            if (currentMenu == null && !titlePokemon.isMoving) {
               
				StartCoroutine("GotoStart");

			}
        }
			if (Inputs.pressed("a")) {
			
			if (currentMenu == nodatamenu && selectedOption == 0) {
				tutorialmanager.SetActive (true);
                BeginHandler.instance.Init();
                SoundManager.instance.PlaySong(15);
				startmenu.SetActive (false);
				this.gameObject.SetActive (false);

			}
			if (currentMenu == nodatamenu && selectedOption == 	1) {
                Options.instance.Init();
				options.SetActive (true);
				currentMenu = options;

			}
			if (currentMenu == datamenu && selectedOption == 	2) {
                Options.instance.Init();
                options.SetActive (true);
				currentMenu = options;

			}
			}
		if (Inputs.pressed("b") && startmenu.activeInHierarchy) {
			if ((currentMenu == nodatamenu || currentMenu == datamenu)) {
				startmenu.SetActive (false);
				currentMenu = null;
                inStartMenu = false;
                ChosenPokemon = 0;
                ChangePokemon();
                cursor.SetActive(false);
                animationsFinished = false;
                titleAnimTimer = 0;
                pokemonAnim.enabled = true;
                pokemonAnim.Play("titlePokemonIdle");
                titleanim.Play("titleAnim",0,0);

            }
			if (currentMenu == options) {
				currentMenu = nodatamenu;
				selectedOption = 0;
			}
		}
			foreach (GameObject menu in startmenus) {
				if (menu != currentMenu) {
					menu.SetActive (false);
				} else {

					menu.SetActive (true);
				}


			}
            if (!switchingPokemon && !inStartMenu) titleAnimTimer += Time.deltaTime;
            if (titleAnimTimer >= 3.33f)
            {
                TitleAnim();
                titleAnimTimer = 0;
            }
        


    }
	public IEnumerator GotoStart(){
        inStartMenu = true;
        pokemonAnim.enabled = false;
        int pokemonIndex = int.Parse(pokemonImage.sprite.name);
        yield return StartCoroutine(SoundManager.instance.PlayCryCoroutine(pokemonIndex - 1));
		startmenu.SetActive (true);
        GameCursor.instance.SetActive(true);
		if (!HasData) {
			currentMenu = nodatamenu;

		} else {

			currentMenu = datamenu;
		}
	}
    public void PlayWhooshSound()
    {
        SoundManager.instance.sfx.PlayOneShot(whooshSound);

    }
    public void PlayCrashSound()
    {
        SoundManager.instance.sfx.PlayOneShot(crashSound);

    }



}
