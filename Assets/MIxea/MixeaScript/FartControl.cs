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
    public GameObject hitBox;

    public Sprite sp1, sp2;

    public Camera camRef;
    [SerializeField] private float screenBorder = 10;
    private Vector3 targetScreenPos;

    private bool isOffscreen;

    [Header("Sfx")]
    [SerializeField] private AudioClip sfxFart;

    [SerializeField] float rotationSpeed;

    private Vector3 originalPosition;

    public EnemyEffects enemyEff;

    

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

        sp1 = enemyEff.GetComponent<SpriteRenderer>().sprite;

        camRef = Camera.main;
        isOffscreen = true;

        originalPosition = transform.position;

    
    }

    void Update()
    {
        //Check if in camera
        targetScreenPos = camRef.WorldToScreenPoint(gameObject.transform.position);

        isOffscreen = targetScreenPos.x <= screenBorder || targetScreenPos.x >= Screen.width || targetScreenPos.y <= screenBorder || targetScreenPos.y >= Screen.height;

        /*
        if (isOffscreen)
        {
            Debug.Log(gameObject.name + " Is OffScreen");
        }
        else
        {
            Debug.Log("Is OnScreen");
        }*/
    }

    private void FixedUpdate()
    {
        AvoidFall();
        

        if (!isOffscreen)
        {
            if (farting == false)
            {
                FartCountdown();
                
                enemyEff.GetComponent<SpriteRenderer>().sprite = sp1;
            }
            else
            {
                Farting();
                enemyEff.StopCoroutine(enemyEff.RechargerPeo());
                enemyEff.GetComponent<SpriteRenderer>().sprite = sp2;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            moveRight = !moveRight;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        

        if (col.gameObject.CompareTag("Player"))
        {
            moveRight = !moveRight;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
            enemyEff.MoveChar();
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            enemyEff.MoveChar();
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
            enemyEff.StartCoroutine(enemyEff.RechargerPeo());
            farting = true;
        }
    }

    void Farting()
    {
        //FartCharge time
        if (fartStartupCurrent <= 0)
        {
            //Resting after fart time
            if (restWaitCurrent <= 0)
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
                    
                    SoundManager.Instance.PlaySound(sfxFart, transform, 1f, 3);
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
