using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Walking,
        Knockback,
        Dead
    }

    private State currentState;

    //size of the raycast that checks for the ground
    [SerializeField]
    private float groundCheckDistance;

    //size of the raycast that checks for the walls
    [SerializeField]
    private float wallCheckDistance;

    //movement speed float
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float maxHealth;

    
    [SerializeField]
    private float knockbackDuration;

    //float for how long before enemy can damage you again after damaging
    [SerializeField]
    private float touchDamageCooldown;
    [SerializeField]
    private float touchDamage;

    //detection box for damaging the player
    [SerializeField]
    private float touchDamageWidth;
    [SerializeField]
    private float touchDamageHeight;
 


    private float currentHealth;
    private float knockbackStartTime;
    //details of enemy attack
    private float[] attackDetails = new float[2];
    private float lastTouchDamageTime;

    //Grabs position for 3 emptys attached to the enemy that represent the check for ground, walls, and the hitbox
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform touchDamageCheck;

    //Checks for ground and player
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private Vector2 knockbackSpeed;

    //Particles that play on death
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private GameObject bloodParticle;

    private int facingDirection;
    private int damageDirection;

    private Vector2 movement;
    private Vector2 touchDamageBottomLeft;
    private Vector2 touchDamageTopRight;

    private bool touchingGround;
    private bool touchingWall;

    [SerializeField]
    private GameManager GM;

    //Enemy game object that is a child of this empty
    [SerializeField]
    private GameObject alive;

    //rigid body of the enemy
    private Rigidbody2D aliveRB;

    void Start()
    {
        aliveRB = alive.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        facingDirection = 1;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;

        }
    }

    //When defeated, count the enemy defeated by 1
    private void OnDestroy()
    {
        GM.enemiesDefeated += 1;
    }

    //Walking
    private void EnterWalkingState()
    {

    }

    //Moving code
    private void UpdateWalkingState()
    {
        //Constantly move the ground and wall checks with the enemy
        touchingGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        touchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        //If touching a wall or not touching floor, turn around
        if (!touchingGround || touchingWall)
        {
            Flip();
        }
        //Move
        else
        {
            movement.Set(movementSpeed * facingDirection, aliveRB.velocity.y);
            aliveRB.velocity = movement;
        }

    }

    private void ExitWalkingState()
    {

    }

    //KnockBack
    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRB.velocity = movement;

    }

    private void UpdateKnockbackState()
    {
        //When the knockback is over resume moving 
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Walking);
        }
    }

    private void ExitKnockbackState()
    {
        
    }

    //On death, create particles and destroy object
    private void EnterDeadState()
    {
        
        Instantiate(deathParticle, alive.transform.position, deathParticle.transform.rotation);
        Instantiate(bloodParticle, alive.transform.position, bloodParticle.transform.rotation);
        Destroy(gameObject);
    }

    private void UpdateDeadState()
    {

    }

    private void ExitDeadState()
    {

    }

    //When enemy recieves damage from a bullet
    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //if the enemy lived, knock back, else die
        if (currentHealth > 0.0f && touchingGround)
        {
            SwitchState(State.Knockback);
        }
        else if(currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }

    //Check to see if the enemy is touching the player, if the detection box is, call the damage method to the player
    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBottomLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBottomLeft, touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    //Flip direction of enemy
    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);

    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    //Displays groundCheck, wallCheck, and the touchDamageCheck in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 bottomLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 bottomRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2)); 
        Vector2 TopRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2)); 
        Vector2 TopLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, TopRight);
        Gizmos.DrawLine(TopRight, TopLeft);
        Gizmos.DrawLine(TopLeft, bottomLeft);

    }

}
