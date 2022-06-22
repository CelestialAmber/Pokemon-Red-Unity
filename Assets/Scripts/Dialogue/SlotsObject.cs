using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsObject : MonoBehaviour, InteractableObject
{
    public IEnumerator Interact()
    {
        if (Player.instance.direction == Direction.Left || Player.instance.direction == Direction.Right)
        {
            if (GameData.instance.coins > 0)
            {
                Dialogue.instance.keepTextOnScreen = true;
                yield return Dialogue.instance.text("A slot machine!&lWant to play?");
                yield return StartCoroutine(Dialogue.instance.prompt());
                if (Dialogue.instance.selectedOption == 0)
                {
                    Dialogue.instance.Deactivate();
                   yield return StartCoroutine(Player.instance.DisplayEmotiveBubble(1));
                    Player.instance.isDisabled = true;
                    GameDataManager.instance.slots.gameObject.SetActive(true);
                    InputManager.Disable(Button.Start);
                    StartCoroutine(Slots.instance.Initialize());
                }
                else
                {
                    Dialogue.instance.Deactivate();
                }
            }
            else
            {
                yield return Dialogue.instance.text("You don't have any&lcoins!");
            }
        }
    }
}
