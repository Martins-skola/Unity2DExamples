using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public CircleSpawner spawner;

    void Start()
    {
        if (spawner == null)
        {
            Debug.LogError("Reference till CircleSpawner-komponent saknas på detta gameobject!");
            this.enabled = false; // Inaktivera skriptet om CircleSpawner saknas
        }

        // Stänger av sprite-renderern så att triggern inte syns (det är dock bra att den syns i Editorn under utveckling)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.SpawnCircle();
        }
    }
}
