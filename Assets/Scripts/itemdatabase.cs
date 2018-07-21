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
public class itemdatabase : MonoBehaviour
{
    //both share the same index;
    //for items
    public List<Item> items = new List<Item>();
    //for PC
    public List<Item> pcitems = new List<Item>();
    public pc lc;
    public List<string> ViridianItems = new List<string>(
        new string[]{
        "POKE BALL",
        "POTION",
        "ANTIDOTE",
        "PARLYZ HEAL",
        "BURN HEAL"
    });
    public List<string> PewterItems = new List<string>(
        new string[]{
        "POKE BALL",
        "POTION",
        "ESCAPE ROPE",
        "ANTIDOTE",
        "BURN HEAL",
        "AWAKENING",
        "PARLYZ HEAL"
    });
    public List<string> CeruleanItems = new List<string>(
        new string[]{
        "POKE BALL",
        "POTION",
        "ESCAPE ROPE",
        "REPEL",
        "ANTIDOTE",
        "BURN HEAL",
        "AWAKENING",
        "PARLYZ HEAL"
    });
    public List<string> VermilionItems = new List<string>(
        new string[]{
       "POKE BALL",
       "SUPER POTION",
        "ICE HEAL",
        "AWAKENING",
       "PARLYZ HEAL",
       "REPEL"
    });
    public List<string> LavenderItems = new List<string>(
        new string[]{

      "GREAT BALL",
      "SUPER POTION",
      "REVIVE",
      "ESCAPE ROPE",
      "SUPER REPEL",
      "ANTIDOTE",
      "BURN HEAL",
      "ICE HEAL",
      "PARLYZ HEAL"
    });
    public List<string> SaffronItems = new List<string>(
        new string[]{
        "GREAT BALL",
        "HYPER  POTION",
        "MAX REPEL",
        "ESCAPE ROPE",
        "FULL HEAL",
        "REVIVE"
    });
    public List<string> FuchsiaItems = new List<string>(
        new string[]{
        "ULTRA BALL",
        "GREAT BALL",
        "SUPER POTION",
        "HYPER POTION",
        "REVIVE",
        "FULL HEAL",
        "SUPER REPEL"
    });
    public List<string> CinnabarItems = new List<string>(
        new string[]{
        "ULTRA BALL",
        "GREAT BALL",
        "HYPER POTION",
        "MAX REPEL",
        "ESCAPE ROPE",
        "FULL HEAL",
        "REVIVE"
    });
    public List<string> IndigoItems = new List<string>(
        new string[]{
        "ULTRA BALL",
        "GREAT BALL",
        "FULL RESTORE",
        "MAX POTION",
        "FULL HEAL",
        "REVIVE",
        "MAX REPEL"
    });

    public string[] keyitems = {
        "BIKE VOUCHER",
        "BICYCLE",
        "HELIX FOSSIL",
        "DOME FOSSIL",
        "CARD KEY",
        "COIN CASE",
        "EXP. ALL",
        "GOLD TEETH",
        "GOOD ROD",
        "ITEMFINDER",
        "LIFT KEY",
        "OAK'S PARCEL",
        "OLD AMBER",
        "OLD ROD",
        "POKEFLUTE",
        "POKEDEX",
        "S.S. TICKET",
        "SECRET KEY",
        "SILPH SCOPE",
        "SUPER ROD",
        "TOWN MAP" 
    };
	// Use this for initialization
	void Start () {
        check1();
        check2();
	}
	
	// Update is called once per frame
	void Update () {


			



		}
	public void check1(){
		for (int i = 0; i < items.Count; i++) {
                if (System.Array.IndexOf(keyitems, items[i].name) > -1) {
                items[i].isKeyItem = true;
				}
			

		}
	}


	public void check2(){
		for (int i = 0; i < pcitems.Count; i++) {
                if (System.Array.IndexOf(keyitems, pcitems[i].name) > -1)
                {
                        pcitems[i].isKeyItem = true;
                    
                }
		}
	}

	}

