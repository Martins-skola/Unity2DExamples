using System.Runtime.CompilerServices;
using UnityEngine;


public class CosinusWobbleAnimation : MonoBehaviour
{
    // Amplituden (hur långt upp och ner objektet rör sig)
    public float amplitude = 0.3f;
    // Hastigheten på rörelsen
    public float frequency = 5f;

    // Ursprunglig position, beräkningen av wobblen baseras på denna position. Kan vara privat eftersom den inte behöver justeras i inspektorn
    private Vector3 startPosition;

    public enum RotationAxis // Enum (en datatyps-lista) för axlarna. Behöver vara publik för att inspektorn ska kunna använda datatypen för att visa gränssnitt
    {
        X,
        Y
    }

    // Publik, Välj rotationaxel i inspektorn (Y förvalt).
    // Dettta är objektets lokala axel, så beroende på objektets ursprungliga rotation i scenen kan rotationsaxel vara svår att förutse, man får prova
    public RotationAxis axis = RotationAxis.X;

    void Start()
    {
        // Spara objektets startposition som beräkningsgrund för wobblen
        startPosition = transform.position;
    }

    void Update()
    {
        // Wobbla den valda axeln med den angivna frekvensen och amplituden och baserat på startpositionen
        // Nedan ger ett Y (the wobble) längs en cosinus-kurva baserat på tid (aktuell frame), frekvens och amplitud
        // Vi använder sedan detta Y (the wobble) på en vald axel
        float wobble = Mathf.Cos(Time.time * frequency) * amplitude;

        // Skapa en förflyttning-vektor för the wobble applicerad på vald axel
        // Värdet multipliceras sedan med hastigheten för att få rätt rotationshastighet
        Vector3 wobbleVector = new Vector3(); // get vektorn (0,0,0) som standard

        if (axis == RotationAxis.X)
        {
            wobbleVector = new Vector3(wobble, 0, 0);
        }
        else 
        {
            wobbleVector = new Vector3(0, wobble, 0);
        }

        // Ny position = startPosition + wobble
        transform.position = startPosition + wobbleVector;
    }
}