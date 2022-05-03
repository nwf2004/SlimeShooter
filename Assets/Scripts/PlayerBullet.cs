using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    [SerializeField]
    private LayerMask whatIsDamageable;

    //How much damage each bullet does
    [SerializeField]
    private float attack1Damage;

    private float[] attackDetails = new float[2];

    private PlayerShoot PlayerAim;

    [SerializeField]
    private GameObject PR;

    private Rigidbody2D BRB;

    //Speed of bullet
    Vector2 shootVector;

    private void Start()
    {

        BRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAim = GameObject.Find("PlayerAim").GetComponent<PlayerShoot>();
        shootVector = transform.right * PlayerAim.shootSpeed;
        StartCoroutine(travelTime());
    }

    private void Update()
    {
        //When bullet is instantiated travel in a straight line
        BRB.velocity = PR.GetComponent<Rigidbody2D>().velocity + shootVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the object that the bullet collides with is 9 (enemy) send the attack details to that enemy
        if (collision.gameObject.layer == 9)
        {
            attackDetails[0] = attack1Damage;
            attackDetails[1] = transform.position.x;

            collision.gameObject.transform.parent.SendMessage("Damage", attackDetails);
        }

        //If bullet collides with anything, destroy it
        Destroy(gameObject);
    }

    //Destroy the bullet if it travels for too long
    IEnumerator travelTime()
    {

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
