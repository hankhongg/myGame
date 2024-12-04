using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    Vector2 startPos;
    float startZ;
    // => means calculate on every frame
    Vector2 camMovedSinceStart => (Vector2)cam.transform.position - startPos;
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget / clippingPlane);

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    float clippingPlane => (cam.transform.position.z + distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane);
    
    float distanceFromSubject = 0;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPos + camMovedSinceStart * parallaxFactor;

        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
