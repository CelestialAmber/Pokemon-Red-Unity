using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class NPCTileDialogue : MonoBehaviour
{
    public DialogueMessage[] dialogueArray;
    public IEnumerator PlayDialogue(DialogueMessage[] dialogueArray) 
    {
        for(int i = 0; i < dialogueArray.Length; i++){
            
            if(dialogueArray[i].type == DialogueMessage.Type.Text){
                if(dialogueArray[i].keepTextOnScreen)Dialogue.instance.keepTextOnScreen = true;
                yield return Dialogue.instance.text(dialogueArray[i].message);
        
            }
            else{
                yield return Dialogue.instance.prompt();
                i = dialogueArray[i].responseIndices[Dialogue.instance.selectedOption] - 1;
                continue; 
            }
            if(dialogueArray[i].endDialogue){
                break;
            }
        }
    }
}
