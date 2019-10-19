using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomText : MaskableGraphic
{

    public class TextLine{
        public int width;
        public int height;
        public List<FontChar> characters = new List<FontChar>();
    }
    public enum AlignmentType {
        Left,
        Right,
        Center
    
    }
   
    TextLine currentline = new TextLine();
    private List<TextLine> lines = new List<TextLine>();
    public string text
    {
        get
        {
            return field;
        }
        set
        {
            if(field == value) return;
            field = value;
            SetVerticesDirty();
            SetLayoutDirty();
        }
    }
     [SerializeField]
    [TextArea(3,10)]
    private string field;
    public AlignmentType alignmentType {
        get{
          return m_AlignmentType;

        }
        set{
              if(m_AlignmentType == value) return;
              m_AlignmentType = value;
              SetVerticesDirty();
              SetLayoutDirty();
        }
    }

    [SerializeField]
    private AlignmentType m_AlignmentType = AlignmentType.Left;

    public FontAtlas fontAtlas;

    [SerializeField]
    Texture m_Texture;
    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value)
                return;
 
            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }
    public override Texture mainTexture
    {
        get
        {
            return m_Texture == null ? s_WhiteTexture : m_Texture;
        }
    }
    void UpdateText()
    {
        lines.Clear();
        currentline.characters.Clear();
        for (int i = 0; i < text.Length; i++){
            if(text[i] == '\''){ //is the current character an apostrophe?
                if (i < text.Length - 1) 
                {
                    bool shiftPosition = true;
                    switch (text.ToCharArray()[i + 1])
                    {
                        case 'd':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'d"));
                            break;
                        case 'l':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'l"));
                            break;
                        case 's':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'s"));
                            break;
                        case 't':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'t"));
                            break;
                        case 'v':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'v"));
                            break;
                        case 'r':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'r"));
                            break;
                        case 'm':
                            currentline.characters.Add(fontAtlas.FetchFontChar("'m"));
                            break;
                        default:
                            currentline.characters.Add(fontAtlas.FetchFontChar("\'"));
                            shiftPosition = false;
                            break;
                    }

                    if(shiftPosition) i++;
                }else{
                    currentline.characters.Add(fontAtlas.FetchFontChar("\'"));
                }
             
            }
            else if(text[i] == '<'){ //is the current character a left bracket?
                foreach(BracketChar bracketChar in fontAtlas.bracketChars){
                    if(text.Substring(i).IndexOf("<" + bracketChar.name + ">") == 0){ //did we find a bracket expression?
                        currentline.characters.Add(fontAtlas.fontChars[bracketChar.index]);
                        i += bracketChar.name.Length + 1;
                        break;
                    }
                }
            }
            else
            {
               
                if (text[i] == '\n') //is the current character a line break?
                {
                lines.Add(currentline);
                currentline.height = 8; //set the height of the line to at least be 8
                currentline = new TextLine();   
                }else if(!fontAtlas.keys.Contains(text[i].ToString()))
                 continue; //if the given character isn't in the atlas, skip it
                else{
                currentline.characters.Add(fontAtlas.FetchFontChar(text.Substring(i,1)));
                }
               
            }
            if(i == text.Length - 1 && text[i] != '\n'){ //if we're at the end of the text, and the last character isn't a line break, add the current line
            lines.Add(currentline);
             }  
        }
       
        
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if(fontAtlas == null || texture == null || text == null) return;
        UpdateText();
         /*Find the dimensions of the texture by adding the 
        maximum width of the rows and the sum of the maximum
        height of each row plus a blank line for each line
        */
        int texWidth = 0;
        int texHeight = 0;
        for(int i = 0; i < lines.Count; i++){ //get the height of the texture
           if(i < lines.Count - 1) texHeight += 8; //if the line isn't the last line (line break), add 8 pixels of space
            int maxCharHeight = lines[i].height; //set the max height directly in case a line only has a line break
            foreach(FontChar fontChar in lines[i].characters){
                if(fontChar.size.y > maxCharHeight) maxCharHeight = (int)fontChar.size.y;
            }
            lines[i].height = maxCharHeight;
            texHeight += maxCharHeight;
        }
         int maxLineWidth = 0; //get the width of the texture
        for(int i = 0; i < lines.Count; i++){
           int lineWidth = 0;
            foreach(FontChar fontChar in lines[i].characters){
                lineWidth += (int)fontChar.size.x; //get the line's total width
            }
            lines[i].width = lineWidth;
            if(lineWidth > maxLineWidth) maxLineWidth = lineWidth; //update the highest width

        }
        texWidth = maxLineWidth;
        float xStart = 0;
        float yStart = 0;
        float xOffset = 0;
        float yOffset = 0;
        for(int i = 0; i < lines.Count; i++){
            xStart = alignmentType == AlignmentType.Center ? -((float)lines[i].width/2f) : 0;
            xOffset = 0;
            
            for(int j = 0; j < lines[i].characters.Count; j++){
                FontChar currentChar = lines[i].characters[alignmentType == AlignmentType.Right ? lines[i].characters.Count - j - 1 : j];
                Vector2 uv = currentChar.texPos;
                Vector2 size = currentChar.size;
                Vector2 texSize = new Vector2(texture.width,texture.height);
                
                float x = xStart + (alignmentType == AlignmentType.Right ? -xOffset - (int)size.x : xOffset);
                float y = yStart - yOffset - 8;
                int offset = vh.currentVertCount;
                vh.AddVert(new Vector3(x, y),this.color,new Vector2(uv.x / texSize.x, uv.y / texSize.y));
                 vh.AddVert(new Vector3(x + size.x, y),this.color,new Vector2((uv.x+size.x) / texSize.x,  uv.y / texSize.y));
                 vh.AddVert(new Vector3(x + size.x, y + size.y),this.color, new Vector2((uv.x+size.x) / texSize.x, (uv.y+size.y) / texSize.y));
                 vh.AddVert( new Vector3(x, y + size.y),this.color,new Vector2(uv.x / texSize.x, (uv.y+size.y) / texSize.y));
                 vh.AddTriangle(0 + offset, 2 + offset, 1 + offset);
                vh.AddTriangle( 0 + offset, 3 + offset, 2 + offset );

                xOffset += (size.x);
            }
            yOffset += 8;
        }
        lines.Clear();

    }
}
