using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item{
    public ItemsEnum item;
    public int quantity;
    public bool isKeyItem;
    
    public Item(ItemsEnum item, int quantity, bool isKeyItem){
        this.item = item;
        this.quantity = quantity;
        this.isKeyItem = isKeyItem;
    }
}

public class Inventory : Singleton<Inventory>
{
    //both share the same index;
    //for items
    public List<Item> items = new List<Item>();
    //for PC
    public List<Item> pcItems = new List<Item>();

    public List<ItemsEnum> keyItems = new List<ItemsEnum>(new ItemsEnum[]{
        ItemsEnum.BikeVoucher,
        ItemsEnum.Bicycle,
        ItemsEnum.HelixFossil,
        ItemsEnum.DomeFossil,
        ItemsEnum.CardKey,
        ItemsEnum.CoinCase,
        ItemsEnum.ExpAll,
        ItemsEnum.GoldTeeth,
        ItemsEnum.GoodRod,
        ItemsEnum.Itemfinder,
        ItemsEnum.LiftKey,
        ItemsEnum.OaksParcel,
        ItemsEnum.OldAmber,
        ItemsEnum.OldRod,
        ItemsEnum.PokeFlute,
        ItemsEnum.Pokedex,
        ItemsEnum.SSTicket,
        ItemsEnum.SecretKey,
        ItemsEnum.SilphScope,
        ItemsEnum.SuperRod,
        ItemsEnum.TownMap
    });

    // Use this for initialization
	void Start(){
	}
	
	// Update is called once per frame
	void Update(){
	}

    //Checks whether the requested item exists in the bag.
    public bool hasItem(ItemsEnum itemId){
        foreach(Item item in items){
            if (item.item == itemId) return true;
        }
        return false;
    }

    //Checks whether the given item is a key item.
    public bool IsKeyItem(ItemsEnum item){
        return keyItems.Contains(item);
    }

    //Removes the first instance of an item from the bag if it exists.
    public void RemoveItem(ItemsEnum itemId){
        foreach(Item item in items){
            if (item.item == itemId)
            {
                items.Remove(item);
                return;
            }
        }
    }

    //Adds an item to the bag.
    public void AddItem(ItemsEnum itemId, int quantity)
    {
        bool alreadyInBag = false;

        Item itemToAdd = new Item(itemId, quantity, keyItems.Contains(itemId));
        foreach (Item item in items)
        {
            if (item.item == itemId)
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
            if(items.Count < 20)items.Add(new Item(itemToAdd.item,newStackAmount,itemToAdd.isKeyItem));
            //if the given inventory is full, then revert the stack's quantity
            else items[items.IndexOf(itemToAdd)].quantity -= quantity;
           }
        }
        else if (items.Count < 20) items.Add(itemToAdd);
    }

       //Adds an item to the PC.
    public void AddItemPC(ItemsEnum itemId, int quantity)
    {
        bool alreadyInPC = false;

        Item itemToAdd = new Item(itemId, quantity, IsKeyItem(itemId));
        foreach (Item item in pcItems)
        {
            if (item.item == itemId)
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
                if(pcItems.Count < 50) pcItems.Add(new Item(itemToAdd.item,newStackAmount,itemToAdd.isKeyItem));
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

