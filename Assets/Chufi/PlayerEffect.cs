using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] public Material flashMaterial;

    [SerializeField] private float duration;

    [SerializeField] float rotationSpeed;

    [SerializeField] float rotationSpeedPlayer;

    private Vector3 originalPosition;

    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    public Coroutine flashRoutine;

    private float currentRotation = 0f;
    private bool isRotating;

    public Vector3 originalScale;
    [SerializeField] private float pulseSpeed = 1.0f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;

    public PieMov pieMov;

    public GameObject pata;

    public Jumo jump;

    public GameObject jumpObj;

    public static GameObject particleHit;

    public static GameObject particleDeadEnemy;

    public bool isPulsating;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        originalScale = transform.localScale;
        originalPosition = transform.position;
        pieMov = pata.GetComponent<PieMov>();
        jump = jumpObj.GetComponent<Jumo>();
        isPulsating = false;
        isRotating = false;
        particleHit = GameObject.Find("hit_particle");
        particleHit.SetActive(false);
        particleDeadEnemy = GameObject.Find("ENEMYdeath_particles");
        particleDeadEnemy.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            pieMov.tiempoTeclaPresionada = 0;
            pieMov.spriteRendererCuerpo.sprite = pieMov.pose;
            pieMov.spriteRendererCuerpo.flipX = false;
        }
        if (PieMov.movingRightLeg && Input.GetKey(KeyCode.D))
        {

            if (pieMov.tiempoTeclaPresionada > pieMov.tiempoMaximoTeclaPresionada)
            {
                Flash(Color.magenta);
                currentRotation = 0f;
                isRotating = true;
                StartPulse();
                pieMov.spritePieDer.SetActive(false);
                PieMov.piernaDer.SetActive(false); // Mostrar pierna y pie derecho
                Debug.Log("¡Has perdido!");

            }


        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            pieMov.tiempoTeclaPresionada = 0;
            pieMov.spriteRendererCuerpo.sprite = pieMov.pose;
            pieMov.spriteRendererCuerpo.flipX = true;
        }

        if (PieMov.movingLeftLeg && Input.GetKey(KeyCode.A))
        {


            if (pieMov.tiempoTeclaPresionada > pieMov.tiempoMaximoTeclaPresionada)
            {
                Flash(Color.magenta);
                currentRotation = 0f;
                isRotating = true;
                StartPulse();
                pieMov.spritePieIzq.SetActive(false);
                PieMov.piernaIzq.SetActive(false);
                Debug.Log("¡Has perdido!");
            }

        }
        if (jump.onGround && PieMov.movingLeftLeg && Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine(MoveFinalPlayerScale());
            Invoke("StopCoroutine(MoveFinalPlayerScale())", 5f);
        }
        if (!jump.onGround && Input.GetKeyUp(KeyCode.A))
        {
            jump.spriteRenderer.sprite = jump.caida;

            pieMov.spritePieIzq.SetActive(false);
        }




        if (jump.onGround && PieMov.movingRightLeg && Input.GetKeyUp(KeyCode.D))
        {
            StartCoroutine(MoveFinalPlayerScale());
            Invoke("StopCoroutine(MoveFinalPlayerScale())", 5f);
        }
        if (!jump.onGround && Input.GetKeyUp(KeyCode.D))
        {
            jump.spriteRenderer.sprite = jump.caida;
            pieMov.spritePieDer.SetActive(false);

        }



        if (isRotating)
        {
            // Rotar en el eje Z constantemente hasta llegar a 360 grados.
            currentRotation += rotationSpeedPlayer * Time.deltaTime;
            if (currentRotation >= 360f)
            {
                // Detener la rotación y la escala una vez que alcance 360 grados.
                isRotating = false;
                currentRotation = 0f; // Reiniciar la rotación a 0 para la próxima vez.
                StopPulse();
            }
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        }
        if (isPulsating)
        {
            // Calcular la escala actual del palpito mediante una interpolación sinusoidal
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * pulseSpeed, 1f));

            // Aplicar la escala al objeto en ambos ejes
            transform.localScale = originalScale * scale;

        }
    }



    private void StartPulse()
    {
        // Iniciar el efecto de palpito
        isPulsating = true;
        pieMov.spriteRendererCuerpo.sprite = pieMov.normal;

    }

    private void StopPulse()
    {
        // Detener el efecto de palpito
        isPulsating = false;

        // Reiniciar la escala gradualmente a la escala original
        StartCoroutine(ScaleToOriginal());
        pieMov.spriteRendererCuerpo.sprite = pieMov.normal;
    }

    private IEnumerator ScaleToOriginal()
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // Duración de la transición de escala

        // Escalar gradualmente el objeto a su escala original
        while (elapsedTime < duration)
        {
            // Calcular el valor de escala entre el valor actual y la escala original
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, t);

            // Actualizar el tiempo transcurrido
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la escala sea exactamente la escala original al finalizar la transición
        transform.localScale = originalScale;
    }

    public void Flash(Color color)
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        spriteRenderer.material = flashMaterial;
        flashMaterial.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = flashMaterial;
        flashMaterial.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }
    public IEnumerator MoveFinalPlayerScale()
    {

        Vector3 newScale = new Vector3(0.5f, 1f, 1f);
        transform.localScale = Vector3.Scale(originalScale, newScale);
        yield return new WaitForSeconds(0.5f);

        transform.localScale = originalScale;
    }



}
