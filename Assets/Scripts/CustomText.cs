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
    float lastWidth;
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
    }
    void Start()
    {
    }

    void OnEnable()
    {
        Update();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void UpdateText()
    {
    }
}
