using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item{
    public string name;
    public int quantity;
    public bool isKeyItem;
    public Item(string name, int quantity, bool isKeyItem){
        this.name = name;
        this.quantity = quantity;
        this.isKeyItem = isKeyItem;
    }
}
public class Items : MonoBehaviour
{
    //both share the same index;
    //for items
    public List<Item> items = new List<Item>();
    //for PC
    public List<Item> pcItems = new List<Item>();



    public List<string> keyitems = new List<string>(new string[]{
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
    });
	// Use this for initialization

    public static Items instance;
    private void Awake()
    {
        instance = this;
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


			



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

        Item itemToAdd = new Item(name, quantity,keyitems.Contains(name));
        foreach (Item item in items)
        {
            if (item.name == name)
            {
                itemToAdd = item;
                alreadyInBag = true;
                break;
            }

        }
        //If the item is already in the inventory, and the item isn't a key item, just increase the stack.
        if (alreadyInBag){ 
           items[items.IndexOf(itemToAdd)].quantity += quantity;
           int newQuantity = items[items.IndexOf(itemToAdd)].quantity;
           int newStackAmount;
           if(newQuantity > 99){ //if the stack is bigger than 99, then try to create a new stack
            newStackAmount = newQuantity - 99;
            if(items.Count < 20)items.Add(new Item(itemToAdd.name,newStackAmount,itemToAdd.isKeyItem));
            //if the given inventory is full, then revert the stack's quantity
            else items[items.IndexOf(itemToAdd)].quantity -= quantity;
           }
        }
        else if (items.Count < 20) items.Add(itemToAdd);
    }

       //Adds an item to the PC.
    public void AddItemPC(string name, int quantity)
    {
        bool alreadyInPC = false;

        Item itemToAdd = new Item(name, quantity,keyitems.Contains(name));
        foreach (Item item in pcItems)
        {
            if (item.name == name)
            {
                itemToAdd = item;
                alreadyInPC = true;
                break;
            }

        }
        //If the item is already in the PC, and the item isn't a key item, just increase the stack.
        if (alreadyInPC){ 
           pcItems[pcItems.IndexOf(itemToAdd)].quantity += quantity;
           int newQuantity = pcItems[pcItems.IndexOf(itemToAdd)].quantity;
           int newStackAmount;
           if(newQuantity > 99){ //if the stack is bigger than 99, then try to create a new stack
            newStackAmount = newQuantity - 99;
            if(pcItems.Count < 50)pcItems.Add(new Item(itemToAdd.name,newStackAmount,itemToAdd.isKeyItem));
            //if the given inventory is full, then revert the stack's quantity
            else pcItems[pcItems.IndexOf(itemToAdd)].quantity -= quantity;
           }
        }
        else if (pcItems.Count < 50) pcItems.Add(itemToAdd);
    }
  
    public void RemoveItem(int amount, int index)
    {
        items[index].quantity -= amount;
        if (items[index].quantity <= 0) items.RemoveAt(index);
        }
    public void RemoveItemPC(int amount, int index)
    {   
        pcItems[index].quantity -= amount;
        if (pcItems[index].quantity <= 0) pcItems.RemoveAt(index);
        }

	}

