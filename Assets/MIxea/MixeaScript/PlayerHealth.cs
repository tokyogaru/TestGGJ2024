using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int hpMax = 100, hpCurrent;

    public float bounce = 5;
    private Rigidbody2D rb;

    public HealthBar hpBar;
    public GameoverMenu overMenu;

    private bool lastHit;

    public bool touchEnemy;

    public PieMov pieMov;
    public GameObject pata;


    
    
    public PlayerEffect playerEffect;
    public GameObject scriptEffect;




    [SerializeField] private GameObject particleMad;

    [Header("Sfx")]
    [SerializeField] private AudioClip sfxHurt;
    [SerializeField] private AudioClip sfxDeath;
    [SerializeField] private AudioClip sfxHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scriptEffect.GetComponent<SpriteRenderer>();
        hpBar.SetMaxHp(hpMax);
        hpCurrent = hpMax;
        lastHit = false;
        particleMad.SetActive(false);

        pieMov = pata.GetComponent<PieMov>();
        

        playerEffect = scriptEffect.GetComponent<PlayerEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            PlayerEffect.particleHit.SetActive(true);
            PlayerEffect.particleDeadEnemy.SetActive(true);

            //col.GetComponent<DeathController>().DeathSelf();
            SoundManager.Instance.PlaySound(sfxHit, transform, 1f, 0);

            //Jump again
            rb.velocity = new Vector2(rb.velocity.x, bounce);
        }
      PlayerEffect.particleHit.SetActive(false);
      PlayerEffect.particleDeadEnemy.SetActive(false);

        if (col.CompareTag("Enemy Hitbox"))
        {

            LooseHp(25);
            playerEffect.Flash(Color.magenta);
            Debug.Log("Damaged");
        }

        if (col.CompareTag("Obstacle"))
        {
            LooseHp(25);
            playerEffect.Flash(Color.magenta);
            //Jump away
            rb.velocity = new Vector2(rb.velocity.x, bounce *2);
        }

        if (col.CompareTag("Pit"))
        {
            LooseHp(25);
            
            //Jump away
            rb.velocity = new Vector2(rb.velocity.x, bounce * 5);
        }

        if (col.CompareTag("Next Level"))
        {
            col.GetComponent<SceneManaging>().NextScene();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            
            

            Debug.Log("Collided");
        }
    }

    void LooseHp(int dmg)
    {
        
        if (hpCurrent == 25 || hpCurrent <= 25)
        {
            
        }
        
        hpCurrent -= dmg;
        playerEffect.Flash(Color.magenta);
        SoundManager.Instance.PlaySound(sfxHurt, transform, 1f, 0);

        if (hpCurrent < 25 && lastHit == false)
        {
            //1hp Magic pixel
            lastHit = true;
            hpCurrent = 10;
            particleMad.SetActive(true);
        }
        if (hpCurrent <= 0)
        {
            particleMad.SetActive(false);
            hpCurrent = 0;
            StartCoroutine(LoseAnim());
        }
        hpBar.SetHp(hpCurrent);
    }

    public IEnumerator LoseAnim()
    {
        pieMov.spriteRendererCuerpo.sprite = pieMov.perder;
        pieMov.cry.SetActive(true);
        yield return new WaitForSeconds(2f);
        overMenu.PauseGame();
    }
    
   
}
