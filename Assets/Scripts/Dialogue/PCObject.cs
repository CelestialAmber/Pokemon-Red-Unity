using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCObject : MonoBehaviour
{
    public PC pc;
    
    public IEnumerator PlayDialogue(){
		Dialogue.instance.Deactivate();
        yield return Dialogue.instance.text (GameData.instance.playerName + " turned on&lthe PC!");
		Player.instance.menuActive = true;
        //PC.instance.gameObject.SetActive(true);
        //StartCoroutine(PC.instance.Initialize());
		pc.gameObject.SetActive(true);
        Inputs.Disable("start");
        StartCoroutine(pc.Initialize());
    }
}
