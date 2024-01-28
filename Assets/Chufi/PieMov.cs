using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieMov : MonoBehaviour
{
    [SerializeField] static GameObject piernaDer;
    [SerializeField] static GameObject piernaIzq;
    [SerializeField] static GameObject pieDer;
    [SerializeField] static GameObject pieIzq;
    [SerializeField] GameObject cuerpo;

    [SerializeField] static float legScale = 1f;
    [SerializeField] static float legPos = 0.5f;
    [SerializeField] static float footPos = 1f;
    [SerializeField] float tiempoMaximoTeclaPresionada = 3.0f;


    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite pose;
    [SerializeField] private Sprite perder;

    private Vector3 initialPiernaDerPosition;
    private Vector3 initialPiernaIzqPosition;
    private Vector3 initialPieDerPosition;
    private Vector3 initialPieIzqPosition;

    private static bool canMove = true;

    public static bool wall = false;

    [SerializeField] private SpriteRenderer spriteRendererCuerpo;

    private bool reachedMaxXDer = false;
    private bool reachedMaxXIzq = false;
    private static bool movingRightLeg = false;
    private static bool movingLeftLeg = false;
    private float tiempoTeclaPresionada = 0.0f;

    public Transform posCuerpo;
    public Transform posPieIzq;
    public Transform posPieDer;

    void Start()
    {
        piernaDer = GameObject.Find("Player/sprite_cuerpo/pata/sprite_piernaDer");
        piernaIzq = GameObject.Find("Player/sprite_cuerpo/pata/sprite_piernaIzq");
        pieDer = GameObject.Find("Player/sprite_cuerpo/pata/pieDer");
        pieIzq = GameObject.Find("Player/sprite_cuerpo/pata/pieIzq");

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
            Debug.LogError("El SpriteRenderer del cuerpo no se encontró.");
        }
    }

    void Update()
    {
        if (!canMove)
            return;


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
                piernaDer.transform.localPosition += new Vector3(legPos * Time.deltaTime, 0, 0);
                piernaDer.transform.localScale += new Vector3(legScale * Time.deltaTime, 0, 0);
                pieDer.transform.localPosition += new Vector3(footPos * Time.deltaTime, 0, 0);
            }
            else
            {
                reachedMaxXDer = true;
            }
        }

        if (movingRightLeg && Input.GetKeyUp(KeyCode.D))
        {
            StartCoroutine(StopDer());

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
                piernaIzq.transform.localPosition -= new Vector3(legPos * Time.deltaTime, 0, 0);
                piernaIzq.transform.localScale += new Vector3(legScale * Time.deltaTime, 0, 0);
                pieIzq.transform.localPosition -= new Vector3(footPos * Time.deltaTime, 0, 0);
            }
            else
            {
                reachedMaxXIzq = true;
            }
        }

        if (movingLeftLeg && Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine(StopIzq());

        }

        if (movingRightLeg && Input.GetKey(KeyCode.D))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
            }
        }

        if (movingLeftLeg && Input.GetKey(KeyCode.A))
        {
            if (tiempoTeclaPresionada > tiempoMaximoTeclaPresionada)
            {
                Debug.Log("¡Has perdido!");
            }
        }

        if (movingLeftLeg && movingRightLeg)
        {
            spriteRendererCuerpo.sprite = pose;
        }

        if (wall == true)
        {
            legScale = 0f;
            legPos = 0f;
            footPos = 0f;
            StartCoroutine(StopDer());
            StartCoroutine(StopIzq());
            stopMove();
        }
    }

    public static void SetGameObjectActive(bool active, bool rightSide)
    {
        if (rightSide)
        {
            piernaDer.SetActive(active);
            pieDer.SetActive(active);
        }
        else
        {
            piernaIzq.SetActive(active);
            pieIzq.SetActive(active);
        }
    }
    IEnumerator StopDer()
    {
        posCuerpo.position = new Vector3((posPieDer.position.x + 0.75f), posCuerpo.localPosition.y, posCuerpo.localPosition.z);
        tiempoTeclaPresionada = 0.0f;
        movingRightLeg = false;
        piernaDer.transform.localPosition = initialPiernaDerPosition;
        piernaDer.transform.localScale = new Vector3(0, 1, 1);
        pieDer.transform.localPosition = initialPieDerPosition;
        reachedMaxXDer = false;

        // Ocultar pierna y pie derecho cuando dejes de presionar la tecla
        resumeMove();

        SetGameObjectActive(false, true);
        spriteRendererCuerpo.sprite = normal;
        return null;
    }
    IEnumerator StopIzq()
    {
        posCuerpo.position = new Vector3((posPieIzq.position.x + -0.75f), posCuerpo.localPosition.y, posCuerpo.localPosition.z);
        tiempoTeclaPresionada = 0.0f;
        movingLeftLeg = false;
        piernaIzq.transform.localPosition = initialPiernaIzqPosition;
        piernaIzq.transform.localScale = new Vector3(0, 1, 1);
        pieIzq.transform.localPosition = initialPieIzqPosition;
        reachedMaxXIzq = false;

        // Ocultar pierna y pie izquierdo cuando dejes de presionar la tecla
        resumeMove();
        SetGameObjectActive(false, false);
        spriteRendererCuerpo.sprite = normal;
        return null;
    }

    public static void stopMove()
    {
        Debug.Log("Stop");
        canMove = false; // Detiene el movimiento


    }
    public static void resumeMove()
    {
        Debug.Log("Resume Move");
        canMove = true;
        legScale = 1f;
        legPos = 0.5f;
        footPos = 1f;

    }

}