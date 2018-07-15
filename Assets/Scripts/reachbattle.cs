using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reachbattle : MonoBehaviour {
	public BattleManager bm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void ShowBall(){
		StartCoroutine(bm.PokeballShow());
	}
	void DetermineFront(){
	bm.DetermineFrontSprite();
	}
	void DetermineBack(){
		bm.DetermineBackSprite();
	}
	void ReadyToBattle(){

		bm.readytobattle = true;
		bm.selectedOption = 0;
	}
	void ActivateStatsOur(){
		bm.ActivateOurStats ();
	}
	void ActivateStatsTheir(){
		bm.ActivateTheirStats ();
	}
}
