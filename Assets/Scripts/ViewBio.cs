﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ViewBio : MonoBehaviour {
	public Image bioscreen;
	public int currentBioNumber;
	public Inputs INPUT;
    public bool displayingbio;

	public Sprite[] bios = new Sprite[453];
	// Use this for initialization
	void Start(){
		bios = Resources.LoadAll<Sprite> ("Bios");
		bioscreen.enabled = false;

	}

	public IEnumerator DisplayABio(int whatBio){
        PokedexEntry entry = SaveData.pokedexlist[whatBio - 1];
        Debug.Log("Display " + PokemonStats.IndexToMon(whatBio) +  "'s bio. \n This Pokemon " + (entry.seen && entry.caught ? "has been seen and caught." : entry.seen ? "has been seen." : "has not been seen or caught."));
		bioscreen.enabled = true;
			currentBioNumber = 0;
		for (int i = 0; i < 3; i++) {
            if(i > 0 && !SaveData.pokedexlist[whatBio - 1].caught){
                break;
            }
			displayingbio = true;
			bioscreen.sprite = bios [whatBio + (2 * (whatBio - 1)) + (currentBioNumber - 1)];
			currentBioNumber++;
			while (displayingbio) {
					yield return new WaitForSeconds (0.01f);
				if (Inputs.pressed("a")) {
					displayingbio = false;
						break;

				}
			}

		}

		bioscreen.enabled = false;
	}
}

