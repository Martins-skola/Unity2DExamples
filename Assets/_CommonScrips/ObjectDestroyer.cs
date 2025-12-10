using UnityEngine;

/*
 * En komponent som förstör (destroy) gameobjectet på några valbara sätt:
 * - När det inte längre är synligt av någon kamera (OnBecameInvisible)
 * - Efter en viss tid (AfterTime)
 * - Efter en period av inaktivitet (Inactivity)
*/

public class ObjectDestroyer : MonoBehaviour
{
    public enum DestroyCondition
    {
        NotVisibleInScene,
        AfterTime,
        Inactivity
    }

    public DestroyCondition destroyCondition = DestroyCondition.NotVisibleInScene;

    [Header("Efter tid (både AfterTime och Inactivity")]
    public float afterSeconds = 5f; // Tid i sekunder innan objektet förstörs (om AfterTime eller Inactivity används)

    [Header("Inaktivitet")]
    [Tooltip("Förflyttning under detta värde anses som inaktivitet")]
    public float inactivityTime = 3f;
    public float movementThreshold = 0.01f;

    [Header("Don't destroy - Sänd bara meddelanade")]
    [Tooltip("Istället för destroy skickas anropas 'destroyObject(gameObject)' på komponenter")]
    public bool dontDestroyJustNotify = false; // Om true, anropar vi bara 'destroyObject' istället för att förstöra objektet

    private Vector3 lastPosition;
    private float inactivityTimer = 0f;

    private void Start()
    {
        if (destroyCondition == DestroyCondition.AfterTime)
        {
            Destroy(gameObject, afterSeconds);
        }
        else if (destroyCondition == DestroyCondition.Inactivity)
        {
            lastPosition = transform.position;
        }
    }

    private void Update()
    {
        if (destroyCondition == DestroyCondition.Inactivity)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if (distanceMoved > movementThreshold)
            {
                // Objektet rör sig - nollställ timer
                inactivityTimer = 0f;
                lastPosition = transform.position;
            }
            else
            {
                // Objektet är inaktivt - öka timer
                inactivityTimer += Time.deltaTime;

                if (inactivityTimer >= inactivityTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }


    // OnBecameInvisible anropas av Unity när gameobjectets render-komponent inte längre är synlig av någon kamera
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
