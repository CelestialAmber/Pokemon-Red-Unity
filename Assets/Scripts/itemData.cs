using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TilesData {
    public bool hasText, onlyonce, triggered;
    public int TextID, coinamount;
    public string itemName;
}
public class itemData : MonoBehaviour  {
    public TilesData data;

	
}
