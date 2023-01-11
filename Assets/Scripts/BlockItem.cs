using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Animate());
    }



    private IEnumerator Animate()
    {
        //getting reference to rigid body
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        //getting reference to circle collider
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        //... circle collider
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //you cant disable the rigid body in unity but we can make it kinematic which means
        //the physics engine wont simulate physics on it
        rigidbody.isKinematic = true;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;


        //we are briefly disabling the sprite renderer
        //this is so that the block animation can finish before we see the item animation
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.enabled = true;


        //animation variables
        float elapsed = 0f;
        float duration = 0.5f;

        //posistions for vector of movement
        Vector3 startPosistion = transform.localPosition;
        Vector3 endPosistion = transform.localPosition + Vector3.up;


        while (elapsed < duration)
        {
            float t = elapsed / duration;
            //make sure to use consistancy with posistion we changed from posistion to localPosistion
            transform.localPosition = Vector3.Lerp(startPosistion, endPosistion, t);
            elapsed += Time.deltaTime;

            yield return null;

        }
        //good habit is to update the posistion of the object to what ever its supposed to be at the end of the animation
        transform.localPosition = endPosistion;
        


        //allowing physics engine on it agin
        rigidbody.isKinematic = false;
        //turn everythin back on
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;



    }
 


    //unlike the blockcoin didnt have any colliders or rigid  bodies. 
    //we need to disable the components that we added to the object when its spawning in and animating through the box
    // the colliders and boxes will cross over the blocks




}
