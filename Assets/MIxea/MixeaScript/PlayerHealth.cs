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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(col.gameObject);
            SoundManager.Instance.PlaySound(sfxHit, transform, 1f, 0);

            //Jump again
            rb.velocity = new Vector2(rb.velocity.x, bounce);
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
        if (hpCurrent == 1)
        {
            overMenu.PauseGame();
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
}
