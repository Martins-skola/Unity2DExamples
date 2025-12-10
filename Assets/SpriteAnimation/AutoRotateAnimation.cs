using System.Runtime.CompilerServices;
using UnityEngine;


public class AutoRotateAnimation : MonoBehaviour
{
    public float speed = 120f; // Rotationshastighet i grader per sekund (publik, justerbar i inspektorn)

    void Update()
    {

        // Rotera objektet runt Z-axeln (i 2D är Z-axeln "framåt"/"bakåt" mot kameran)
        // Vector3.forward = en konstant för vektor (0, 0, 1) som ger riktningen "framåt", z = 1
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
