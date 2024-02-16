using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxEffect : MonoBehaviour
{
    Transform camTransform;
    private Vector3 lastCamPos;
    Vector3 camStartPos;
    public float speed;
    private Vector2 offset;
    private Material mat;
    private float startPos;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
        startPos = transform.position.x;
        mat = GetComponent<SpriteRenderer>().material;
    }
    private void LateUpdate()
    {

        float deltaX = (camTransform.position.x - lastCamPos.x) * speed;
        offset = new Vector2(deltaX, 0);
        mat.mainTextureOffset = offset;
    }
}
