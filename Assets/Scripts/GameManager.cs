using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    //Where the player respawns when defeated
    [SerializeField]
    private Transform respawnPoint;

    //The player to respawn
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    //Global enemies defeated value
    public float enemiesDefeated = 0;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;

    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        CheckRespawn();
    }

    //Similar method to the knockback checking, wait for the respawn to be finished
    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    //Respawn player when the timer is up and have the camera follow the player again
    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;

        }
    }

}
