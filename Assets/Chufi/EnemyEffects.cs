using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField] public Material flashMaterial;

    [SerializeField] private float duration;
    public SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    public Coroutine flashRoutine;
    [SerializeField] float rotationSpeed;
    private Vector3 originalPosition;

    public Sprite dead;

    public GameObject particleDead;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        originalScale = transform.localScale;
        originalPosition = transform.position;
        particleDead = GameObject.Find("ENEMYdeath_particles");
        particleDead.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator EnemysDead()
    {
        Flash(Color.magenta);
        Vector3 newScale = new Vector3(1f, 0.5f, 1f); // Define la nueva escala (50% de la escala original en el eje Y)
        transform.localScale = Vector3.Scale(originalScale, newScale);
        particleDead.SetActive(true);
        spriteRenderer.sprite = dead;
        yield return null;
    }
    public void MoveChar()
    {
        float rotation = Mathf.Sin(Time.time * rotationSpeed) * 1f - 0.5f;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation * 5f);

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

    public IEnumerator RedFadeAndScale()
    {
        float elapsedTime = 0f;
        float duration = 1f; // Duración total de la transición (5 segundos)
        float moveY = -0.5f;

        // Guardar el color original y la escala original
        Color originalColor = spriteRenderer.color;
        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.position;

        // Cambiar gradualmente el color a rojo
        while (elapsedTime < duration)
        {
            // Calcular el factor de progreso de la transición
            float t = elapsedTime / duration;

            // Calcular el nuevo color interpolando entre el color original y el rojo
            Color newColor = Color.Lerp(originalColor, Color.red, t);

            // Aplicar el nuevo color al objeto
            spriteRenderer.color = newColor;

            // Escalar gradualmente en el eje Y
            transform.localScale = Vector3.Lerp(originalScale, new Vector3(originalScale.x, 0.5f, originalScale.z), t);
            transform.localPosition -= Vector3.down * moveY * Time.deltaTime;

            // Mover el objeto de izquierda a derecha rápidamente
            float moveX = Mathf.PingPong(Time.time * 5f, 1f) - 0.5f; // Rango de movimiento de -1 a 1
            transform.position += Vector3.right * moveX * Time.deltaTime * 5f; // Movimiento rápido de izquierda a derecha

            // Actualizar el tiempo transcurrido
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Al terminar la transición, restaurar el color, la escala y la posición originales
        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;
        transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);

    }
    public IEnumerator RechargerPeo()
    {
        float elapsedTime = 0f;
        float duration = 1f; // Duración total de la transición (5 segundos)
      

        // Guardar el color original y la escala original
        Color originalColor = spriteRenderer.color;
        //Vector3 originalScale = transform.localScale;
        

        // Cambiar gradualmente el color a rojo
        while (elapsedTime < duration)
        {
            // Calcular el factor de progreso de la transición
            float t = elapsedTime / duration;

            // Calcular el nuevo color interpolando entre el color original y el rojo
            Color newColor = Color.Lerp(originalColor, Color.red, t);

            // Aplicar el nuevo color al objeto
            spriteRenderer.color = newColor;

            // Actualizar el tiempo transcurrido
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Al terminar la transición, restaurar el color, la escala y la posición originales
        spriteRenderer.color = originalColor;
        //transform.localScale = originalScale;
    }
}
