using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsObject : MonoBehaviour
{
    public IEnumerator PlayDialogue()
    {
        if (Player.instance.direction == Direction.Left || Player.instance.direction == Direction.Right)
        {
            if (GameData.coins > 0)
            {
                yield return Dialogue.instance.text("A slot machine!\nWant to play?", true);
                yield return StartCoroutine(Dialogue.instance.prompt());
                if (Dialogue.instance.selectedOption == 0)
                {
                    Dialogue.instance.Deactivate();
                    Player.disabled = true;
                    StartCoroutine(Player.instance.DisplayEmotiveBubble(1));
                    while (Player.instance.displayingEmotion)
                    {
                        yield return new WaitForSeconds(0.1f);
                        if (!Player.instance.displayingEmotion)
                        {
                            break;
                        }
                    }
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
                yield return Dialogue.instance.text("You don't have any\ncoins!");


            }
        }
    }
}
