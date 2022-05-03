using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //A float between -1 to 1 depending on user input "A D"
    private float movementInputDirection;

    //float that counts up as soon as the player is hit by enemy
    private float knockbackStartTime;

    //User inputted float that determines how long the player is being knockedback for
    [SerializeField]
    private float knockbackDuration;

    //T/F for if the player is currently being knocked back
    private bool knockback;

    //Two floats that determine how far up and back the player moves when hit
    [SerializeField]
    private Vector2 knockbackSpeed;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStat PS;

    private bool isWalking;
    private bool isGrounded;
    private bool canJump;

    public float movementSpeed = 10.0f;
    public float jumpSpeed = 16.0f;

    //A detectionSphere that checks for the floor, this is the size
    public float groundCheckRadius;

    public LayerMask whatIsGround;

    //Position of a detectionSphere
    public Transform groundCheck;

    //Grabs the player reticle and the shoot function inside it
    [SerializeField]
    private GameObject PReticle;

    private PlayerShoot PShoot;



    void Start()
    {
        //Short cut names
        rb = GetComponent<Rigidbody2D>();
        PS = GetComponent<PlayerStat>();
        anim = GetComponent<Animator>();
        PShoot = PReticle.GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckKnockback();
        CheckIfCanJump();

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    //Constantly check to see if the player is touching the floor or not
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    //If the player is on the ground, allow for jumping
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    //Takes the direction of the initial knock back and applies it to knockback velocity
    private void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);

    }

    //Check to see when knockback is over
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    //Controls looking direction 
    private void CheckMovementDirection()
    {
        if (PShoot.facingLeft)
        {
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        }
        else if (!PShoot.facingLeft)
        {
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        //if moving set animation to walking
        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    //Reloading overrides all animation, if not reloading, or walking, animation is idle
    private void UpdateAnimation()
    {
        if (PShoot.isReloading)
        {
            anim.SetBool("isReloading", PShoot.isReloading);
        }
        else
        {
            anim.SetBool("isReloading", PShoot.isReloading);
            anim.SetBool("isWalking", isWalking);
        }

    }

    //sets movement direction to -1 or 1 depending on the keys A D
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        //if the space key is pressed jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    //Set y velocity to jump speed
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }


    //if not moving back multiple the direction by speed
    private void ApplyMovement()
    {
        if (!knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
    }

    //This method is called from the Enemy when it hits the player
    private void Damage(float[] attackDetails)
    {
        int direction;

        PS.DecreaseHealth(attackDetails[0]);

        if (attackDetails[1] < transform.position.x)
        {
            direction = 1;

        }
        else
        {
            direction = -1;
        }

        Knockback(direction);
    }

    //Draws the Sphere detection under the player
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
