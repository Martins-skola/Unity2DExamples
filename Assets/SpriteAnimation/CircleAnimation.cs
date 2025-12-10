using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class CirculateAnimation : MonoBehaviour
{
    public float speed = 1f; // radianer per sekund
    public float radius = 0.75f; // avstånd från centerposition = radien på cirkeln
    public bool faceCenter = false; // om objektet ska roteras så att det alltid tittar mot centerpositionen

    private float currentAngle = 0f; // aktuell vinkel runt cirkeln i grader. Privat egenskap, behöver inte vara publik. Vi slumpar en startvinkel i Start() för att undvika att flera objekt börjar på samma position

    // Centerpositionen som objektet cirkulerar runt, vi sätter denna till objektets aktuella position i Start()
    // Om objektet även behöver röra sig på annat sätt, så får du lägga detta objekt som ett barnobjekt till ett tomt objekt och animera/röra det tomma objektet
    private Vector3 centerPosition;


    void Start()
    {
        centerPosition = transform.localPosition; // sätt centerpositionen till objektets lokala startposition (lokala, för att möjliggöra för objektet att vara ett barnobjekt till ett annat objekt som kan röra sig)
        currentAngle = Random.Range(0f, 2 * Mathf.PI); // Radianer. Slumpar fram en startvinkel för att undvika att flera objekt börjar på samma position
    }

    void Update()
    {
        currentAngle = (currentAngle + speed * Time.deltaTime) % (2 * Mathf.PI); // håll vinkeln inom 0-2PI radianer. % är modulus-operatorn som ger resten vid division

        float xFromCenter = Mathf.Sin(currentAngle) * radius;
        float yFromCenter = Mathf.Cos(currentAngle) * radius;

        // Skapar en ny position på cirkeln runt centrumpunkten baserat på den aktuella vinkeln och radien
        Vector3 positionFromCenter = new Vector3(xFromCenter, yFromCenter, 0); // z = 0 eftersom vi rör oss i 2D-planet och snurrar runt Z-axeln


        // Ny position = centerPosition + positionFromCenter
        // Sätter objektets lokala position för att möjliggöra för objektet att vara ett barnobjekt till ett annat objekt som kan röra sig
        // transform.position sätter global position
        transform.localPosition = centerPosition + positionFromCenter;

        // Rotera objektet så att det tittar mot centerpositionen om faceCenter är true
        if (faceCenter)
        {
            // Beräkna riktningen till målet
            Vector2 direction = centerPosition - transform.position; // vektorn är dX, dY till centerpositionen

            // Beräkna vinkeln i grader med Atan2
            // Trigonometriska funktionen Atan2(dY, dX), Med vanlig Atan vet man en vinkeltangent som pekar åt två håll. Atan2 en vinkel för det "rätta hållet".
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Skapa rotationen - Unity använder Quaternions för rotationer (en matris istället för en vektor)
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // Applicerar titta-mot-centrum-rotationen på objektet
            transform.rotation = targetRotation;
        }
    }
}