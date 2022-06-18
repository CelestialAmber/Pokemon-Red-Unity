using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemSlot : MonoBehaviour {
	public CustomText slotNameText, slotPriceText;
    public ItemsEnum item;
    //public ItemDataEntry itemData;
    public SlotMode mode;
    public int price;

	// Use this for initialization
	void Start () {

	}
    void Awake()
    {
        slotNameText = transform.GetChild(0).GetComponent<CustomText>();
        slotPriceText = transform.GetChild(1).GetComponent<CustomText>();
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
    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
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
        if (mode == SlotMode.Item)
            slotPriceText.text = "$" + price;
        else slotPriceText.text = "";

	}
}
