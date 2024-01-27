using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieMov : MonoBehaviour
{
    [SerializeField] GameObject piernaDer;
    [SerializeField] GameObject piernaIzq;

    [SerializeField] GameObject pieDer;
    [SerializeField] GameObject pieIzq;

    [SerializeField] float legScale = 0.5f; // Escala de las piernas
    [SerializeField] float legPos = 0.5f;   // Posición de las piernas

    [SerializeField] float footPos = 0.1f;  // Posición de los pies

    [SerializeField] float tiempoMaximoTeclaPresionada = 3.0f; // Tiempo máximo que se permite mantener presionada la tecla


    private Vector3 initialPiernaDerPosition;
    private Vector3 initialPiernaIzqPosition;
    private Vector3 initialPieDerPosition;
    private Vector3 initialPieIzqPosition;

    private bool reachedMaxXDer = false;
    private bool reachedMaxXIzq = false;

    private bool movingRightLeg = false; // Indica si se está moviendo la pierna derecha
    private bool movingLeftLeg = false;

    private float tiempoTeclaPresionada = 0.0f; // Tiempo que se ha mantenido presionada la tecla

    void Start()
    {
        // Guardar las posiciones iniciales
        initialPiernaDerPosition = piernaDer.transform.localPosition;
        initialPiernaIzqPosition = piernaIzq.transform.localPosition;
        initialPieDerPosition = pieDer.transform.localPosition;
        initialPieIzqPosition = pieIzq.transform.localPosition;
    }

    void Update()
    {
        // Mover y escalar la pierna derecha con la tecla D
        if (!movingLeftLeg && !reachedMaxXDer && Input.GetKeyDown(KeyCode.D))
        {
            movingRightLeg = true;
        }

        if (movingRightLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            // Verificar si se alcanzó la posición máxima en el eje X para la pierna derecha
            if (piernaDer.transform.localPosition.x < 1.9f)
            {
                piernaDer.transform.localPosition += new Vector3(legPos, 0, 0);
                piernaDer.transform.localScale += new Vector3(legScale, 0, 0);
                pieDer.transform.localPosition += new Vector3(footPos, 0, 0);
            }
            else
            {
                reachedMaxXDer = true;
                //Debug.Log("Se alcanzó la posición máxima en el eje X para la pierna derecha.");
            }
        }

        if (movingRightLeg && Input.GetKeyUp(KeyCode.D))
        {
            tiempoTeclaPresionada = 0.0f;
            movingRightLeg = false;
            piernaDer.transform.localPosition = initialPiernaDerPosition;
            piernaDer.transform.localScale = Vector3.one;
            pieDer.transform.localPosition = initialPieDerPosition;
            reachedMaxXDer = false;
        }

        // Mover y escalar la pierna izquierda con la tecla A
        if (!movingRightLeg && !reachedMaxXIzq && Input.GetKeyDown(KeyCode.A))
        {
            movingLeftLeg = true;
        }

        if (movingLeftLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            // Verificar si se alcanzó la posición máxima en el eje X para la pierna izquierda
            if (piernaIzq.transform.localPosition.x > -1.9f)
            {
                piernaIzq.transform.localPosition -= new Vector3(legPos, 0, 0);
                piernaIzq.transform.localScale += new Vector3(legScale, 0, 0);
                pieIzq.transform.localPosition -= new Vector3(footPos, 0, 0);
            }
            else
            {
                reachedMaxXIzq = true;
                //Debug.Log("Se alcanzó la posición máxima en el eje X para la pierna izquierda.");
            }
        }

        if (movingLeftLeg && Input.GetKeyUp(KeyCode.A))
        {
            
            tiempoTeclaPresionada = 0.0f;
            movingLeftLeg = false;
            piernaIzq.transform.localPosition = initialPiernaIzqPosition;
            piernaIzq.transform.localScale = Vector3.one;
            pieIzq.transform.localPosition = initialPieIzqPosition;
            reachedMaxXIzq = false;
        }

        if(movingRightLeg && Input.GetKey(KeyCode.D))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
            }
        }
        if(movingLeftLeg && Input.GetKey(KeyCode.A))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
            }
        }
    }
}


