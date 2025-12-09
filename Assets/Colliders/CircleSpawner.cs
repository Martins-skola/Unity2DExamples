using UnityEngine;

/*
 * Spawna en cirkel-prefab som gameobject i scenen
 * Spawnar cirkeln på positionen för det gameobjekt som denna komponent ligger på
 * Spawnar bara på uppmaning (när metoden SpawnCircle kallas)
 */
public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public float circleScale = 1f;

    void Start()
    {
        if (circlePrefab == null)
        {
            Debug.LogError("Ingen cirkel-prefab har angivits!");
            this.enabled = false; // Inaktivera skriptet om prefab saknas
        }
    }

    public void SpawnCircle()
    {
        // Instansiera prebafen till ett gameobject i scenen och sparar en temporärt referens till den, då vi behöver sätta tag, ändra skala och lägga till komponenter
        GameObject instance = Instantiate(circlePrefab, transform.position, Quaternion.identity);

        // Sätter tag på den instansierade cirkeln
        instance.tag = "Enemy";

        // Sätter skalan på den instansierade cirkeln
        instance.transform.localScale = new Vector3(circleScale, circleScale, 1f);

        // Lägger till en Rigidbody2D-komponent för fysik, om den inte redan finns på prefab:en (räknar med att prefab:en har en 2D circle collider))
        if (instance.GetComponent<Rigidbody2D>() == null)
        {
            instance.AddComponent<Rigidbody2D>();
        }
    }
}
