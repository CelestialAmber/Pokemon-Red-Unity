using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMessage {
    public enum Type
    {
        Text,
        Prompt
    }

    public int[] responseIndices = new int[2];
    public Type type;

    public bool endDialogue;
    public bool keepTextOnScreen;
    
    [TextArea(5,10)]
    public string message;
    
}
