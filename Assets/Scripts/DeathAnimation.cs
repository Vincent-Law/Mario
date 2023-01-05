using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    //death animation that can be used anywhere

    //public spriterenderer for some cases later on
    //where there will be different sprite renderers and we manually assign this
    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        //the point of this is so that when the object dies and falls its brought to the forground
        //so we move it to the front of the sorting layer
        spriteRenderer.sortingOrder = 10;

        if (deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }
        
    }


    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        //lets them fall through the ground
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }


        GetComponent<Rigidbody2D>().isKinematic = true;


        //we are chacking to see which movement has been nulled and disabling it
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (entityMovement != null)
        {
            entityMovement.enabled = false;
        }

    }

    private IEnumerator Animate()
    {
        //this will animate in a fixed variable


        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;


        Vector3 velocity = Vector3.up * jumpVelocity;

        while (elapsed < duration)
        {//everyframe of the death animation we are in this while loop until elapsed has met duration
            //changing posistion over time
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;

            elapsed += Time.deltaTime;
            
            yield return null;
        }


    }

}
