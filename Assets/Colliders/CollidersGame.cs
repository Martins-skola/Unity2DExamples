using UnityEngine;

public class CollidersGame : MonoBehaviour
{
    public CircleSpawner circleSpawner;
    public GameObject player;

    private Vector3 playerStartPosition;
    void Start()
    {
        playerStartPosition = player.transform.position;
    }

    public void ResetGame()
    {
        Debug.Log("Återställer spelet...");

        // Återställ spelarens position
        player.transform.position = playerStartPosition;


        // Radera alla cirklar i scenen
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
    }
}
