using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //managing state of player

    //when mario is dying we dont want the other sprites to be occuring or changing
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;


    public DeathAnimation deathAnimation;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;

    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }


    public void Hit()
    {
        if(big)
        {
            Shrink();

        }if (small)
        {
            Death();
        }
    }


    private void Shrink()
    {

    }
    

    private void Death()
    {
        //this will stop mario sprites from updating
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;

        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

}