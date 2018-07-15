using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopitemslotinfo : MonoBehaviour {
	public CustomText slotNameText, slotPriceText;
	public string Name;
	public int intPrice;
	// Use this for initialization
	void Start () {

	}
    void Awake()
    {
        slotNameText = transform.GetChild(1).GetComponent<CustomText>();
        slotPriceText = transform.GetChild(2).GetComponent<CustomText>();
    }
	// Update is called once per frame
	void Update () {

		slotNameText.text = Name;
		if (Name == "POKE BALL") {
			intPrice = 200;

		}
		if (Name == "GREAT BALL") {
			intPrice = 600;

		}
		if (Name == "ULTRA BALL") {
			intPrice = 1200;

		}
		if (Name == "REPEL") {
			intPrice = 350;

		}
		if (Name == "SUPER REPEL") {
			intPrice = 500;

		}
		if (Name == "MAX REPEL") {
			intPrice = 700;

		}
		if (Name == "POTION") {
			intPrice = 300;

		}
		if (Name == "SUPER POTION") {
			intPrice = 700;

		}
		if (Name == "HYPER POTION") {
			intPrice = 1500;

		}
		if (Name == "MAX POTION") {
			intPrice = 2500;

		}
		if (Name == "FULL RESTORE") {
			intPrice = 3000;

		}
		if (Name == "ANTIDOTE") {
			intPrice = 100;

		}
		if (Name == "AWAKENING") {
			intPrice = 200;

		}
		if (Name == "ICE HEAL") {
			intPrice = 250;

		}
		if (Name == "BURN HEAL") {
			intPrice = 250;

		}
		if (Name == "PARLYZ HEAL") {
			intPrice = 200;

		}
		if (Name == "FULL HEAL") {
			intPrice = 600;

		}
		if (Name == "ESCAPE ROPE") {
			intPrice = 550;

		}
		if (Name == "REVIVE") {
			intPrice = 1500;

		}
		slotPriceText.text = "$" + intPrice.ToString();

	}
}
