using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);



    //this is a public getter so we can use it in other scripts like sprite renderer
    //but the state of the variable is only set by this class
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);


    private void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        
    }

 
    private void Update()
    {
        HorizontalMovement();

        //checks if we are grounded from the extensions method
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();    
        }


        ApplyGravity();
        

    }
    private void HorizontalMovement()
    {
        // accelerate / decelerate
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        //we need to reset velocity when running into a wall
        //this will be done by raycasting foward or backward
        //reusing the raycast extension method
        //multiplying by x velocity will give us left or right
        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        //here we make mario look left and right
        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180, 0f);
        }

    }

    //you will still be able to move while in the air but we want to be able to jump only when grounded
    private void GroundedMovement()
    {
        //we want to restrict gravity velocity from building up
        //we set it to either be your upward movement or is 0 while grounded
        velocity.y = Mathf.Max(velocity.y, 0f);


        //instead of setting jump to false when grounded,
        //problem for this is potential being grounded after jumping for a fram and it being set back to false
        // y vel will always be negative because of gravity until jumping

        jumping = velocity.y > 0f;



        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }


    private void ApplyGravity()
    {
        //to match mario phsysics we want normal gravity when going up/jumping
        //when falling we are going to double or tripple it

        //here make a bool to check if falling by his velocity going down and if we are not jumping
        //getbutton is different from getbuttondown, the prior is checking if the button is being held down 
        //if we are holding dfown jump we want more of a jump
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        //if falling we double gravity,
        //gives the effect that while holding down the jump button you are getting less gravity applied to you
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;

        //setting a terminal velocity
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }



    private void FixedUpdate()
    {
        // move mario based on his velocity

        Vector2 position = rigidbody.position;
         position += velocity * Time.fixedDeltaTime;

        // clamp within the screen bounds
         Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
         Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        // this fixes the running into camera velocity issue
        Vector2 tempPos = position;
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        if (position != tempPos) { velocity.x = 0f; }
        rigidbody.MovePosition(position);
    }

    //this is made by unity and will call somthing when mario has made a collision
    //we are making this so that his velocty resets when his head hits something
    //there are different things mario can hit his head on and we need to seperate what happens for each

    //after making goomba script we need to update this to be able to bounce off goomba after killing
    //this will work on any enemy
    //this changes power up if to else if
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //if mario collides with an enemy from above
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                //tell the game we are airborne
                jumping = true;
            }
        }
        //this is a check case to see if mario is hitting a power upfreom undernieth where we dont want him to lose velocity
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            //we are going to use dot product to determine how to handle powerups from above
           // velocity.y = 0f;
           if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }




}
