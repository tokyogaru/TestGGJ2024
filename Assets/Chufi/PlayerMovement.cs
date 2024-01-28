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

    [SerializeField] private float velocidadMovimiento = 5f; // Velocidad de movimiento ajustable

    void Update()
    {
        if (!moviendoseIzquierda && Input.GetKeyDown(KeyCode.D))
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

    void MoveBody(Vector2 target)
    {
        // Calcula la posición objetivo en el eje X, manteniendo la posición Y y Z
        Vector3 targetPosition = new Vector3(target.x, cuerpo.position.y, cuerpo.position.z);

        // Calcula la dirección y distancia al objetivo
        Vector3 direction = (targetPosition - cuerpo.position).normalized;
        float distanceToMove = velocidadMovimiento * Time.deltaTime;

        // Mueve el cuerpo hacia la posición del pie objetivo
        cuerpo.position += direction * distanceToMove;
    }
}
