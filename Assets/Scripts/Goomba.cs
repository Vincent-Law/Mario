using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    //need variable reference to flat sprite to update it when its killed
    public Sprite flatSprite;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        //check to see if goomba came into contact with mario which is tagged with player in the editor
        if(collision.gameObject.CompareTag("Player"))
        {
            //going to preform a dot test for the direction that the player hit the goomba is going down
            //implying it landed on his head
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }
        }
    }


    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

}
