using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGrid : MonoBehaviour
{
    public Vector2 size;
    //public Color color; //For some reason Gizmos don't work when setting the color to a variable

    void OnDrawGizmos()
    {
        Vector3 startPos = new Vector3(transform.position.x - size.x/2f,transform.position.y - size.y/2f,transform.position.z);

        //The Gameboy's screen is 20x18 8x8 tiles big
        for(int x = 0; x <= 20; x++){
            Gizmos.color = Color.black;
            Gizmos.DrawLine(startPos + new Vector3(size.x*((float)x/20f),0,0),startPos + new Vector3(size.x*((float)x/20f),size.y,0));
        }
        for(int y = 0; y <= 18; y++){
            Gizmos.color = Color.black;
            Gizmos.DrawLine(startPos + new Vector3(0,size.y*((float)y/18f),0),startPos + new Vector3(size.x,size.y*((float)y/18f),0));
        }
    }
}
