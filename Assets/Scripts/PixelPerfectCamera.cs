using UnityEngine;

public class PixelPerfectCameraSnapping : MonoBehaviour
{
    public float pixelsPerUnit = 16f; // Adjust this to match your game's Pixels Per Unit

    void LateUpdate()
    {
        // Calculate the size of one pixel in world units
        float pixelSize = 1f / pixelsPerUnit;

        // Get the camera's current position
        Vector3 cameraPosition = transform.position;

        // Snap the camera position to the nearest pixel
        cameraPosition.x = Mathf.Round(cameraPosition.x / pixelSize) * pixelSize;
        cameraPosition.y = Mathf.Round(cameraPosition.y / pixelSize) * pixelSize;

        // Apply the snapped position
        transform.position = cameraPosition;
    }
}
