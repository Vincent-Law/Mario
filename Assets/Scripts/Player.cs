using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //managing state of player

    //when mario is dying we dont want the other sprites to be occuring or changing
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    //this is to keep track of what size we are currently
    private PlayerSpriteRenderer activeRenderer;
    //logic for changing the hitbox collider for big mario
    private CapsuleCollider2D capsuleCollider;

    public DeathAnimation deathAnimation;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;

    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider= GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    //we added a star power condition encapsulating our logic so that when ever we have starpower we cannot get hit
    public void Hit()
    {
        if (!starpower && !dead)
        {
            if (big)
            {
                Shrink();

            }
            else
            {
                Death();
            }
        }
    }

  

    private void Death()
    {
        //this will stop mario sprites from updating
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;

        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }


  //logic for magic mushroom
    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        //we need to keep track of what sprite we are on (big or small)
        activeRenderer = bigRenderer;
        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        //we need to keep track of what sprite we are on (big or small)
        activeRenderer = smallRenderer;
        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    //we will make a couritine for the grow and shrink animation
    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            //this is looking for every 4 frames to switch renderers
            if(Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;

        }
        //we disable all renderers and tell it to look at the current active one and set it to it
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;



    }

    //function for starpower. public to be called from powerup
    //we are defaulting to ten but this is showing us that in powerup.cs we could have a value that we pass to 
    // change the duration time if needed
    public void Starpower(float duration = 10f)
    {
        StartCoroutine(StarpowerAnimation(duration));

    }

    //courtine for starpower animation
    // we are taking this approach instead of having new sprite sets for every single animation like walk run jump
    private IEnumerator StarpowerAnimation(float durtation)
    {
        starpower = true;
        float elapsed = 0f;
        while (elapsed < durtation)
        {
            elapsed += Time.deltaTime;
            if(Time.frameCount % 4 == 0)
            {
                // our aproach wil be to access the sprite renderer and change its color there
                // the issue is we already have active renderer here for the different animation
                // we need to access SpriteRenderer in PlayerSpriteRenderer.cs but it is private

                //with colorHSV, we are getting any random hue from 0-1, Satuarition forced to one
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }
        //after aniumation we set the spriterenderer color back to white
        activeRenderer.spriteRenderer.color = Color.white;
        starpower= false;
    }

    

}