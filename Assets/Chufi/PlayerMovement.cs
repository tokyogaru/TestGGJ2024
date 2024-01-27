using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cuerpo;
public Transform pieIzq;
public Transform pieDer;

private bool moviendoseDerecha = false;
private bool moviendoseIzquierda = false;

private void Update() {

    if (!moviendoseIzquierda  && Input.GetKeyDown(KeyCode.D))
        {
            moviendoseDerecha = true;
        }

    if (moviendoseDerecha && Input.GetKeyUp(KeyCode.D))
        {
            moviendoseDerecha = false;
            MoveBody(pieDer.position);
        }
    if (!moviendoseDerecha && Input.GetKeyDown(KeyCode.A))
        {
            moviendoseIzquierda = true;
        }
    if (moviendoseIzquierda && Input.GetKeyUp(KeyCode.A))
        {
            moviendoseIzquierda = false;
            MoveBody(pieIzq.position);
        }


}
void MoveBody(Vector2 target) {
    cuerpo.position = new Vector3(target.x, cuerpo.position.y, cuerpo.position.z);
}


}
