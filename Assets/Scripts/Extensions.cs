using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//extension methods require to be in a static class
public static class Extensions 
{
    //created for the circle cast to look for anything that is not the mario layer
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    //raycast extension method is used with "this"
    //we are extending a rigidbody and we can pass any other perameters we want in this case it is a vector 2
    //we are actually extending a circle down to the ground to mimic a collider
  public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        // If stationary return false or stationary
        if (rigidbody.isKinematic)
        {
            return false;
        }
        //the circle that is mimicing the collider
        //could use perameters but we will hard code it
        float radius = 0.25f;
        float distance = 0.375f;

        //same shape as marios collider
        //origin is mario's posistion
        //direction is being passed as a perameter
        //we also need a layer perameter so that we are targeting the right ground and not just hitting marios collider
        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);

        //the and operator is redundant because we using layers
        //returns if we hit somthing in the distance + radius
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    //dot product test, exdending transform, compare to another transform then what direction
    //we to 
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        //other is what we are colliding with, transform will be mario in most cases (is block above below)
        Vector2 direction = other.position - transform.position;
        //this will return a float (1 = same, 0 is perpidicular, -1 implies they are opposates)
        //normalise sets it to a simpler number
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }

}
