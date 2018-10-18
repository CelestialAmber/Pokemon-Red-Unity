using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FontAtlas
{
    public List<string> keys;
    public List<Sprite> chars;
    public object this[string key]
    {
        get
        {
            return FetchValue(key);
        }

    }
    object FetchValue(string key)
    {
        return chars[keys.IndexOf(key)];
    }
}
public class CustomTextAtlas : MonoBehaviour {
    public static CustomTextAtlas instance;
    private void Awake()
    {
        instance = this;
    }
    public FontAtlas atlas;
    public GameObject linefab, image;
}
