using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class FontChar {
        public Vector2 texPos;
        public Vector2 size;
    }
    [System.Serializable]
    public class BracketChar {
        public string name;
        public int index;
    }
public class FontAtlas : ScriptableObject
{
   
    public List<string> keys;
    
      public List<FontChar> fontChars;

      public List<FontChar> blueSlotsChars;

      public List<BracketChar> bracketChars;
    public Sprite this[string key]
    {
        get
        {
            return FetchValue(key);
        }

    }

    public FontChar FetchFontChar(string key)
    {
        return fontChars[keys.IndexOf(key)];
    }

    Sprite FetchValue(string key)
    {
        return null;
        //return chars[keys.IndexOf(key)];
    }
}
