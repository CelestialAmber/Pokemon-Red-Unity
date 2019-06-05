using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ViewBio : MonoBehaviour {
	public Image bioscreen;
	public int currentBioNumber;
    public bool displayingbio;


    public static ViewBio instance;
    private void Awake()
    {
        instance = this;
		bios = Resources.LoadAll<Sprite> ("Bios");
    }

	public Sprite[] bios = new Sprite[453];
	// Use this for initialization
	void Start(){
		

	}

	public IEnumerator DisplayABio(int whatBio){
		SoundManager.instance.SetMusicLow();
        PokedexEntry entry = GameData.instance.pokedexlist[whatBio - 1];
        Debug.Log("Display " + PokemonData.IndexToMon(whatBio) +  "'s bio. \n This Pokemon " + (entry.seen && entry.caught ? "has been seen and caught." : entry.seen ? "has been seen." : "has not been seen or caught."));
		bioscreen.enabled = true;
			currentBioNumber = 0;
		for (int i = 0; i < 3; i++) {
            if(i > 0 && !GameData.instance.pokedexlist[whatBio - 1].caught){
                break;
            }
			displayingbio = true;
			bioscreen.sprite = bios [whatBio + (2 * (whatBio - 1)) + (i - 1)];
			while (displayingbio) {
					yield return new WaitForSeconds (0.01f);
				if (Inputs.pressed("a")) {
					displayingbio = false;
						break;

				}
			}

		}

		bioscreen.enabled = false;
		SoundManager.instance.SetMusicNormal();
	}
}

