using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectDer : MonoBehaviour
{
    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WallDerTrue");
            PieMov.walledDer = true;
        }
    
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WallDerFalse");
            PieMov.walledDer = false;
        }
    }
}
