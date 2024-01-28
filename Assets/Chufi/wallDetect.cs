using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WallTrue");
            PieMov.wall = true;

        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            PieMov.wall = false;
            Debug.Log("Wall False");
        }
    }
}
