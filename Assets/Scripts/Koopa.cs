using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    //koopa is similar to goomba so we will copy and paste code and change as we need

    //need variable reference to shell sprite to update it when its killed
    public Sprite shellSprite;

    public float shellSpeed = 12f;

    private bool shelled;
    private bool pushed;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //check to see if goomba came into contact with mario which is tagged with player in the editor


        //added a condition of not being shelled
        if (!shelled && collision.gameObject.CompareTag("Player"))
        {
            //reference to player class, saf
            Player player = collision.gameObject.GetComponent<Player>();
            //going to preform a dot test for the direction that the player hit the goomba is going down
            //implying it landed on his head
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                EnterShell();
            }//otherwise if mario collides with goomba not from above
            else
            {
                player.Hit();
            }
        }
    }


    //we are using the box collider trigger for the shell physics
    private void OnTriggerEnter2D(Collider2D other)
    {
        //will only apply when koopa is in his shell\
        //check and make sure its player colliding
        if(shelled && other.CompareTag("Player"))
            {
            if (!pushed) { 
                //getting direction between 2 objects
                //subtracting to posistion vectors give you the direction we only care about x axis
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();
                player.Hit();
            }
        }
        //coipied from goomba with an added condition of not curently being shelled
        //if THIS koopa is not shelled and is walking currently but then there is another
        //layer of a shell thats been pushed
        // and it has collided
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }


    private void EnterShell()
    {
        shelled = true;
        //dont disable the collider like on goomba 
       // GetComponent<Collider2D>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
        //remove destroy because he is not destroyed
    }

    //need to keep track of the states of koopa


    private void PushShell(Vector2 direction) 
    //setting push variable to true
    {
        pushed = true;
        //renabled rigid body so it can move again, not kenematic means the physics engine will handle the movement
        GetComponent<Rigidbody2D>().isKinematic = false;

        //storing a temporary reference
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        //we need to add logic so that koopas shell will kill enemies
        gameObject.layer = LayerMask.NameToLayer("Shell");
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


    //this despawns the shell if it leaves the screen
    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }

}
