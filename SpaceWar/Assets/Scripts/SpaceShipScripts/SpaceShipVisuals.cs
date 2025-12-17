using UnityEngine;

public class SpaceShipVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer turboSprite;
    
    // İleride buraya ses veya partikül de ekleyebilirsin, Controller'ın haberi bile olmaz.
    public void SetTurboVisual(bool isActive)
    {
        if(turboSprite != null)
            turboSprite.enabled = isActive;
    }
}
