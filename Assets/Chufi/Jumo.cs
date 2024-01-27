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

    // Velocidad horizontal del jugador
    public float moveSpeed = 5f;

    // Componente de script a desactivar/activar
    public MonoBehaviour scriptComponent;

    private void Start()
    {
        onGround = true;
        jumpPressure = 0f;
        minJump = 5f;
        maxJumpPressure = 15f;
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            jumpPressure = minJump;
            transform.localScale = new Vector2(originalScale.x, 0.5f);
            onGround = false;
        }

        // Mantener presionado para saltar más alto
        if (Input.GetKey(KeyCode.Space) && !onGround)
        {
            if (jumpPressure < maxJumpPressure)
            {
                jumpPressure += Time.deltaTime * 50f;
            }
        }

        // Realizar el salto
        if (Input.GetKeyUp(KeyCode.Space) && !onGround)
        {
            rb.velocity = new Vector2(0f, jumpPressure);
            jumpPressure = 0f;
            transform.localScale = originalScale;
        }

        // Activar/desactivar el componente de script según el estado del jugador
        if (scriptComponent != null)
        {
            scriptComponent.enabled = onGround;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}

