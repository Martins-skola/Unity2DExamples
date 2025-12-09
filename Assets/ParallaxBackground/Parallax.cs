using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxSpeedHorizontal = 0.5f;
    [SerializeField] private float parallaxSpeedVertical = 0.5f;
    // [SerializeField] private bool infiniteHorizontal = true;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth;
    private float startPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        // Beräkna sprite-bredd för seamless looping
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWidth = spriteRenderer.bounds.size.x;
            startPosition = transform.position.x;
        }
    }

    void LateUpdate()
    {
        // Beräkna parallax-rörelse
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxSpeedHorizontal;
        float deltaY = (cameraTransform.position.y - previousCameraPosition.y) * parallaxSpeedVertical;

        transform.position += new Vector3(deltaX, deltaY, 0);

        // Oändlig scrollning (loopa bakgrunden)
        /*
        if (infiniteHorizontal && spriteWidth > 0)
        {
            float distanceFromStart = transform.position.x - startPosition;

            if (distanceFromStart > spriteWidth)
            {
                transform.position = new Vector3(
                    transform.position.x - spriteWidth,
                    transform.position.y,
                    transform.position.z
                );
                startPosition = transform.position.x;
            }
            else if (distanceFromStart < -spriteWidth)
            {
                transform.position = new Vector3(
                    transform.position.x + spriteWidth,
                    transform.position.y,
                    transform.position.z
                );
                startPosition = transform.position.x;
            }
        }
        */

        previousCameraPosition = cameraTransform.position;
    }
}