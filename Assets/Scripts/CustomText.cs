using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class CustomText : MonoBehaviour
{
    [TextArea]
    public string field;
    GameObject currentline = null;
    List<GameObject> lines = new List<GameObject>();
    float lastWidth;
    CustomTextAtlas ct;
    public string text
    {
        get
        {
            return field;
        }
        set
        {
            field = value;
        }
    }
    string lasttext;
    public bool alignedLeft = true;
    // Use this for initialization
    private void Awake()
    {
        ct = GameObject.Find("FontAtlas").GetComponent<CustomTextAtlas>();
    }
    void Start()
    {
        foreach (GameObject line in lines)
        {
            RectTransform rt = line.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 8);
        }
        UpdateText();
    }

    void OnEnable()
    {
        Update();
    }
    // Update is called once per frame
    void Update()
    {
        if (ct == null) ct = CustomTextAtlas.instance;
        if (text != lasttext)
        {
            UpdateText();
        }
        if (GetComponent<RectTransform>().sizeDelta.x != lastWidth)
        {
            foreach (GameObject line in lines)
            {
                RectTransform rt = line.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 8);
            }
        }
        lasttext = text;
        lastWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    void UpdateText()
    {
        foreach (GameObject line in lines)
        {
            if(!Application.isPlaying)
            DestroyImmediate(line);
            else
            Destroy(line);

        }
        foreach (Transform child in transform)
        {
             if(!Application.isPlaying)
            DestroyImmediate(child.gameObject);
            else
            Destroy(child.gameObject);


        }
        lines.Clear();
        currentline = null;
       
        for (int i = 0; i < text.Length; i++){
           
            if (text.ToCharArray()[i] != '\'')
            {
                if ((int)text.ToCharArray()[i] == 10)
                    {
                        if (currentline != null)
                        {
                            currentline = null;
                        }
                        else
                        {
                            currentline = Instantiate(ct.linefab, transform, true);
                            currentline.transform.localScale = Vector3.one;
                        currentline.transform.localPosition = new Vector3(currentline.transform.localPosition.x, currentline.transform.localPosition.y, 0);
                        if(alignedLeft){
                            currentline.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.LowerLeft;
                        }else{
                                currentline.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.LowerRight;
                            
                        }

                            lines.Add(currentline);
                        }
                    goto LinebreakSkip;
                    }
                   
                
                if(currentline == null){
                    currentline = Instantiate(ct.linefab, transform, true);
                    currentline.transform.localScale = Vector3.one;
                    currentline.transform.localPosition = new Vector3(currentline.transform.localPosition.x, currentline.transform.localPosition.y, 0);
                    if (alignedLeft)
                    {
                        currentline.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.LowerLeft;
                    }
                    else
                    {
                        currentline.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.LowerRight;

                    }
                    lines.Add(currentline);

                }

                GameObject character = Instantiate(ct.image, currentline.transform, true);
                character.transform.localScale = Vector3.one;
                character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas[text.Substring(i, 1)] as Sprite;
                character.transform.localPosition = new Vector3(character.transform.localPosition.x, character.transform.localPosition.y, 0);
                character.GetComponent<RectTransform>().sizeDelta = new Vector2(character.GetComponent<Image>().sprite.texture.width, character.GetComponent<Image>().sprite.texture.height);
                LinebreakSkip:
                ;
            }else if(text.ToCharArray()[i] == '\''){
                GameObject character = Instantiate(ct.image, currentline.transform, true);
                character.transform.localScale = Vector3.one;
                character.transform.localPosition = new Vector3(character.transform.localPosition.x, character.transform.localPosition.y, 0);
                if (i < text.Length - 1)
                {
                    switch (text.ToCharArray()[i + 1])
                    {
                        case 'd':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'d"] as Sprite;
                            break;
                        case 'l':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'l"] as Sprite;
                            break;
                        case 's':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'s"] as Sprite;
                            break;
                        case 't':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'t"] as Sprite;
                            break;
                        case 'v':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'v"] as Sprite;
                            break;
                        case 'r':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'r"] as Sprite;
                            break;
                        case 'm':
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas["'m"] as Sprite;
                            break;
                        default:
                            character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas[text.Substring(i, 1)] as Sprite;
                            goto SkipIncrement;
                    }
                    i++;
                SkipIncrement:
                    character.GetComponent<RectTransform>().sizeDelta = new Vector2(character.GetComponent<Image>().sprite.texture.width, character.GetComponent<Image>().sprite.texture.height);
                }else{
                    character.GetComponent<UnityEngine.UI.Image>().sprite = ct.atlas[text.Substring(i, 1)] as Sprite;
                    character.GetComponent<RectTransform>().sizeDelta = new Vector2(character.GetComponent<Image>().sprite.texture.width, character.GetComponent<Image>().sprite.texture.height);
                }
            }
        }
        foreach (GameObject line in lines)
        {
            RectTransform rt = line.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, 8);
        }
    }
}
