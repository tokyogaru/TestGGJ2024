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

    public bool touchEnemy;

    public PieMov pieMov;
    public GameObject pata;

    private GameObject particleHit; 
    private GameObject enemyDeath;

    PlayerEffect effectEnemy;
    public GameObject effectEnemyObject;

    [Header("Sfx")]
    [SerializeField] private AudioClip sfxHurt;
    [SerializeField] private AudioClip sfxDeath;
    [SerializeField] private AudioClip sfxHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        hpBar.SetMaxHp(hpMax);
        hpCurrent = hpMax;

        pieMov = pata.GetComponent<PieMov>();
        particleHit = GameObject.Find("hit_particle");
        enemyDeath = GameObject.Find("ENEMYdeath_particles");
        particleHit.SetActive(false);
        enemyDeath.SetActive(false);

        effectEnemy = effectEnemyObject.GetComponent<PlayerEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            touchEnemy = true;
            Destroy(col.gameObject);
            SoundManager.Instance.PlaySound(sfxHit, transform, 1f, 0);

            //Jump again
            rb.velocity = new Vector2(rb.velocity.x, bounce);
        }
        if(touchEnemy)
        {
            effectEnemy.Flash(Color.magenta);
            Vector3 newScale = new Vector3(1f, 0.5f, 1f); // Define la nueva escala (50% de la escala original en el eje Y)
            transform.localScale = Vector3.Scale(effectEnemy.originalScale, newScale); // Aplica la nueva escala al objeto
            particleHit.SetActive(true);
            enemyDeath.SetActive(true);
        }
        else
        {
            particleHit.SetActive(false);
            enemyDeath.SetActive(false);
        }

        if (col.CompareTag("Enemy Hitbox"))
        {

            LooseHp(5);
            
            Debug.Log("Damaged");
        }

        if (col.CompareTag("Obstacle"))
        {
            LooseHp(5);

            //Jump away
            rb.velocity = new Vector2(rb.velocity.x, bounce *2);
        }

        if (col.CompareTag("Pit"))
        {
            LooseHp(5);

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
            LooseHp(5);

            Debug.Log("Collided");
        }
    }

    void LooseHp(int dmg)
    {
        
        if (hpCurrent == 1 || hpCurrent <= 1)
        {
            StartCoroutine(LoseAnim());
        }

        hpCurrent -= dmg;
        SoundManager.Instance.PlaySound(sfxHurt, transform, 1f, 0);

        if (hpCurrent < 1)
        {
            //1hp Magic pixel
            hpCurrent = 1;
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
