using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class updatecanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		VerticalLayoutGroup hlg = gameObject.GetComponent<VerticalLayoutGroup>();
			Canvas.ForceUpdateCanvases();
			hlg.CalculateLayoutInputHorizontal();
			hlg.CalculateLayoutInputVertical();
			hlg.SetLayoutHorizontal();
			hlg.SetLayoutVertical();

	}
}
