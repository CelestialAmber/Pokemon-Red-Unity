using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotMode{
    Item,
    Empty,
    Cancel
}

public enum SlotType{
    Item,
    Shop
}

public class ItemSlot : MonoBehaviour {
	public bool isKeyItem;
	public CustomText slotNameText, slotQuantityText;
	public ItemsEnum item;
    //public ItemDataEntry itemData;
	public int intquantity;
    public int price;
    public SlotMode mode;
    public SlotType type;

	// Use this for initialization
	void Awake () {
        slotNameText = transform.GetChild(0).GetComponent<CustomText>();
        slotQuantityText = transform.GetChild(1).GetComponent<CustomText>();
	}
	
	// Update is called once per frame
	void Update () {
        switch(mode){
            case SlotMode.Item:
                slotNameText.text = PokemonData.GetItemName(item);
                break;
            case SlotMode.Empty:
                slotNameText.text = "";
                break;
            case SlotMode.Cancel:
                slotNameText.text = "CANCEL";
                break;
        }
		
		if (!isKeyItem && mode == SlotMode.Item) {
            if(type == SlotType.Item){
			    slotQuantityText.text = "*" + (intquantity <= 9 ? " ": "") + intquantity.ToString();
            }else{
                slotQuantityText.text = "$" + price;
            }
		} else {

			slotQuantityText.text = "";
		}
	}

    public void UpdatePrice()
    {
        ItemDataEntry itemDataEntry = PokemonData.itemData[(int)item];

        if (itemDataEntry.price != 0)
        {
            price = itemDataEntry.price;
        }
        else throw new UnityException("A price entry doesn't exist for " + "\"" + name + "\"");
    }
}
