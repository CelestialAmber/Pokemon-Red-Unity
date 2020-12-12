using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TownMapLocation {
    public string name;
    public int xPos, yPos; //map cursor position on town map screen
}

[System.Serializable]
public class MapLocation{
    public Map map; //internal location
    public int townMapLocationIndex; //location shown on town map
}
public class MapMenu : MonoBehaviour
{

    public Image mapCursor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
