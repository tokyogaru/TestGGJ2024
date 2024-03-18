using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jumo : MonoBehaviour
{
   private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    public float jumpForce = 50;
    public float jumpTime = 0.5f;
    private bool canJump = true;

    [SerializeField] GameObject sprite;

    private SpriteRenderer spriteRenderer;

    public Sprite agachado;
    public Sprite normal;

    public Sprite salto;

    public Sprite pose;

    private void Start()
    {
        jumpPressure = 0f;
        minJump = 5f;
        maxJumpPressure = 15f;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Raycast para detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        if (hit.collider != null)
        {
            if (Input.GetKey(KeyCode.W) && canJump)
            {
                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * (maxJumpPressure / jumpTime);
                    spriteRenderer.sprite = agachado;
                    transform.localScale = new Vector2(originalScale.x, 0.5f); // Escalar en el eje Y a -1
                }
            }
            else
            {
                if (jumpPressure > 0f)
                {
                    spriteRenderer.sprite = salto;
                    StartCoroutine(LockJump());
                    if (Input.GetKey(KeyCode.A))
                    {
                        spriteRenderer.sprite = pose;
                    }
                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        spriteRenderer.sprite = salto;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        spriteRenderer.sprite = pose;
                    }
                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        spriteRenderer.sprite = salto;
                    }
                    rb.velocity = new Vector2(0f, jumpPressure);
                    jumpPressure = 0f;
                    transform.localScale = originalScale; // Restaurar la escala original
                    canJump = false; // Desactivar el salto
                }
            }
        }
        else
        {
            spriteRenderer.sprite = salto; // Cambia al sprite de salto
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            spriteRenderer.sprite = normal;
            canJump = true; // Permitir saltar cuando toque el suelo
        }
    }

    IEnumerator LockJump()
    {
        yield return new WaitForSeconds(0.025f);
    }
}

