using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5;
    private float rotSpeed = 200;

    private float lifeSpan;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        lifeSpan = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        lifeSpan -= Time.deltaTime;

        Vector2 direccion = (Vector2)target.position - rb.position;
        direccion.Normalize();

        float rotAmount = Vector3.Cross(direccion, transform.up).z;

        rb.angularVelocity = -rotAmount * rotSpeed;
        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
