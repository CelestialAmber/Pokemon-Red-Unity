using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Version{
    Red,
    Blue
}
[ExecuteInEditMode]
public class VersionManager : MonoBehaviour {
    public Version version;
    public Image frame;
    public Sprite[] frames;
	// Update is called once per frame
	void Update () {
        frame.sprite = frames[(int)version];
	}
}
