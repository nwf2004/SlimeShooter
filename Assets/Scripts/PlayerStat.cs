using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for player health
public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private GameObject bloodParticle;

    private float currentHealth;

    private GameManager GM;

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    //Same die method as the enemy
    private void Die()
    {
        Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        Instantiate(bloodParticle, transform.position, bloodParticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);
    }

}
