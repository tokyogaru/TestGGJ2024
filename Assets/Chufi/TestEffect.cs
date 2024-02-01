using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;

    [SerializeField] private float duration;

    [SerializeField] float rotationSpeed;
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

    public bool isPulsating { get; private set; }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        originalScale = transform.localScale;
    }

    private void Update()
    {
        // Verificar si se presiona una tecla, por ejemplo, la tecla "Espacio".
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Ejecutar el efecto de destello con un color específico, por ejemplo, rojo.
            Flash(Color.magenta);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            // Rotar en el eje Z constantemente dentro del rango de -1 a 1.
            float rotation = Mathf.Sin(Time.time * rotationSpeed) * 2f - 1f;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation * 5f);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
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
}

