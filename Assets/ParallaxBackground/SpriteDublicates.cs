using UnityEngine;

/// Komponenten duplicerar en sprite horisontellt och speglar varje andra sprite
/// för att skapa sömlösa övergångar mellan kopiorna.
public class SeamlessSpriteRepeater : MonoBehaviour
{
    [Header("Inställningar")]
    [Tooltip("Totalt antal sprites som ska skapas (inklusive originalet)")]
    [SerializeField] private int totalCount = 5;

    [Tooltip("Om true, speglas varje andra sprite för sömlösa kanter")]
    [SerializeField] private bool seamless = true;

    [Header("Positionering")]
    [Tooltip("Manuell offset mellan sprites i world units (negativ värde = överlapp, positiv = extra mellanrum)")]
    [SerializeField] private float manualOffset = 0f;

    [Tooltip("Försök automatiskt justera position för pixel-perfect rendering")]
    [SerializeField] private bool pixelPerfect = true;

    [Tooltip("Lägg till en minimal överlapp för att undvika glipor (rekommenderat: 0.01-0.05)")]
    [SerializeField] private float overlapAmount = 0.02f;

    [Header("Information")]
    [SerializeField] private bool showDebugInfo = false;

    private SpriteRenderer originalSpriteRenderer;
    private GameObject[] duplicates;
    private float spriteWidth;

    private void Start()
    {
        CreateDuplicates();
    }

    /// Skapar alla duplicerade sprites och arrangerar dem.
    public void CreateDuplicates()
    {
        // Rensa eventuella tidigare duplicat
        ClearDuplicates();

        // Hämta SpriteRenderer från detta GameObject
        originalSpriteRenderer = GetComponent<SpriteRenderer>();

        if (originalSpriteRenderer == null)
        {
            Debug.LogError("SeamlessSpriteRepeater: Ingen SpriteRenderer hittades på GameObject!", this);
            return;
        }

        if (originalSpriteRenderer.sprite == null)
        {
            Debug.LogError("SeamlessSpriteRepeater: Ingen sprite tilldelad till SpriteRenderer!", this);
            return;
        }

        // Beräkna sprite-bredd i world units
        spriteWidth = originalSpriteRenderer.sprite.bounds.size.x * transform.localScale.x;

        if (totalCount < 1)
        {
            Debug.LogWarning("SeamlessSpriteRepeater: totalCount måste vara minst 1. Sätter till 1.", this);
            totalCount = 1;
        }

        // Skapa array för duplicaten (originalet räknas som första, så vi skapar totalCount - 1 kopior)
        duplicates = new GameObject[totalCount - 1];

        // Beräkna hur många sprites på varje sida (originalet ligger i mitten)
        int spritesOnEachSide = (totalCount - 1) / 2;
        int extraSprite = (totalCount - 1) % 2; // Om udda antal kopior, läggs en extra åt höger

        float actualWidth = spriteWidth - overlapAmount + manualOffset;

        // Skapa kopior
        int duplicateIndex = 0;

        // Skapa sprites till vänster om originalet
        for (int i = 1; i <= spritesOnEachSide; i++)
        {
            GameObject leftSprite = CreateDuplicate(-i, duplicateIndex, actualWidth);
            duplicates[duplicateIndex] = leftSprite;
            duplicateIndex++;
        }

        // Skapa sprites till höger om originalet
        for (int i = 1; i <= spritesOnEachSide + extraSprite; i++)
        {
            GameObject rightSprite = CreateDuplicate(i, duplicateIndex, actualWidth);
            duplicates[duplicateIndex] = rightSprite;
            duplicateIndex++;
        }

        if (showDebugInfo)
        {
            Debug.Log($"SeamlessSpriteRepeater: Skapade {duplicates.Length} kopior ({spritesOnEachSide} vänster, {spritesOnEachSide + extraSprite} höger). Total antal sprites: {totalCount}");
        }
    }

    /// Skapar en duplicerad sprite på angiven position.
    /// <param name="positionIndex">Positiv = höger, negativ = vänster från originalet</param>
    /// <param name="duplicateIndex">Index i duplicates-arrayen</param>
    /// <param name="actualWidth">Beräknad bredd mellan sprites</param>
    private GameObject CreateDuplicate(int positionIndex, int duplicateIndex, float actualWidth)
    {
        // Skapa nytt GameObject
        string side = positionIndex < 0 ? "Left" : "Right";
        GameObject duplicate = new GameObject($"{gameObject.name}_Copy_{side}_{Mathf.Abs(positionIndex)}");

        // Sätt som child till samma parent som originalet
        duplicate.transform.parent = transform.parent;

        // Kopiera transform-värden från originalet
        duplicate.transform.position = transform.position;
        duplicate.transform.rotation = transform.rotation;
        duplicate.transform.localScale = transform.localScale;

        // Lägg till SpriteRenderer och kopiera inställningar
        SpriteRenderer duplicateSR = duplicate.AddComponent<SpriteRenderer>();
        duplicateSR.sprite = originalSpriteRenderer.sprite;
        duplicateSR.color = originalSpriteRenderer.color;
        duplicateSR.sortingLayerID = originalSpriteRenderer.sortingLayerID;
        duplicateSR.sortingOrder = originalSpriteRenderer.sortingOrder;
        duplicateSR.material = originalSpriteRenderer.material;
        duplicateSR.flipY = originalSpriteRenderer.flipY;

        // Positionera spriten
        Vector3 offset = transform.right * (actualWidth * positionIndex);
        Vector3 newPosition = transform.position + offset;

        // Pixel-perfect justering om aktiverat
        if (pixelPerfect)
        {
            newPosition = RoundToNearestPixel(newPosition);
        }

        duplicate.transform.position = newPosition;

        // Spegla varje andra sprite om seamless är aktiverat
        if (seamless)
        {
            // Beräkna absolut position från centrum (originalet är 0)
            int absolutePosition = Mathf.Abs(positionIndex);

            // För att få sömlös spegling: spegla baserat på om positionen är udda/jämn
            // OCH ta hänsyn till om vi är på vänster eller höger sida
            bool shouldFlip;

            if (positionIndex < 0)
            {
                // Vänster sida: spegla udda positioner
                shouldFlip = (absolutePosition % 2 == 1);
            }
            else
            {
                // Höger sida: spegla udda positioner
                shouldFlip = (absolutePosition % 2 == 1);
            }

            duplicateSR.flipX = shouldFlip ? !originalSpriteRenderer.flipX : originalSpriteRenderer.flipX;

            if (showDebugInfo)
            {
                Debug.Log($"Sprite pos {positionIndex}: FlipX = {duplicateSR.flipX}");
            }
        }
        else
        {
            duplicateSR.flipX = originalSpriteRenderer.flipX;
        }

        return duplicate;
    }

    /// Rensar alla skapade duplicat.
    public void ClearDuplicates()
    {
        if (duplicates != null)
        {
            foreach (GameObject duplicate in duplicates)
            {
                if (duplicate != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(duplicate);
                    }
                    else
                    {
                        DestroyImmediate(duplicate);
                    }
                }
            }
            duplicates = null;
        }
    }

    private void OnDestroy()
    {
        ClearDuplicates();
    }

    /// Rundar position till närmaste pixel för pixel-perfect rendering.
    private Vector3 RoundToNearestPixel(Vector3 position)
    {
        if (originalSpriteRenderer == null || originalSpriteRenderer.sprite == null)
            return position;

        float pixelsPerUnit = originalSpriteRenderer.sprite.pixelsPerUnit;

        // Rundar varje komponent till närmaste pixel
        position.x = Mathf.Round(position.x * pixelsPerUnit) / pixelsPerUnit;
        position.y = Mathf.Round(position.y * pixelsPerUnit) / pixelsPerUnit;
        position.z = Mathf.Round(position.z * pixelsPerUnit) / pixelsPerUnit;

        return position;
    }

    /// Uppdaterar antalet sprites runtime.
    public void SetTotalCount(int count)
    {
        totalCount = Mathf.Max(1, count);
        CreateDuplicates();
    }

    /// Aktiverar eller inaktiverar seamless-läget runtime.
    public void SetSeamless(bool enabled)
    {
        seamless = enabled;
        CreateDuplicates();
    }

    /// Ändrar manuell offset runtime.
    public void SetManualOffset(float offset)
    {
        manualOffset = offset;
        CreateDuplicates();
    }

    /// Ändrar overlap-mängden runtime.
    public void SetOverlapAmount(float amount)
    {
        overlapAmount = amount;
        CreateDuplicates();
    }

    /// Aktiverar eller inaktiverar pixel-perfect läget runtime.
    public void SetPixelPerfect(bool enabled)
    {
        pixelPerfect = enabled;
        CreateDuplicates();
    }

    // Editor-funktionalitet för att se ändringar direkt i Inspector
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            CreateDuplicates();
        }
    }
}