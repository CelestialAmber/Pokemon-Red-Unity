using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCObject : MonoBehaviour, InteractableObject
{
    public IEnumerator Interact(){
		Dialogue.instance.Deactivate();
        yield return Dialogue.instance.text (GameData.instance.playerName + " turned on&lthe PC!");
		Player.instance.menuActive = true;
        PC pc = GameDataManager.instance.pc;
        pc.gameObject.SetActive(true);
        StartCoroutine(pc.Initialize());
        InputManager.Disable(Button.Start);
    }
}
