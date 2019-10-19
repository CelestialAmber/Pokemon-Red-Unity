using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsObject : MonoBehaviour
{
    public IEnumerator PlayDialogue()
    {
        if (Player.instance.direction == Direction.Left || Player.instance.direction == Direction.Right)
        {
            if (GameData.instance.coins > 0)
            {
                Dialogue.instance.keepTextOnScreen = true;
                yield return Dialogue.instance.text("A slot machine!\\lWant to play?");
                yield return StartCoroutine(Dialogue.instance.prompt());
                if (Dialogue.instance.selectedOption == 0)
                {
                    Dialogue.instance.Deactivate();
                   yield return StartCoroutine(Player.instance.DisplayEmotiveBubble(1));
                    Player.disabled = true;
                    GameDataManager.instance.slots.gameObject.SetActive(true);
                    Inputs.Disable("start");
                    StartCoroutine(Slots.instance.Initialize());

                }
                else
                {
                    Dialogue.instance.Deactivate();
                }

            }
            else
            {
                yield return Dialogue.instance.text("You don't have any\\lcoins!");


            }
        }
    }
}
