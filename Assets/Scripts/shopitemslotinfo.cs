using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopitemslotinfo : MonoBehaviour {
	public CustomText slotNameText, slotPriceText;
	public string name;
	public int intPrice;
    public SlotMode mode;
	// Use this for initialization
	void Start () {

	}
    void Awake()
    {
        slotNameText = transform.GetChild(0).GetComponent<CustomText>();
        slotPriceText = transform.GetChild(1).GetComponent<CustomText>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case SlotMode.Item:
                slotNameText.text = name.ToUpper();
                break;
            case SlotMode.Empty:
                slotNameText.text = "";
                break;
            case SlotMode.Cancel:
                slotNameText.text = "CANCEL";
                break;
        }
        switch (name)
        {

            case "Poke Ball":
            intPrice = 200;
            break;

             case "Great Ball":
            intPrice = 600;
             break;

        case "Ultra Ball":
        intPrice = 1200;
        break;

       case "Repel":
        intPrice = 350;
         break;

       case "Super Repel":
            intPrice = 500;
        break;

       case "Max Repel":
        
            intPrice = 700;

        break;
       case "Potion":
        
            intPrice = 300;

        break;
       case "Super Potion":
        
            intPrice = 700;

        break;
       case "Hyper Potion":
        
            intPrice = 1500;

        break;
       case "Max Potion":
        
            intPrice = 2500;

        break;
       case "Full Restore":
        
            intPrice = 3000;

        break;
       case "Antidote":
        
            intPrice = 100;

        break;
       case "Awakening":
        
            intPrice = 200;

        break;
       case "Ice Heal":
        
            intPrice = 250;

        break;
       case "Burn Heal":
        
            intPrice = 250;

        break;
       case "Parlyz Heal":
        
            intPrice = 200;

        break;
       case "Full Heal":
        
            intPrice = 600;

        break;
       case "Escape Rope":
        
            intPrice = 550;

        break;
       case "REVIVE":
        
            intPrice = 1500;
                break;
        
    }
        if (mode == SlotMode.Item)
            slotPriceText.text = "$" + intPrice.ToString();
        else slotPriceText.text = "";

	}
}
