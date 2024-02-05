using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestEffect : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;

    [SerializeField] private float duration;

    [SerializeField] float rotationSpeed;

    private Vector3 originalPosition;
    [SerializeField] private Vector3 pointA;
    [SerializeField] private Vector3 pointB;
    [SerializeField] float rotationSpeedPlayer;

    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private float currentRotation = 0f;
    private bool isRotating;

    private Vector3 originalScale;

    [SerializeField] private float pulseSpeed = 1.0f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;

    public bool isPulsating;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        originalScale = transform.localScale;
        originalPosition = transform.position;
        pointA = originalPosition + Vector3.up * 0.08f; 
        pointB = originalPosition + Vector3.up * -0.08f;
    }

    private void Update()
    {
        // Verificar si se presiona una tecla, por ejemplo, la tecla "Espacio".
        if (Input.GetKeyDown(KeyCode.Space))///morirenemigos
        {
            //daño
            Flash(Color.magenta);
            Vector3 newScale = new Vector3(1f, 0.5f, 1f); // Define la nueva escala (50% de la escala original en el eje Y)
            transform.localScale = Vector3.Scale(originalScale, newScale); // Aplica la nueva escala al objeto
        }

        if (Input.GetKey(KeyCode.Z)) //Movimiento enemigos
        {
            // Rotar en el eje Z constantemente dentro del rango de -1 a 1.
            float rotation = Mathf.Sin(Time.time * rotationSpeed) * 1f - 0.5f;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation * 5f);
            float moveY = Mathf.PingPong(Time.time * moveSpeed, 1f);
            transform.position = Vector3.Lerp(pointA, pointB, moveY);

        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.L))//usado
        {
            //daño jugador
            Flash(Color.magenta);
            currentRotation = 0f;
            isRotating = true;
            StartPulse();
        }

        // Verificar si la rotación está en curso.
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(MoveFinalPlayerScale());
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            StopCoroutine(MoveFinalPlayerScale());
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(RedFadeAndScale());
        }
        if(Input.GetKeyUp(KeyCode.J))
        {
            StopCoroutine(RedFadeAndScale());
        }


    }
    private void StartPulse()
    {
        // Iniciar el efecto de palpito
        isPulsating = true;
    }

    private void StopPulse()
    {
        // Detener el efecto de palpito
        isPulsating = false;

        // Reiniciar la escala gradualmente a la escala original
        StartCoroutine(ScaleToOriginal());
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


    private IEnumerator MoveFinalPlayerScale()
    {

        Vector3 newScale = new Vector3(0.5f, 1f, 1f);
        transform.localScale = Vector3.Scale(originalScale, newScale);

        // Mueve el objeto de izquierda a derecha tres veces
        for (int i = 0; i < 1; i++)
        {
            // Mueve el objeto hacia la derecha
            float elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                // Calcula la nueva posición del objeto interpolando entre su posición actual y la posición hacia la derecha
                float t = elapsedTime / 0.5f;
                transform.position += Vector3.right * Time.deltaTime * 1f; // Mueve el objeto 3 unidades por segundo hacia la derecha
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Espera un corto período de tiempo antes de cambiar de dirección
            yield return new WaitForSeconds(0.01f);

            // Mueve el objeto hacia la izquierda
            elapsedTime = 0f;
            while (elapsedTime < 0.5f)
            {
                // Calcula la nueva posición del objeto interpolando entre su posición actual y la posición hacia la izquierda
                float t = elapsedTime / 0.5f;
                transform.position -= Vector3.right * Time.deltaTime * 1f; // Mueve el objeto 3 unidades por segundo hacia la izquierda
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Espera un corto período de tiempo antes de cambiar de dirección
            yield return new WaitForSeconds(0.01f);
        }

        // Devuelve el objeto a su posición y escala originales
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localScale = originalScale;
    }

    private IEnumerator RedFadeAndScale()
    {
        float elapsedTime = 0f;
        float duration = 5f; // Duración total de la transición (5 segundos)

        // Guardar el color original y la escala original
        Color originalColor = spriteRenderer.color;
        Vector3 originalScale = transform.localScale;

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
        transform.position = originalPosition;
    }
}

