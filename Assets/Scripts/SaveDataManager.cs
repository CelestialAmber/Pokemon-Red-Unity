using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour {
    
    void Awake(){
        SaveData.Init();
        SaveData.money = 3000;
        SaveData.coins = 300;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
