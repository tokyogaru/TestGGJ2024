using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    private BoxCollider2D[] selfColliders;
    private Rigidbody2D rb;
    [SerializeField] private Behaviour disableScript;


    private void Start()
    {
        selfColliders = gameObject.GetComponents<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        //Testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            DeathSelf();
        }
    }

    public void DeathSelf()
    {
        //Disable Colliders
        foreach (var col in selfColliders)
        {
            col.enabled = false;
        }
       
        rb.gravityScale = 0f;
        disableScript.enabled = false;
        Debug.Log("morido");
    }
}
