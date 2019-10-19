using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public string item;

     public void GetItem(string item){
            StartCoroutine(GetItemText(item));
    }

    public IEnumerator GetItemText(string item){
        Items.instance.AddItem(item, 1);
        yield return Dialogue.instance.text(GameData.instance.playerName + " found \\l"+item.ToUpper() + "!");
        this.gameObject.SetActive(false);


    }
}
