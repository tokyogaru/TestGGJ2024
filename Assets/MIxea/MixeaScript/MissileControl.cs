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


    public Camera camRef;
    [SerializeField] private float screenBorder = 10;
    private Vector3 targetScreenPos;

    private bool isOffscreen;

    [Header("Sfx")]
    [SerializeField] private AudioClip sfxCharge;
    [SerializeField] private AudioClip sfxShoot;

    [SerializeField] float rotationSpeed;

    private Vector3 originalPosition;

    private SpriteRenderer spriteRenderer;

    private Vector3 originalScale;

    public EnemyEffects enemyEff;

    public GameObject enemyFxs;



    void Start()
    {
        moveRight = true;

        floorDetect1 = transform.Find("Floor Detect 1");
        floorDetect2 = transform.Find("Floor Detect 2");

        shooting = false;
        shootTimerCurrent = shootTimer;
        missileCreated = false;

        missileSpawner = transform.Find("Missile Spawner");

        spRend = enemyEff.GetComponent<SpriteRenderer>();
        spRend.flipX = true;
        spIdle = spRend.sprite;

        camRef = Camera.main;
        isOffscreen = true;

        originalPosition = transform.position;
        originalScale = transform.localScale;
        spriteRenderer = enemyEff.GetComponent<SpriteRenderer>();

        enemyEff = enemyFxs.GetComponent<EnemyEffects>();
      
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
            if (shooting == false)
            {
                ShootCountdown();
                enemyEff.GetComponent<SpriteRenderer>().sprite = spIdle;
            }
            else
            {
                Shooting();
                enemyEff.GetComponent<SpriteRenderer>().sprite = spShoot;
            }
        }

       
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Change directions
        if (col.gameObject.CompareTag("Wall"))
        {
            ChangeDirection();
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("player col");
            ChangeDirection();
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
            enemyEff.MoveChar();

        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            enemyEff.MoveChar();

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
            enemyEff.StartCoroutine(enemyEff.RedFadeAndScale());
            //Start shooting

            shooting = true;
            SoundManager.Instance.PlaySound(sfxCharge, transform, 1f, 3);

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
                    enemyEff.StopCoroutine(enemyEff.RedFadeAndScale());

                    //SFX
                    SoundManager.Instance.PlaySound(sfxShoot, transform, 1f, 3);
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
