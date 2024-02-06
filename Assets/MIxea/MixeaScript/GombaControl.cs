using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GombaControl : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool moveRight;

    private Transform floorDetect1;
    private Transform floorDetect2;

    [SerializeField] float rotationSpeed;

    private Vector3 originalPosition;


    void Start()
    {
        moveRight = true;

        floorDetect1 = gameObject.transform.Find("Floor Detect 1");
        floorDetect2 = gameObject.transform.Find("Floor Detect 2");
        originalPosition = transform.position;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Moving();
        AvoidFall();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Enemy"))
        {
            moveRight = !moveRight;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void AvoidFall()
    {
        RaycastHit2D hit = Physics2D.Raycast(floorDetect1.transform.position, Vector2.down, 1);
        RaycastHit2D hit2 = Physics2D.Raycast(floorDetect2.transform.position, Vector2.down, 1);
        Debug.DrawRay(floorDetect1.transform.position, Vector2.down, Color.green);
        Debug.DrawRay(floorDetect2.transform.position, Vector2.down, Color.green);

        if (hit.collider == null || hit2.collider == null)
        {
            moveRight = !moveRight;
        }
    }

    void Moving()
    {
        if (moveRight)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
            MoveChar();
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            MoveChar();
        }
    }
    void MoveChar()
    {
        float rotation = Mathf.Sin(Time.time * rotationSpeed) * 1f - 0.5f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation * 5f);

    }
}
