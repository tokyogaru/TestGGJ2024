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
    public GameObject spritePieDer;
    public GameObject spritePieIzq;


    [SerializeField] static float legScaleDer = 1f;
    [SerializeField] static float legPosDer = 0.5f;
    [SerializeField] static float footPosDer = 1f;
    [SerializeField] static float legScaleIzq = 1f;
    [SerializeField] static float legPosIzq = 0.5f;
    [SerializeField] static float footPosIzq = 1f;
    [SerializeField] float tiempoMaximoTeclaPresionada = 3.0f;


    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite pose;
    [SerializeField] private Sprite perder;

    private Vector3 initialPiernaDerPosition;
    private Vector3 initialPiernaIzqPosition;
    private Vector3 initialPieDerPosition;
    private Vector3 initialPieIzqPosition;

    private bool canMoveDer = true;
    private bool canMoveIzq = true;


    public static bool walledDer = false;
    public static bool walledIzq = false;


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
        spritePieDer.SetActive(false);
        spritePieIzq.SetActive(false);
        piernaDer.SetActive(false);
        piernaIzq.SetActive(false);


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
      

        if (!movingLeftLeg && !reachedMaxXDer && walledDer == false && Input.GetKeyDown(KeyCode.D))
        {
            movingRightLeg = true;
            spriteRendererCuerpo.sprite = pose;
            spriteRendererCuerpo.flipX = false;
            spritePieDer.SetActive(true);
            piernaDer.SetActive(true); // Mostrar pierna y pie derecho
        }

        if (movingRightLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            if (piernaDer.transform.localPosition.x < 2.0f)
            {
                piernaDer.transform.localPosition += new Vector3(legPosDer * Time.deltaTime, 0, 0);
                piernaDer.transform.localScale += new Vector3(legScaleDer * Time.deltaTime, 0, 0);
                pieDer.transform.localPosition += new Vector3(footPosDer * Time.deltaTime, 0, 0);
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

        if (!movingRightLeg && !reachedMaxXIzq && walledIzq == false && Input.GetKeyDown(KeyCode.A))
        {
            movingLeftLeg = true;
            spriteRendererCuerpo.sprite = pose;
            spriteRendererCuerpo.flipX = true;
            spritePieIzq.SetActive(true);
            piernaIzq.SetActive(true); // Mostrar pierna y pie izquierdo
        }

        if (movingLeftLeg)
        {
            tiempoTeclaPresionada += Time.deltaTime;

            if (piernaIzq.transform.localPosition.x > -2.0f)
            {
                piernaIzq.transform.localPosition -= new Vector3(legPosIzq * Time.deltaTime, 0, 0);
                piernaIzq.transform.localScale += new Vector3(legScaleIzq * Time.deltaTime, 0, 0);
                pieIzq.transform.localPosition -= new Vector3(footPosIzq * Time.deltaTime, 0, 0);
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
        //Detener movimiento
        if (walledDer == true)
        {
            stopMoveDer();
            //StartCoroutine(StopDer());

        }
        if (walledIzq == true)
        {
            stopMoveIzq();
            //StartCoroutine(StopIzq());
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
        yield return new WaitForSeconds(0.1f);
        posCuerpo.position = new Vector3((posPieDer.position.x + 0.75f), posCuerpo.localPosition.y, posCuerpo.localPosition.z);
        tiempoTeclaPresionada = 0.0f;
        movingRightLeg = false;
        piernaDer.transform.localPosition = initialPiernaDerPosition;
        piernaDer.transform.localScale = new Vector3(0, 1, 1);
        pieDer.transform.localPosition = initialPieDerPosition;
        reachedMaxXDer = false;

        // Ocultar pierna y pie derecho cuando dejes de presionar la tecla
        resumeMove();

        spritePieDer.SetActive(false);
        piernaDer.SetActive(false);
        spriteRendererCuerpo.sprite = normal;
    }
    IEnumerator StopIzq()
    {
        yield return new WaitForSeconds(0.1f);
        posCuerpo.position = new Vector3((posPieIzq.position.x + -0.75f), posCuerpo.localPosition.y, posCuerpo.localPosition.z);
        tiempoTeclaPresionada = 0.0f;
        movingLeftLeg = false;
        piernaIzq.transform.localPosition = initialPiernaIzqPosition;
        piernaIzq.transform.localScale = new Vector3(0, 1, 1);
        pieIzq.transform.localPosition = initialPieIzqPosition;
        reachedMaxXIzq = false;

        // Ocultar pierna y pie izquierdo cuando dejes de presionar la tecla
        resumeMove();
        spritePieIzq.SetActive(false);
        piernaIzq.SetActive(false);
        spriteRendererCuerpo.sprite = normal;
    }

    public void stopMoveDer()
    {
        Debug.Log("StopDer");
        legScaleDer = 0f;
        legPosDer = 0f;
        footPosDer = 0f;
        canMoveDer = false; // Detiene el movimiento
    }

    public void stopMoveIzq()
    {
        Debug.Log("StopIzq");
        legScaleIzq = 0f;
        legPosIzq = 0f;
        footPosIzq = 0f;
        canMoveIzq = false; // Detiene el movimiento
    }
    public void resumeMove()
    {
        Debug.Log("Resume Move");
        canMoveDer = true;
        canMoveIzq = true;
        legScaleDer = 1f;
        legPosDer = 0.5f;
        footPosDer = 1f;
        legScaleIzq = 1f;
        legPosIzq = 0.5f;
        footPosIzq = 1f;
    }

}