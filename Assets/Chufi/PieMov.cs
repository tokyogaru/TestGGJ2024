using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieMov : MonoBehaviour
{
    [SerializeField] GameObject piernaDer;
    [SerializeField] GameObject piernaIzq;
    [SerializeField] GameObject pieDer;
    [SerializeField] GameObject pieIzq;
    [SerializeField] GameObject cuerpo;

    [SerializeField] float legScale = 0.5f;
    [SerializeField] float legPos = 0.5f;
    [SerializeField] float footPos = 0.1f;
    [SerializeField] float tiempoMaximoTeclaPresionada = 3.0f;

    [SerializeField] private Sprite normal;

    [SerializeField] private Sprite pose;

    [SerializeField] private Sprite perder;

    private Vector3 initialPiernaDerPosition;
    private Vector3 initialPiernaIzqPosition;
    private Vector3 initialPieDerPosition;
    private Vector3 initialPieIzqPosition;

    [SerializeField] private SpriteRenderer spriteRendererCuerpo;


    private bool reachedMaxXDer = false;
    private bool reachedMaxXIzq = false;
    private bool movingRightLeg = false;
    private bool movingLeftLeg = false;
    private float tiempoTeclaPresionada = 0.0f;

    void Start()
    {
        // Guardar las posiciones iniciales
        initialPiernaDerPosition = piernaDer.transform.localPosition;
        initialPiernaIzqPosition = piernaIzq.transform.localPosition;
        initialPieDerPosition = pieDer.transform.localPosition;
        initialPieIzqPosition = pieIzq.transform.localPosition;

        // Ocultar los GameObjects al inicio
        SetGameObjectActive(false, true); // Desactivar la pierna y el pie derecho
        SetGameObjectActive(false, false); // Desactivar la pierna y el pie izquierdo

        spriteRendererCuerpo = cuerpo.GetComponent<SpriteRenderer>();

        // Verificar si los SpriteRenderer se encontraron correctamente
        if (spriteRendererCuerpo != null)
        {

            spriteRendererCuerpo.sprite = normal;
        }
        else
        {
            Debug.LogError("a");
        }
    }

    void Update()
    {
        if (!movingLeftLeg && !reachedMaxXDer && Input.GetKeyDown(KeyCode.D))
        {
            movingRightLeg = true;
            spriteRendererCuerpo.sprite = pose;
            spriteRendererCuerpo.flipX = false;
            SetGameObjectActive(true, true); // Mostrar pierna y pie derecho

        }

        if (movingRightLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            if (piernaDer.transform.localPosition.x < 2.0f)
            {
                piernaDer.transform.localPosition += new Vector3(legPos, 0, 0);
                piernaDer.transform.localScale += new Vector3(legScale, 0, 0);
                pieDer.transform.localPosition += new Vector3(footPos, 0, 0);
            }
            else
            {
                reachedMaxXDer = true;
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

            // Ocultar pierna y pie derecho cuando dejes de presionar la tecla
            SetGameObjectActive(false, true);
            spriteRendererCuerpo.sprite = normal;


        }

        if (!movingRightLeg && !reachedMaxXIzq && Input.GetKeyDown(KeyCode.A))
        {
            movingLeftLeg = true;
            spriteRendererCuerpo.sprite = pose;
            spriteRendererCuerpo.flipX = true;
            SetGameObjectActive(true, false); // Mostrar pierna y pie izquierdo

        }

        if (movingLeftLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            if (piernaIzq.transform.localPosition.x > -2.0f)
            {
                piernaIzq.transform.localPosition -= new Vector3(legPos, 0, 0);
                piernaIzq.transform.localScale += new Vector3(legScale, 0, 0);
                pieIzq.transform.localPosition -= new Vector3(footPos, 0, 0);
            }
            else
            {
                reachedMaxXIzq = true;
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

            // Ocultar pierna y pie izquierdo cuando dejes de presionar la tecla
            SetGameObjectActive(false, false);
            spriteRendererCuerpo.sprite = normal;

        }

        if (movingRightLeg && Input.GetKey(KeyCode.D))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
                spriteRendererCuerpo.sprite = normal;
            }
        }
        if (movingLeftLeg && Input.GetKey(KeyCode.A))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
                spriteRendererCuerpo.sprite = normal;
            }
        }
        if (movingLeftLeg && movingRightLeg)
        {
            spriteRendererCuerpo.sprite = pose;
        }
    }

    void SetGameObjectActive(bool active, bool rightSide)
    {
        if (rightSide)
        {
            Debug.Log("Activando/desactivando lado derecho: " + active);
            piernaDer.SetActive(active);
            pieDer.SetActive(active);
        }
        else
        {
            Debug.Log("Activando/desactivando lado izquierdo: " + active);
            piernaIzq.SetActive(active);
            pieIzq.SetActive(active);
        }
    }

}