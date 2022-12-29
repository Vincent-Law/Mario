using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    //this is going to handle changing mario from big to small

    private SpriteRenderer spriteRenderer;

    private PlayerMovement movement;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //inparent gets the component from mario instead of big or small where the playermovement script is not found
        movement = GetComponentInParent<PlayerMovement>();
    }

    //need to enable and disable the sprite renderer
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {

        //we are handeling our running with  the animated sprites since it is more that one sprite
        // we also need to make sure this is in the correct order
        //dont want to show a running animation while jumping
        // same thing with sliding we are moving which meets the condition for the running animation but the if and else if
        //come after
        run.enabled = movement.running;

        if (movement.jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (movement.sliding)
        {
            spriteRenderer.sprite = slide;
        }
 //else if youre not running
        else if (!movement.running)
        {
            spriteRenderer.sprite = idle;
        }
    }
}
