using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour, InteractableObject
{
    public ItemsEnum item;

    public IEnumerator Interact(){
        Inventory.instance.AddItem(item, 1);
        yield return Dialogue.instance.text(GameData.instance.playerName + " found &l" + PokemonData.GetItemName(item) + "!");
        this.gameObject.SetActive(false); //maybe replace with Destroy
    }
}
