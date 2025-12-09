using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    // OnBecameInvisible anropas av Unity när gameobjectets render-komponent inte längre är synlig av någon kamera
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
