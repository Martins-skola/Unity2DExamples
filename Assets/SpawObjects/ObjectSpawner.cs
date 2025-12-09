using UnityEngine;

/*
 * Spawnar en in en prefab som gameobject i scenen vid musklick
 * Väljer slumpmässigt mellan tre olika prefabs att spawna
 * Väljer spawn-position baserat på musens position i världen
*/

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn1;
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;

 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        // Skapa en spawn-position från musens position i världen
        // Konvertera musens skärmposition till världskoordinater (från kamera-pixlar till världskoordinatssystem)
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Sätt z-avståndet från kameran
        spawnPosition.z = 0; 

        // Väljer slumpmässigt en av de tre prefabs att spawna
        int randomPrefabIndex = Random.Range(1, 4);
        if (randomPrefabIndex == 1)
        {

            SpawnPrefab(prefabToSpawn1, spawnPosition); 
        }
        else if (randomPrefabIndex == 2)
        {
            // Spawna prefab 2
            SpawnPrefab(prefabToSpawn2, spawnPosition);
        }
        else
        {
            // Spawna prefab 3
            SpawnPrefab(prefabToSpawn3, spawnPosition);
        }
    }

    void SpawnPrefab(GameObject prefab, Vector3 position)
    {
        // Spawna prefab 1 - Instantiate skapar gameobject i scenen av prefabs
        // I detta fall är det viktigt att spara en referens till det skapade objektet då vi vill lägga till komponenter på det
        GameObject instance = Instantiate(prefab, position, Quaternion.identity); // Quaternion.identity betyder ingen rotation

        // Sätter på en RigidBody2-komponent (fysik-komponent) som gör att gameobjektet faller ner av gravitation
        instance.AddComponent<Rigidbody2D>();

        // Sätter på en custom-komponent som förstör objektet när det inte längre är synligt
        instance.AddComponent<ObjectDestroyer>();
    }
}
