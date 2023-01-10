using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    //this will handle the animation code for coin not much logic
    //will update game manager to increase coins

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddCoin();
        StartCoroutine(Animate());
    }



    //we are going to just copy the code from the block hit animation because it acts similar.
    //increased the height and duration, we will also add a destroy be cause we will no longer need this object

    // to animate the blocks we will use a couritine
    private IEnumerator Animate()
    {

        //we dont want the block to do anything while it is animating
       
        Vector3 restingPosistion = transform.localPosition;

        //this is the posistion at the tip of its animate. we only need it to move up half a unit
        Vector3 animatedPosisition = restingPosistion + Vector3.up * 2f;

        yield return Move(restingPosistion, animatedPosisition);
        yield return Move(animatedPosisition, restingPosistion);

       Destroy(gameObject);

    }


    //we will animate with no tweening system
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = .25f;


        //we continue to animate and we calculate the percentage of time in the animation
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            //interpilate between the 2 posistions, getting the points between 2 points
            //t is our percentaged


            transform.localPosition = Vector3.Lerp(from, to, t);

            //update time from last frame
            elapsed += Time.deltaTime;

            //yield so it waits until next frame to continue on
            yield return null;
        }
        //sometimes the elapsed doesnt perfectly match the duration
        //make sure object is in correct final posistion
        transform.localPosition = to;
    }


}
