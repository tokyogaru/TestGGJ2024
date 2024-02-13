using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Transform objeto;
    public Transform startPoint;
    public Transform endPoint;
    Vector2 targetPos;
    public float speed = 1;

    //int direction = 1;

    private void OnDrawGizmos()
    {
        if (objeto != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(objeto.transform.position, startPoint.position);
            Gizmos.DrawLine(objeto.transform.position, endPoint.position);
        }
    }

    /*Vector2 CurrentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }*/
    public void Start()
    {
        targetPos = startPoint.position;
    }
    void Update()
    {
        if (Vector2.Distance(objeto.transform.position, endPoint.position) < 0.1f) targetPos = startPoint.position;
        if (Vector2.Distance(objeto.transform.position, startPoint.position) < 0.1f) targetPos = endPoint.position;

        objeto.transform.position = Vector2.MoveTowards(objeto.transform.position, targetPos, speed * Time.deltaTime);

        /*Vector2 target = CurrentMovementTarget();
        objeto.position = Vector2.Lerp(objeto.position, target, speed * Time.deltaTime);

        float distance = (target - (Vector2)objeto.position).magnitude;

        if (distance <= 0.1f)
        {
            direction *= -1;
        }*/
    }
}
