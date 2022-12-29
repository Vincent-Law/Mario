
using UnityEngine;
//this is everything we need for animating  a game object and is reusable accross our game
public class AnimatedSprite : MonoBehaviour
{
    // array of sprites for the animation that we cylce between
    public Sprite[] sprites;
    //cyle 6fps, made public so we can change for each game object
    public float framerate = 1f / 6f;


    // reference to sprite renderer to update that sprite
    private SpriteRenderer spriteRenderer;

    //need to know what index for the frame
    private int frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    // when we enable this feature we need to start our animation
    private void OnEnable()
    {
        //built in unity function
        //invokes some function every x amount of seconds
        // we use nameof to pass the function with that name
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    private void OnDisable()
    {
        //cancels any invokes 
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;


        //check if we reacched the end to loop back to the beggining
        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        //makes sure we are in the bounds of the array
        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }
    }

}
