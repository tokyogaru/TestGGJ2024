using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool moveRight;

    private Transform floorDetect1;
    private Transform floorDetect2;

    private bool shooting;
    [SerializeField] private float shootTimer;
    private float shootTimerCurrent;
    [SerializeField] private float shootStartup;
    private float shootStartupCurrent;
    [SerializeField] private float restWait;
    private float restWaitCurrent;

    private bool missileCreated;
    public GameObject missile;
    [SerializeField] private Transform missileSpawner;

    public Sprite spIdle, spShoot, spDie;
    private SpriteRenderer spRend;


    void Start()
    {
        moveRight = true;

        floorDetect1 = transform.Find("Floor Detect 1");
        floorDetect2 = transform.Find("Floor Detect 2");

        shooting = false;
        shootTimerCurrent = shootTimer;
        missileCreated = false;

        missileSpawner = transform.Find("Missile Spawner");

        spRend = GetComponent<SpriteRenderer>();
        spRend.flipX = true;
        spIdle = spRend.sprite;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        AvoidFall();

        if (shooting == false)
        {
            ShootCountdown();
            GetComponent<SpriteRenderer>().sprite = spIdle;
        }
        else
        {
            Shooting();
            GetComponent<SpriteRenderer>().sprite = spShoot;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Change directions
        if (col.gameObject.CompareTag("Wall"))
        {
            ChangeDirection();
        }

        if (col.gameObject.CompareTag("Player"))
        {
            ChangeDirection();
        }
    }

    void AvoidFall()
    {
        //Floor detection
        RaycastHit2D hit = Physics2D.Raycast(floorDetect1.transform.position, Vector2.down, 1);
        RaycastHit2D hit2 = Physics2D.Raycast(floorDetect2.transform.position, Vector2.down, 1);
        Debug.DrawRay(floorDetect1.transform.position, Vector2.down, Color.green);
        Debug.DrawRay(floorDetect2.transform.position, Vector2.down, Color.green);

        if (hit.collider == null || hit2.collider == null)
        {
            //Change directions
            ChangeDirection();
        }
    }

    void Moving()
    {
        if (moveRight)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
        }
    }

    void ChangeDirection()
    {
        moveRight = !moveRight;
        spRend.flipX = !spRend.flipX;

        missileSpawner.transform.localPosition = new Vector3(missileSpawner.transform.localPosition.x * -1, missileSpawner.transform.localPosition.y, missileSpawner.transform.localPosition.z);
    }

    void ShootCountdown()
    {
        if (shootTimerCurrent >= 0)
        {
            Moving();
            shootTimerCurrent -= Time.deltaTime;
        }
        else
        {
            //Reset shooting Timers
            shootStartupCurrent = shootStartup;
            restWaitCurrent = restWait;
            //Start shooting
            shooting = true;
        }
    }

    void Shooting()
    {
        //shootCharge time
        if (shootStartupCurrent <= 0)
        {
            //Resting after shoot time
            if (restWaitCurrent <= 0)
            {
                //Start shootCountdown()
                shootTimerCurrent = shootTimer;
                shooting = false;

                missileCreated = false;
            }
            else
            {
                //Create missile once
                if (missileCreated == false)
                {
                    missileCreated = true;
                    Instantiate(missile, missileSpawner.position, missileSpawner.rotation);
                }

                //Timer
                restWaitCurrent -= Time.deltaTime;
            }
        }
        else
        {
            //Timer
            shootStartupCurrent -= Time.deltaTime;
        }
    }

}
