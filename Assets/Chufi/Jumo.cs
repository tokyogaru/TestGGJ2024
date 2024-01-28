using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jumo : MonoBehaviour
{
    private bool onGround;
    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;
    private Rigidbody2D rb;
    private Vector3 originalScale;

    [SerializeField] private Sprite agachado;

    [SerializeField] private Sprite normal;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        onGround = true;
        jumpPressure = 0f;
        minJump = 5f;
        maxJumpPressure = 15f;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (onGround)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime * 50f;
                    spriteRenderer.sprite = agachado;
                    transform.localScale = new Vector2(originalScale.x, 0.5f); // Escalar en el eje Y a -1
                }
            }
            else
            {
                if (jumpPressure > 0f)
                {
                    rb.velocity = new Vector2(0f, jumpPressure);
                    jumpPressure = 0f;
                    onGround = false;
                    spriteRenderer.sprite = normal;
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
        }
    }
}

