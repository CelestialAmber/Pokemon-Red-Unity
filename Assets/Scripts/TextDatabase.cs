using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDatabase : MonoBehaviour {
	public GameObject itemPCMenu, shopmenu, slotmenu;
	public Bag bag;
    public GameCursor cursor;
	public Items itemDatabase;
	public PokeMart pokeMart;
	public Player player;
	public PokemonMenu pokemonData;
	public Slots slots;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void PlayText(int ID, int amount){
		switch (ID) {
		case 1:
		//	if (play.direction == 3 || play.direction == 4) {
				StartCoroutine (Text1 ());
		//	}
			break;
		case 2:
			StartCoroutine (Text2 ());
			break;
		case 3:
			StartCoroutine (Text3 ());
			break;
		case 4:
			StartCoroutine (Text4 ());
			break;
		case 5:
			StartCoroutine (Text5 ());
			break;

		case 7:
			StartCoroutine (Text7 ());
			break;
            case 8:
                StartCoroutine(Text8());
                break;
		}




	}
    public void GetItem(string item, int coinAmount){
        if (coinAmount == 0)
            StartCoroutine(GetItemText(item));
        else StartCoroutine(FoundCoinsText(coinAmount));
    }

    IEnumerator GetItemText(string item){
        itemDatabase.AddItem(item, 1);
        yield return StartCoroutine(Dialogue.instance.text(GameData.playerName + " found "));
        yield return StartCoroutine(Dialogue.instance.line(item.ToUpper() + "!"));
        yield return StartCoroutine(Dialogue.instance.done());


    }
	IEnumerator Text1(){
		if (player.direction == 3 || player.direction == 4) {
			if (GameData.coins > 0) {
				yield return StartCoroutine(Dialogue.instance.text ("A slot machine!"));
				yield return StartCoroutine(Dialogue.instance.line ("Want to play?"));
                yield return StartCoroutine(Dialogue.instance.prompt ());
				if (Dialogue.instance.selectedOption == 0) {
					Dialogue.instance.Deactivate ();
					Player.disabled = true;
					StartCoroutine (player.DisplayEmotiveBubble (1));
					while (player.displayingEmotion) {
						yield return new WaitForSeconds (0.1f);
						if (!player.displayingEmotion) {
							break;
						}
					}
					Player.disabled = true;
					slotmenu.SetActive (true);
                    Inputs.Disable("start");
					StartCoroutine (slots.Initialize ());

				} else {
					Dialogue.instance.Deactivate ();
					player.WaitToInteract ();
				}

			} else {
				yield return StartCoroutine(Dialogue.instance.text ("You don't have any"));
				yield return StartCoroutine(Dialogue.instance.line ("coins!"));
				yield return StartCoroutine(Dialogue.instance.done ());


			}
		} else {
			player.canInteractAgain = true;
		}
	}

	IEnumerator Text2(){
		Dialogue.instance.Deactivate ();
		Dialogue.instance.cantscroll = false;
		Dialogue.instance.finishedWithTextOverall = true;
        yield return StartCoroutine(Dialogue.instance.para (GameData.playerName + " turned on"));
		yield return StartCoroutine(Dialogue.instance.line ("the PC!"));
		yield return StartCoroutine(Dialogue.instance.done());
        Player.disabled = true;
		itemPCMenu.SetActive (true);
        Inputs.Disable("start");
        StartCoroutine(itemPCMenu.GetComponent<PC> ().Initialize ());
	}
	IEnumerator Text3(){
		Dialogue.instance.Deactivate ();
		Dialogue.instance.cantscroll = false;
		Dialogue.instance.finishedWithTextOverall = true;
		yield return StartCoroutine(Dialogue.instance.para ("Battle!"));
		yield return StartCoroutine(Dialogue.instance.done());
		player.StartBattle(0,0);


	}
	IEnumerator Text4(){
		Dialogue.instance.Deactivate ();
		Dialogue.instance.cantscroll = false;
		Dialogue.instance.finishedWithTextOverall = true;
        yield return StartCoroutine(Dialogue.instance.text("Hi there!"));
        yield return StartCoroutine(Dialogue.instance.line("May I help you?"));
		yield return StartCoroutine(Dialogue.instance.done());
		player.shopup = true;
        cursor.SetActive(true);
        Inputs.Disable("start");
		pokeMart.martlist = itemDatabase.IndigoItems;
		pokeMart.Init();
		shopmenu.SetActive (true);
        pokeMart.currentMenu = pokeMart.buysellwindow;


	}

IEnumerator Text5()
{
	Dialogue.instance.buycoinsmenu.SetActive(true);
	yield return StartCoroutine(Dialogue.instance.text("Welcome to ROCKET"));
	yield return StartCoroutine(Dialogue.instance.line("GAME CORNER!",1));
	yield return StartCoroutine(Dialogue.instance.para("Do you need some"));
	yield return StartCoroutine(Dialogue.instance.line("game coins?",1));
	yield return StartCoroutine(Dialogue.instance.para("It's $1000 for 50"));
	yield return StartCoroutine(Dialogue.instance.line("coins. Would you",0));
	yield return StartCoroutine(Dialogue.instance.cont("like some?"));
	yield return StartCoroutine(Dialogue.instance.prompt());
	if (Dialogue.instance.selectedOption == 0)
	{
		if (GameData.coins <= 9949 && GameData.coins >= 1000)
		{
			GameData.money -= 1000;
			GameData.coins += 50;
			yield return StartCoroutine(Dialogue.instance.text("Thanks! Here are"));
			yield return StartCoroutine(Dialogue.instance.line("your 50 coins!"));
			yield return StartCoroutine(Dialogue.instance.done());
		}
		else
		{
			if (GameData.money < 1000)
			{
				yield return StartCoroutine(Dialogue.instance.text("You can't afford"));
				yield return StartCoroutine(Dialogue.instance.line("the coins!"));
				yield return StartCoroutine(Dialogue.instance.done());
				Dialogue.instance.buycoinsmenu.SetActive(false);
				yield break;


			}
			if (GameData.coins > 9949)
			{
				yield return StartCoroutine(Dialogue.instance.text("Oops! Your COIN"));
				yield return StartCoroutine(Dialogue.instance.line("CASE is full."));
				yield return StartCoroutine(Dialogue.instance.done());
				Dialogue.instance.buycoinsmenu.SetActive(false);
				yield break;

			}



		}
	}
	else
	{
		yield return StartCoroutine(Dialogue.instance.text("No? Please come"));
		yield return StartCoroutine(Dialogue.instance.line("play sometime!"));
		yield return StartCoroutine(Dialogue.instance.done());


	}


	Dialogue.instance.buycoinsmenu.SetActive(false);
}

IEnumerator FoundCoinsText(int coinamount)
{
	GameData.coins += coinamount;
	yield return StartCoroutine(Dialogue.instance.text("Found " + coinamount + " coins!"));
	yield return StartCoroutine(Dialogue.instance.done());



}
IEnumerator Text7()
{
	yield return StartCoroutine(Dialogue.instance.text("I'm a rocket"));
	yield return StartCoroutine(Dialogue.instance.line("scientist!"));
	yield return StartCoroutine(Dialogue.instance.done());


}
    IEnumerator Text8()
    {
        yield return StartCoroutine(Dialogue.instance.text("LINQ's next"));
        yield return StartCoroutine(Dialogue.instance.line("stream:",1));
        yield return StartCoroutine(Dialogue.instance.para("(Insert time here)"));
        yield return StartCoroutine(Dialogue.instance.done());


    }

}
