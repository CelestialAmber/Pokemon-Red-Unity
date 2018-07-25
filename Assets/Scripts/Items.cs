using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item{
    public string name;
    public int quantity;
    public bool isKeyItem;
    public Item(string name, int quantity){
        this.name = name;
        this.quantity = quantity;
    }
}
public class Items : MonoBehaviour
{
    //both share the same index;
    //for items
    public List<Item> items = new List<Item>();
    //for PC
    public List<Item> pcItems = new List<Item>();
    public PC lc;
    public List<string> ViridianItems = new List<string>(
        new string[]{
        "Poke Ball",
        "Potion",
        "Antidote",
        "Parlyz Heal",
        "Burn Heal"
    });
    public List<string> PewterItems = new List<string>(
        new string[]{
        "Poke Ball",
        "Potion",
        "Escape Rope",
        "Antidote",
        "Burn Heal",
        "Awakening",
        "Parlyz Heal"
    });
    public List<string> CeruleanItems = new List<string>(
        new string[]{
        "Poke Ball",
        "Potion",
        "Escape Rope",
        "Repel",
        "Antidote",
        "Burn Heal",
        "Awakening",
        "Parlyz Heal"
    });
    public List<string> VermilionItems = new List<string>(
        new string[]{
       "Poke Ball",
       "Super Potion",
        "Ice Heal",
        "Awakening",
       "Parlyz Heal",
       "Repel"
    });
    public List<string> LavenderItems = new List<string>(
        new string[]{

      "Great Ball",
      "Super Potion",
      "Revive",
      "Escape Rope",
      "Super Repel",
      "Antidote",
      "Burn Heal",
      "Ice Heal",
      "Parlyz Heal"
    });
    public List<string> SaffronItems = new List<string>(
        new string[]{
        "Great Ball",
        "Hyper Potion",
        "Max Repel",
        "Escape Rope",
        "Full Heal",
        "Revive"
    });
    public List<string> FuchsiaItems = new List<string>(
        new string[]{
        "Ultra Ball",
        "Great Ball",
        "Super Potion",
        "Hyper Potion",
        "Revive",
        "Full Heal",
        "Super Repel"
    });
    public List<string> CinnabarItems = new List<string>(
        new string[]{
        "Ultra Ball",
        "Great Ball",
        "Hyper Potion",
        "Max Repel",
        "Escape Rope",
        "Full Heal",
        "Revive"
    });
    public List<string> IndigoItems = new List<string>(
        new string[]{
        "Ultra Ball",
        "Great Ball",
        "Full Restore",
        "Max Potion",
        "Full Heal",
        "Revive",
        "Max Repel"
    });

    public string[] keyitems = {
        "Bike Voucher",
        "Bicycle",
        "Helix Fossil",
        "Dome Fossil",
        "Card Key",
        "Coin Case",
        "Exp. All",
        "Gold Teeth",
        "Good Rod",
        "Itemfinder",
        "Lift Key",
        "Oak's Parcel",
        "Old Amber",
        "Old Rod",
        "Pokeflute",
        "Pokedex",
        "S.S. Ticket",
        "Secret Key",
        "Silph Scope",
        "Super Rod",
        "Town Map" 
    };
	// Use this for initialization
	void Start () {
        checkKeyItemsBag();
        checkKeyItemsPC();
	}
	
	// Update is called once per frame
	void Update () {


			



		}
    public void checkKeyItemsBag(){
		for (int i = 0; i < items.Count; i++) {
                if (System.Array.IndexOf(keyitems, items[i].name) > -1) {
                items[i].isKeyItem = true;
				}
			

		}
	}


    public void checkKeyItemsPC(){
		for (int i = 0; i < pcItems.Count; i++) {
                if (System.Array.IndexOf(keyitems, pcItems[i].name) > -1)
                {
                        pcItems[i].isKeyItem = true;
                    
                }
		}
	}
    //Checks whether the requested item exists in the bag.
    public bool hasItem(string name){
        foreach(Item item in items){
            if (item.name == name) return true;
        }
        return false;
    }
    //Removes the first instance of an item from the bag if it exists.
    public void RemoveItem(string name){
        foreach(Item item in items){
            if (item.name == name)
            {
                items.Remove(item);
                return;
            }
        }


    }
    //Adds an item to the bag.
    public void AddItem(string name, int quantity)
    {
        bool alreadyInBag = false;

        Item inBagItem = new Item("", 0);
        foreach (Item item in items)
        {
            if (item.name == name)
            {
                inBagItem = item;
                alreadyInBag = true;
                break;
            }

        }
        //If the item is already in the bag, just increase the stack.
        if (alreadyInBag) items[items.IndexOf(inBagItem)].quantity += quantity;
        else if (items.Count < 20) items.Add(new Item(name, quantity));

        checkKeyItemsBag();



    }
    public void RemoveItem(int amount, int index)
    {

        items[index].quantity -= amount;
        if (items[index].quantity <= 0) items.RemoveAt(index);
    }

	}

