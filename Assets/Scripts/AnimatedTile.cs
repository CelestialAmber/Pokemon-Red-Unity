using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimatedTile : MonoBehaviour {
	public Sprite[] tileanimsprites;
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
	{
        if(Vector2.Distance(player.transform.position,transform.position) < 7){

            GetComponent<SpriteRenderer>().sprite = tileanimsprites[Mathf.FloorToInt((tileanimsprites.Length - 1) * (player.GetComponent<Player>().tileanimtimer/2))];

            }
        }
		

	
  

}

