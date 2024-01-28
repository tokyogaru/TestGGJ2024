using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectIzq : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WallIzqTrue");
            PieMov.walledIzq = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WallDerFalse");
            PieMov.walledIzq = false;
        }
    }
}
