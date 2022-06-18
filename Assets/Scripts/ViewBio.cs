using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ViewBio : MonoBehaviour {
	public GameObject menu;
	public int pokemonID;
    public bool displayingBio;
	public Image pokemonSprite;
	public CustomText descriptionText, nameText, categoryText, heightText, weightText, dexNoText;
	string pokemonName = "";
	PokemonDataEntry entryData;


	public IEnumerator DisplayABio(int whatBio){
		SoundManager.instance.SetMusicLow();
        PokedexEntry entry = GameData.instance.pokedexlist[whatBio - 1];
		pokemonName = PokemonData.IndexToMon(whatBio);

        Debug.Log("Display " + pokemonName +  "'s bio. This Pokemon " + (entry.seen && entry.caught ? "has been seen and caught." : entry.seen ? "has been seen." : "has not been seen or caught."));
		
		pokemonID = whatBio;
		entryData = PokemonData.pokemonData[pokemonID - 1];

		InitText();
		menu.SetActive(true);
		displayingBio = true;

		SoundManager.instance.PlayCry(pokemonID - 1);

		while(SoundManager.instance.isPlayingCry){
			yield return new WaitForSeconds(0.01f);
		}

		while (true) {
			yield return new WaitForSeconds(0.01f);
			if(Inputs.pressed("a")) break;
		}

		//If there's more than one page for the description, go to the next page
		if(entryData.descriptionText.Length > 1){
			descriptionText.text = entryData.descriptionText[1];

			while (true) {
				yield return new WaitForSeconds (0.01f);
				if(Inputs.pressed("a")) break;
			}
		}

		displayingBio = false;
		menu.SetActive(false);
		SoundManager.instance.SetMusicNormal();
	}

	public void InitText(){
		nameText.text = pokemonName;
		categoryText.text = entryData.category;
		heightText.text = entryData.heightFeet + " " + (entryData.heightInches < 10 ? "0" : "") + entryData.heightInches;
		weightText.text = string.Format("{0,5:0.0}",entryData.weight);
		dexNoText.text = (pokemonID > 99 ? "" : pokemonID > 9 ? "0" : "00") + pokemonID.ToString();
		descriptionText.text = entryData.descriptionText[0];
		pokemonSprite.sprite = GameData.instance.frontMonSprites[pokemonID - 1];
	}


}

