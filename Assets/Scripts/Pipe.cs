using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Pipe : MonoBehaviour
{

    public Transform connection;
    
    //we want a public variable to know what pipe we are trying to enter from IE down up or up down
    public KeyCode enterKeyCode = KeyCode.S;


    public Vector3 enterDirection = Vector3.down;
    //we use vector zero for non animated pipe exits
    public Vector3 exitDirection = Vector3.zero;

    //we want to check to see if mario is in the trigger box
    // we cant do on triggerenter because that is a single fram and we do not know what
    // what frame mario is going to try to go down the pipe
    private void OnTriggerStay2D(Collider2D other)
    {
        //connection being null is if the pipe doesnt lead anywhere
        if (connection != null && other.CompareTag("Player")) 
        {
            if (Input.GetKey(enterKeyCode)) {

                //passing the transform of mario (other) to the couritine animation
                StartCoroutine(Enter(other.transform));


            }


        }


        

    }


    //this is called on pipe enter
    private IEnumerator Enter(Transform player)
    {




        //disable movement
        player.GetComponent<PlayerMovement>().enabled = false;
        //find out what posistion to animate mario to
        Vector3 enteredPosistion = transform.position + enterDirection;
        //scale mario down
        Vector3 enteredScale = Vector3.one * 0.5f;

        //after getting and checking components we actually start the move
        yield return Move(player, enteredPosistion, enteredScale);
        //we wait here to let it be more smooth
        yield return new WaitForSeconds(1f);


        //check if we are underground
        bool underground = connection.position.y < 0f;
        //we need to reference the camera script to tell is that mario is underground
        //unity looks at the Main camera, finds the component sidescrolling and calls setunderground
        //then pass underground which if its true or false
        Camera.main.GetComponent<SideScrolling>().SetUnderground(underground);

        //sometimes when you exit is a fixed location rather than a pipe
        //we set up for this in our original variables 
        //we can check to see if the exit direction is zero or not

        //this is for exiting a pipe
        if (exitDirection != Vector3.zero)
        {
            //we animation in to out

            //we are posistioning him inside the pipe, by subtracting exit direction from the exit posistion
            player.position = connection.position - exitDirection;
            //we can just use the same move animation
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else//exiting to specififc location/ no animation needed
        {
            player.position = connection.position;
            player.localScale = Vector3.one;


        }
        player.GetComponent<PlayerMovement>().enabled = true;

    }


    //move animation,m slightly different than our other animations
    private IEnumerator Move(Transform player, Vector3 endPosistion, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f;


        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;
 
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            //linearly moveing mario from start to end posistion with respect to time
            player.position = Vector3.Lerp(startPosition, endPosistion, t);
            //same with scale
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;


        }
        //check mario scale and posistion after ending animation
        player.position = endPosistion;
        player.localScale = endScale;


    
    }


}
