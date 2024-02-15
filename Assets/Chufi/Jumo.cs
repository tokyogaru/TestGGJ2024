using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jumo : MonoBehaviour
{
    public bool onGround;
    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    public float jumpForce = 50;
    public float jumpTime = 0.5f;

    [SerializeField] GameObject sprite;

    public SpriteRenderer spriteRenderer;

    public Sprite agachado;
    public Sprite normal;

    public Sprite caida;

    public PieMov pieMov;

    public PlayerEffect playerEffect;

    public GameObject pata;

    public GameObject playerFX;

    private void Start()
    {
        onGround = true;
        jumpPressure = 0f;
        minJump = 5f;
        maxJumpPressure = 15f;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        pieMov = pata.GetComponent<PieMov>();
        playerEffect = playerFX.GetComponent<PlayerEffect>();

    }

    void Update()
    {
        if (onGround)
        {

            if (Input.GetKey(KeyCode.W) )
            {
                pieMov.enabled = false;
                playerEffect.enabled = false;

                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * (maxJumpPressure / jumpTime);
                    spriteRenderer.sprite = agachado;
                    transform.localScale = new Vector2(originalScale.x, 0.5f); // Escalar en el eje Y a -1
                }
            }
            else
            {
                pieMov.enabled = true;
                playerEffect.enabled = true;
                if (jumpPressure > 0f)
                {
                    StartCoroutine(LockJump());
                    rb.velocity = new Vector2(0f, jumpPressure);
                    jumpPressure = 0f;
                    spriteRenderer.sprite = caida;
                    transform.localScale = originalScale; // Restaurar la escala original
                }

            }
            

        }


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            spriteRenderer.sprite = normal;
        }

    }
    IEnumerator LockJump()
    {
        yield return new WaitForSeconds(0.025f);
        onGround = false;

    }
}

