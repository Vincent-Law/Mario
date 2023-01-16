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
            //reference to player class, saf
            Player player = collision.gameObject.GetComponent<Player>();

            //logic for starpower, instead of calling player.hit we calla goomba hit
            if (player.starpower)
            {
                Hit();
            }

            //going to preform a dot test for the direction that the player hit the goomba is going down
            //implying it landed on his head
            else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            }//otherwise if mario collides with goomba not from above
            else
            {
                player.Hit();
            }
        }
    }

    //getting hit by shell
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
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



    //on hit for getting hit by somthing, like shell

    private void Hit()
    {
        //disable animated sprite (walking)
        GetComponent<AnimatedSprite>().enabled = false;
        //goombva gets hit turn on death animation
        GetComponent<DeathAnimation>().enabled = true;
        //destroy after 3seconds
        Destroy(gameObject, 3f);
    }
    
    

}
