using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //Controls firing speed
    public bool gunReady = true;
    public GameObject bullet;
    public float shootSpeed;
    GameObject newBullet;

    //Firing speed float
    [SerializeField]
    private float firingSpeed;

    [SerializeField]
    private Camera mainCamera;

    //a random spread float
    [SerializeField]
    private float spread;

    public bool facingLeft;

    private int bulletsInMag;
    private int bulletsToReload = 5;

    public bool isReloading = false;

    public AudioSource shoot;
    public AudioSource[] reload;
    public AudioSource ready;
    public AudioSource empty;

    //When fired, make sure the gun cant fire again until its ready, determined by the firing speed
    IEnumerator fireRate()
    {


        gunReady = false;
        yield return new WaitForSeconds(firingSpeed);
        gunReady = true;


    }

    //when reloading, cannot shoot or reload again until the reload is finished, reload each bullet individually
    IEnumerator reloading()
    {

        isReloading = true;
        for (int i = 0; i < bulletsToReload - bulletsInMag; i++)
        {
            reload[Random.Range(0, 2)].Play();
            yield return new WaitForSeconds(0.36f);
        }


        ready.Play();
        bulletsInMag = bulletsToReload;
        isReloading = false;
    }

    void Start()
    {
        bulletsInMag = 5;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        AimReticle();
        CheckKeys();
    }

    //Move the reticle to face the user mouse input at all times
    private void AimReticle()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        //Code for determining whether the reticle is on the left side or right
        transform.right = direction;
        if (direction.x < 0)
        {
            facingLeft = true;
        }
        else
        {
            facingLeft = false;
        }
       

    }

    private void CheckKeys()
    {
        if (Input.GetMouseButton(0) && gunReady == true && !isReloading)
        {
            //if the gun isnt empty, fire 5 bullets in random direction based on spread
            if (bulletsInMag > 0)
            {
                shoot.Play();
                for (int i = 0; i < 5; i++)
                {
                    float randomSpread = Random.Range(-spread, spread);
                    newBullet = Instantiate(bullet);

                    newBullet.transform.position = transform.position + (transform.right);
                    newBullet.transform.rotation = transform.rotation;
                    newBullet.transform.rotation = Quaternion.Euler(0, 0, newBullet.transform.eulerAngles.z + randomSpread);

                }
                StartCoroutine(fireRate());
                bulletsInMag -= 1;
            }
            //if the gun is empty play empty sound
            else if (Input.GetMouseButton(0) && gunReady)
            {
                empty.Play();
                StartCoroutine(fireRate());
            }
        }
        //if the user hits R, and the gun has at least 1 bullet misisng, reload
        if (Input.GetKey(KeyCode.R) && bulletsInMag < bulletsToReload && !isReloading)
        {
            StartCoroutine(reloading());
        }
        
    }
}
