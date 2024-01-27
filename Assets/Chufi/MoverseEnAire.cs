using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverseEnAire : MonoBehaviour
{
   public float velocidadMovimiento = 5f; // Velocidad de movimiento

    // Update is called once per frame
    void Update()
    {
        // Obtener la entrada del teclado
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Calcular el desplazamiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0f, 0f) * velocidadMovimiento * Time.deltaTime;

        // Aplicar el desplazamiento
        transform.Translate(movimiento);
    }
}
