using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float legLength;
    [SerializeField] private LayerMask legLayer;
    // Start is called before the first frame update

    private Vector3 legPoint;
    private DistanceJoint2D joint;
    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.D))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                origin:  transform.position,
                direction: Vector2.right,
                distance: Mathf.Clamp(0, 1, 50),
                layerMask: legLayer
            );

            if(hit.collider != null)
            {
                legPoint = hit.point;
                legPoint.z = 0;
                legPoint.y = 0;
                joint.connectedAnchor = legPoint;
                joint.enabled = true;
                joint.distance = legLength;
            }
        }
    }
}
