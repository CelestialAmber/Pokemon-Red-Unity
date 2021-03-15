using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class CreateCustomTextMenu
{
    [MenuItem("GameObject/UI/Custom Text",false)]
    static void CreateCustomText(MenuCommand menuCommand){
        GameObject go = new GameObject("Text");
        CustomText CustomText = go.AddComponent<CustomText>();
        CustomText.texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Fonts/font.png");
        CustomText.fontAtlas = AssetDatabase.LoadAssetAtPath<FontAtlas>("Assets/Fonts/FontAtlas.asset");
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go,"Create " + go.name);
        Selection.activeGameObject = go;
    }
    
}
