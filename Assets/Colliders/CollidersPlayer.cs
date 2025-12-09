using UnityEngine;

public class CollidersPlayer : MonoBehaviour
{
    public CollidersGame game;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Krock med fiende! Återställer spelet.");
            game.ResetGame();
        }
    }
}
