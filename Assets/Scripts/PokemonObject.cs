using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PokemonObject : MonoBehaviour
{
   public Vector2 playerDistance;
    public bool isDisabled;
    public List<SpriteRenderer> spriteRenderers;
    [HideInInspector]
    public UnityEvent onEnabled = new UnityEvent(), onDisabled = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
       foreach(Transform go in transform)
        {
            SpriteRenderer spriteRend = go.GetComponent<SpriteRenderer>();
            if (spriteRend != null) spriteRenderers.Add(spriteRend);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Player.instance.transform.position - transform.position;
        playerDistance.y = Mathf.Abs(playerDistance.y);
        if ((playerDistance.x <= -6f || playerDistance.x >= 5f || playerDistance.y >= 5f))
        {

            if (!isDisabled)
            {
                //if the NPC isn't disabled, disable the NPC
                isDisabled = true;
                foreach (SpriteRenderer spriteRend in spriteRenderers)
                {
                    spriteRend.enabled = false;
                }
                onDisabled.Invoke();
            }
        }
        if ((playerDistance.x >= -5f && playerDistance.x <= 4f && playerDistance.y <= 4f) && isDisabled)
        {
            //enable the NPC
            isDisabled = false;
            foreach(SpriteRenderer spriteRend in spriteRenderers)
            {
                spriteRend.enabled = true;
            }
            onEnabled.Invoke();
        }
    }
}
