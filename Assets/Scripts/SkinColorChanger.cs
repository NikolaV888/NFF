using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SkinColorChanger : MonoBehaviour
{
    public Color skinColor = Color.white;
    private Material material;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = new Material(Shader.Find("Custom/ColorSwapShader"));
        spriteRenderer.material = material;
    }

    private void Start()
    {
        SetSkinColor(skinColor);
    }

    public void SetSkinColor(Color newColor)
    {
        skinColor = newColor;
        material.SetColor(ColorProperty, skinColor);
    }
}
