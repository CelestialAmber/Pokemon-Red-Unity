using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IntroHandler : MonoBehaviour {
	public int ChosenPokemon;
	public GameObject middle;
	public Sprite[] titleMons = new Sprite[17];
	// Use this for initialization
    void Awake()
    {
        Inputs.Disable("start");
        Player.disabled = true;
    }
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void ChangePokemonSprite(){
	 ChosenPokemon = Random.Range (1, 17);
		GetComponent<Image> ().sprite = titleMons [ChosenPokemon];

	}
	void ActivateTitle(){
		middle.SetActive (true);

	}
}

