using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    

    //we could use a gravity variable to customize gravity for out entities 
    //public float gravity = -9.81f;
    //not using because we are handeling in the editor


    private new Rigidbody2D rigidbody;
    private Vector2 velocity;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //this is so in the scene they start disabled
        enabled = false;
    }



    //enemies do not move become active until on screen which we will replicated with unity built in function


    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled=false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    //called at a fixed variable
    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        // this is thier acceleration
        rigidbody.MovePosition(rigidbody.position +velocity * Time.fixedDeltaTime);


        //here we are using raycast that we made in extensions to make the enemie turn around once it has touched somthing

        if (rigidbody.Raycast(direction))
        {
            direction = -direction;
        }

        //this runs awhile grounded so thier velocity down isnt just building
        if (rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }

    }




}
