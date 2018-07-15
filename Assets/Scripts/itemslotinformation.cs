using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class itemslotinformation : MonoBehaviour {
	public bool isKeyItem;
	public CustomText slotNameText, slotQuantityText;
	public string Name;
	public int intquantity;
	// Use this for initialization
	void Awake () {
        slotNameText = transform.GetChild(1).GetComponent<CustomText>();
        slotQuantityText = transform.GetChild(2).GetComponent<CustomText>();
	}
	
	// Update is called once per frame
	void Update () {
		slotNameText.text = Name;
		if (!isKeyItem) {
			slotQuantityText.text = "*" +(intquantity <= 9 ? " ": "") + intquantity.ToString ();
		} else {

			slotQuantityText.text = "";
		}
	}
}
