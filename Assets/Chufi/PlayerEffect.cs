using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] public Material flashColor;

    [SerializeField] private float duration;

    [SerializeField] float rotationSpeed;

    [SerializeField] float rotationSpeedPlayer;

    private Vector3 originalPosition;

    public SpriteRenderer spriteRenderer;

    public SpriteRenderer spritePata;

    public SpriteRenderer spritePata2;
    public SpriteRenderer spritePierna;
    public SpriteRenderer spritePierna2;

    public Material pata1Mat;
    public Material pata2Mat;
    public Material pierna1Mat;
    public Material pierna2Mat;
    public Material originalColor;

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

    public static GameObject particleHit;

    public static GameObject particleDeadEnemy;

    public bool isPulsating;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.material;
        originalScale = transform.localScale;
        originalPosition = transform.position;
        pieMov = pata.GetComponent<PieMov>();
        isPulsating = false;
        isRotating = false;
        particleHit = GameObject.Find("hit_particle");
        particleHit.SetActive(false);
        particleDeadEnemy = GameObject.Find("ENEMYdeath_particles");
        particleDeadEnemy.SetActive(false);

        spritePierna = GameObject.Find("sprite_piernaDer").GetComponent<SpriteRenderer>();
        pierna1Mat = spritePata.material;
        spritePierna2 = GameObject.Find("sprite_piernaIzq").GetComponent<SpriteRenderer>();
        pierna2Mat = spritePata.material;
        spritePata = GameObject.Find("sprite_pata").GetComponent<SpriteRenderer>();
        pata1Mat = spritePata.material;
        spritePata2 = GameObject.Find("sprite_pata2").GetComponent<SpriteRenderer>();
        pata2Mat = spritePata.material;

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
                Flash(flashColor);

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
                Flash(flashColor);

                Debug.Log("¡Has perdido!");
            }

        }
        if (PieMov.movingLeftLeg && Input.GetKeyUp(KeyCode.A))
        {
            transform.localScale = new Vector3(originalScale.x * -0.5f, transform.localScale.y, transform.localScale.z);
            StartCoroutine(ScaleHorizontally(2.0f, 0.25f));
            if (pieMov.tiempoTeclaPresionada > pieMov.tiempoMaximoTeclaPresionada)
            {
                Flash(flashColor);
                currentRotation = 0f;
                isRotating = true;
                StartPulse();
                StopCoroutine("ScaleHorizontally");
                pieMov.spritePieIzq.SetActive(false);
                PieMov.piernaIzq.SetActive(false);

            }

        }
        if (PieMov.movingRightLeg && Input.GetKeyUp(KeyCode.D))
        {
            transform.localScale = new Vector3(originalScale.x * -0.5f, transform.localScale.y, transform.localScale.z);
            StartCoroutine(ScaleHorizontally(2.0f, 0.25f));
            if (pieMov.tiempoTeclaPresionada > pieMov.tiempoMaximoTeclaPresionada)
            {
                Flash(flashColor);
                currentRotation = 0f;
                isRotating = true;
                StartPulse();
                StopCoroutine("ScaleHorizontally");
                pieMov.spritePieDer.SetActive(false);
                PieMov.piernaDer.SetActive(false);


            }

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
        pieMov.spriteRendererCuerpo.sprite = pieMov.pose;

    }

    private void StopPulse()
    {
        // Detener el efecto de palpito
        isPulsating = false;
        pieMov.spriteRendererCuerpo.sprite = pieMov.normal;
        StartCoroutine(ScaleToOriginal());

    }

    public IEnumerator ScaleToOriginal()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;

        Vector3 targetScale = originalScale;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void Flash(Material material)
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine(material));
    }

    private IEnumerator FlashRoutine(Material material)
    {
        spriteRenderer.material = material;
        spritePata.material = material;
        spritePierna.material = material;
        spritePata2.material = material;
        spritePierna2.material = material;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalColor;
        spritePata.material = pata1Mat;
        spritePierna.material = pierna1Mat;
        spritePata2.material = pata2Mat;
        spritePierna2.material = pierna2Mat;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = material;
        spritePata.material = material;
        spritePierna.material = material;
        spritePata2.material = material;
        spritePierna2.material = material;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalColor;
        spritePata.material = pata1Mat;
        spritePierna.material = pierna1Mat;
        spritePata2.material = pata2Mat;
        spritePierna2.material = pierna2Mat;
        flashRoutine = null;
    }
    public IEnumerator ScaleHorizontally(float scaleFactor, float duration)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(scaleFactor, originalScale.y, originalScale.z);
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, progress);
            yield return null;
        }

        transform.localScale = destinationScale;

        // Luego de escalar, se llama al método para volver a la escala original
        StartCoroutine(ScaleToOriginal());
    }
    


}

