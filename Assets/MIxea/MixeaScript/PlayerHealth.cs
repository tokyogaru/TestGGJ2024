using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int hpMax = 100, hpCurrent;

    public float bounce = 5;
    private Rigidbody2D rb;

    public HealthBar hpBar;

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

            //Jump again
            rb.velocity = new Vector2(rb.velocity.x, bounce);
        }

        if (col.CompareTag("Enemy Hitbox"))
        {
            //Jump again
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
        if (hpCurrent <= 1)
        {
            Destroy(gameObject);
        }

        hpCurrent -= dmg;
        if(hpCurrent < 1)
        {
            hpCurrent = 1;
        }
        hpBar.SetHp(hpCurrent);
    }
}
