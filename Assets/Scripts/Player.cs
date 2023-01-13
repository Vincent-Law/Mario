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

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider= GetComponent<CapsuleCollider2D>();
    }


    public void Hit()
    {
        if(big)
        {
            Shrink();

        }else
        {
            Death();
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

    

}