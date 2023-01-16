using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
   //script will be used for the power up logic for interacting with mario
   public enum Type
    {
        Coin,
        ExtraLife,
        MagicMuchroom,
        StarPower,
    }

    //logic for giving the actual game object a type 
    public Type type;

    //we will use the box collider to know when we have entered the item
    //we could use the circle collider but we want the physics to be handled seprately
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //lets check what object we are colliding with 
        if (collision.CompareTag("Player"))
        {
            //when we collect we need to switch on what type it is
            //we are calling collect on mario
            Collect(collision.gameObject);

        }



    }


    //we also want to pass a reference to the player(game object Not Script) so we can make the accoring changes to mario
    //collect will look at mario and run the according Enum
    private void Collect(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                //this is looking at current Instance of game manager and running AddCoin*
                GameManager.Instance.AddCoin();
                
                break;
            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;
            case Type.MagicMuchroom:
                //we are getting the player script and calling the grow function on it
                player.GetComponent<Player>().Grow();
                
                break;
            case Type.StarPower:
                //this will be setting a state change, adding an animation
                player.GetComponent<Player>().Starpower();
                break;

        }
        //after its been collected destroy it
        Destroy(gameObject);
    }

    


}
