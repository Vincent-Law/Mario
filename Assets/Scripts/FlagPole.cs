using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    //references 
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;

    //speed of animation, speed based instead of time based
    //we cant use time based because of the variations of mario touch the pole at different hights
    public float speed = 6f;

    

    //detect when mario touches
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        { 
            //start animation to move the flag to the bottom
            StartCoroutine(MoveTo(flag, poleBottom.position));

            //full animation for mario
            StartCoroutine(LevelCompleteSequence(collision.transform));

        }
    }


    private IEnumerator LevelCompleteSequence(Transform player)
    {
        //mario sequence
        player.GetComponent<PlayerMovement>().enabled= false;
        //4 posistions for the whole ending animation
        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);
    }

    private IEnumerator MoveTo(Transform subject, Vector3 destination)
    {
        //look while subject is not at the destination
        //we could do until it reaches the posistion but this gap between will just get smaller instead we want to just
        //know when its around at the bottom
        while (Vector3.Distance(subject.position, destination)> .125f)
        {
            //cant lurp because we would need to know the current and end posistion
            //so we just use movetowards
            //we multiplay speed times time to move them over time
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return  null;
        }

        //once we are close enough just set the posistion
        subject.position = destination;


    }





}
