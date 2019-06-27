using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FontAtlasEdit))]
public class FontAtlasEditor: Editor {
    FontAtlasEdit fontatlasEdit;
public override void OnInspectorGUI(){
    DrawDefaultInspector();
    fontatlasEdit = (FontAtlasEdit)target;
    if(GUILayout.Button("UV fix")){
        FontAtlas fontAtlas = fontatlasEdit.fontAtlas;
        for(int i = 0; i < fontAtlas.fontChars.Count; i++){
            fontAtlas.fontChars[i].texPos.y -= 32;
        }
         for(int i = 0; i < fontAtlas.blueSlotsChars.Count; i++){
            fontAtlas.blueSlotsChars[i].texPos.y -= 32;
        }
    }

}
}
