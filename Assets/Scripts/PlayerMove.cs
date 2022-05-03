using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int playerSpeed = 10;
    public int playerJumpPower = 1250;
    private float moveX;
    public bool isGrounded;
    public float distanceToBottomOffPlayer = 1.8f;

    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;

    private Rigidbody2D rb;

    private PlayerStat PS;

    [SerializeField]
    private GameObject PReticle;

    private PlayerShoot PShoot;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PS = GetComponent<PlayerStat>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckKnockback();
        if (!knockback)
        {
            Player_Move();
        }
    }

    private void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
        
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    void Player_Move()
    {
        //CONTROLS
        moveX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            Jump();
        }
        //ANIMATION

        //PLAYER DIRECTION
        if (moveX < 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (moveX > 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;

        }
        //PHYSICS
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    void Jump()
    {
        //JUMPING CODE
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
        isGrounded = false;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

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
}
