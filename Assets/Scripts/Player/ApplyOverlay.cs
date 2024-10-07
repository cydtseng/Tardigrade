using System;
using UnityEngine;

// Make sure all the target sprites are readable and writeable
public class ApplyOverlay : MonoBehaviour
{
    public Sprite[] overlaySprites;  // new Sprite to overlap using
    private SpriteRenderer baseRenderer;  // Renderer to replace Sprite for
    private Sprite baseSprite;

    // Merge all changes into a new separate sprite to avoid overwrites
    private Texture2D targetTexture;
    private Texture2D baseTexture;  // caching

    void Awake() {
        baseRenderer = GetComponent<SpriteRenderer>();
        baseSprite = baseRenderer.sprite;
    }

    void Start() {
        // Note: Textures are only synchronized at Start()
        baseTexture = baseSprite.texture;
        targetTexture = new Texture2D(baseTexture.width, baseTexture.height);
        Apply();
    }

    public void Apply() {
        // Populate with base first
        int mipMapLevel = baseTexture.loadedMipmapLevel;
        Color[] baseColors = baseTexture.GetPixels(mipMapLevel);

        // Populate with overlays, earlier indexes are nearer to the base
        foreach (Sprite overlaySprite in overlaySprites) {
            Texture2D overlayTexture = overlaySprite.texture;
            Color[] overlayColors = overlayTexture.GetPixels(mipMapLevel);

            // Fill non-transparent objects
            for (int i = 0; i < overlayColors.Length; i++) {
                if (overlayColors[i].a != 0) {  // not transparent
                    baseColors[i] = overlayColors[i];
                }
            }
        }

        // Write to texture in GPU, then assign
        targetTexture.SetPixels(baseColors, mipMapLevel);
        targetTexture.Apply(updateMipmaps: false);

        Sprite targetSprite = Sprite.Create(targetTexture, baseSprite.rect, baseSprite.pivot);
        baseRenderer.sprite = targetSprite;
    }
}
