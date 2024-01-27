using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RocketControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = 5;
    private float rotSpeed = 200;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 direccion = (Vector2)target.position - rb.position;

        direccion.Normalize();

        float rotAmount = Vector3.Cross(direccion, transform.up).z;

        rb.angularVelocity = -rotAmount * rotSpeed;

        rb.velocity = transform.up * speed;
    }

    private void RotateRocket()
    {
    }
}
