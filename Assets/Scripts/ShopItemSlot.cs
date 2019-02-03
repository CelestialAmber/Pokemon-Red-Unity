using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemSlot : MonoBehaviour {
	public CustomText slotNameText, slotPriceText;
	public new string name;
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
    public void UpdatePrice()
    {
        if (PokemonData.itemPrices.ContainsKey(name))
        {
            intPrice = PokemonData.itemPrices[name];
        }
        else throw new UnityException("A price entry doesn't exist for " + "\"" + name + "\"");
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
        if (mode == SlotMode.Item)
            slotPriceText.text = "$" + intPrice;
        else slotPriceText.text = "";

	}
}
