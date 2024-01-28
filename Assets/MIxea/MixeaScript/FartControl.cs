using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartControl : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool moveRight;

    private Transform floorDetect1;
    private Transform floorDetect2;

    private bool farting;
    [SerializeField] private float fartTimer;
    private float fartTimerCurrent;
    [SerializeField] private float fartStartup;
    private float fartStartupCurrent;
    [SerializeField] private float restWait;
    private float restWaitCurrent;

    private bool fartCreated;
    private GameObject hitBox;

    public Sprite sp1, sp2;


    void Start()
    {
        moveRight = true;

        floorDetect1 = gameObject.transform.Find("Floor Detect 1");
        floorDetect2 = gameObject.transform.Find("Floor Detect 2");

        farting = false;
        fartTimerCurrent = fartTimer;
        fartCreated = false;

        hitBox = GameObject.Find("Fart Hitbox");
        hitBox.SetActive(false);

        sp1 = GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        AvoidFall();

        if (farting == false)
        {
            FartCountdown();
            GetComponent<SpriteRenderer>().sprite = sp1;
        }
        else
        {
            Farting();
            GetComponent<SpriteRenderer>().sprite = sp2;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            moveRight = !moveRight;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            moveRight = !moveRight;
            //col.gameObject.GetComponent<PlayerHealth>().health -= 5;
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
            moveRight = !moveRight;
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

    void FartCountdown()
    {
        if (fartTimerCurrent >= 0)
        {
            Moving();
            fartTimerCurrent -= Time.deltaTime;
        }
        else
        {
            //Reset Farting Timers
            fartStartupCurrent = fartStartup;
            restWaitCurrent = restWait;
            //Start farting
            farting = true;
        }
    }

    void Farting()
    {
        //FartCharge time
        if (fartStartupCurrent <= 0)
        {
            //Resting after fart time
            if(restWaitCurrent <= 0)
            {
                //Start FartCountdown()
                fartTimerCurrent = fartTimer;
                farting = false;
                fartCreated = false;
                hitBox.SetActive(false);
            }
            else
            {
                //Create fart cloud once
                if (fartCreated == false)
                {
                    fartCreated = true;
                    hitBox.SetActive(true);
                }

                //Timer
                restWaitCurrent -= Time.deltaTime;
            }
        }
        else
        {
            //Timer
            fartStartupCurrent -= Time.deltaTime;
        }
    }

}
