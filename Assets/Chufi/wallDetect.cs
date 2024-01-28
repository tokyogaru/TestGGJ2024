using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall");
            PieMov.stopMove();
        }
    }
}
