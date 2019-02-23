using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NPCTileDialogue : MonoBehaviour
{
    public DialogueMessage[] dialogueArray;
    public IEnumerator PlayDialogue(DialogueMessage[] dialogueArray) 
    {
        foreach (DialogueMessage dialogueMessage in dialogueArray)
        {

                if (dialogueMessage.type == DialogueMessage.Type.Continue)
                {
                    yield return Dialogue.instance.cont(dialogueMessage.message);
                }
                else
                {
                    yield return Dialogue.instance.text(dialogueMessage.message);
                }
            
        }
    }
}
