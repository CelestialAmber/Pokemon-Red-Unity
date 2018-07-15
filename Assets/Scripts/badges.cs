using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class badges : MonoBehaviour {
	public Image[] allbadges = new Image[8];
	public Sprite[] notobtainedimages = new Sprite[8];
	public Sprite[] obtainedimages = new Sprite[8];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 8; i++) {
			if (SaveData.hasBadge [i]) {
				allbadges [i].sprite = obtainedimages [i];

			} else {

				allbadges [i].sprite = notobtainedimages [i];
			}


		}
	}
}
