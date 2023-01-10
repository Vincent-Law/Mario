using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    //need to detect when mario collides with block


    //there are different amount of times a block can be hit
    //we will  change this value in the editor for different blocks
    //infite starting at -1
    // single hit blocks will start at 1
    public int maxHits = -1;

    // we need to swap out sprites on the block depending on this value too
    public Sprite emptyBlock;


    private bool animating;


    //adding coin object
    //going to use item to spawn what ever we need to be spawned once block is hit
    public GameObject item;

    //to fix the bug of empty blocks being hit still we will add a condition to the if statement (maxHits !=0)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        
        if (collision.gameObject.CompareTag("Player"))
        {
            //need to find out where mario is hitting it from, that be it we want to make changes when mario is hitting from
            //underneith
            //we will do a dot test from our extensions
            //  this is mario              this is the block
            if(!animating && maxHits!=0 && collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }




    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //this is added for invisable blocks
        spriteRenderer.enabled = true;



        maxHits--;
        if(maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        //checking for item inside
        //sometimes we dont want to spawn anything and we dont want an error thrown
        if (item != null)
        {
            //spawning item  at blocks posistion with no rotation(quant)
            //
            Instantiate(item, transform.position, Quaternion.identity);
        }


        StartCoroutine(Animate());
    }


    // to animate the blocks we will use a couritine
    private IEnumerator Animate()
    {

        //we dont want the block to do anything while it is animating
        animating = true;
        Vector3 restingPosistion = transform.localPosition;

        //this is the posistion at the tip of its animate. we only need it to move up half a unit
        Vector3 animatedPosisition = restingPosistion + Vector3.up * 0.5f;

        yield return Move(restingPosistion, animatedPosisition);
        yield return Move(animatedPosisition, restingPosistion);

        animating = false;

    }


    //we will animate with no tweening system
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = .125f;


        //we continue to animate and we calculate the percentage of time in the animation
        while (elapsed <duration)
        {
            float t = elapsed / duration;

            //interpilate between the 2 posistions, getting the points between 2 points
            //t is our percentaged


            transform.localPosition = Vector3.Lerp(from, to, t);
            
            //update time from last frame
            elapsed += Time.deltaTime;

            //yield so it waits until next frame to continue on
            yield return null;
        }
        //sometimes the elapsed doesnt perfectly match the duration
        //make sure object is in correct final posistion
        transform.localPosition = to;
    }
}
