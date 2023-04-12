using System.Collections;
using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    public Sprite[] waterFrames; // Assign the frames of water animation in the editor
    public float frameRate = 0.1f; // Time between each frame of the animation

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimateWater());
    }

    IEnumerator AnimateWater()
    {
        int currentFrame = 0;
        while (true)
        {
            spriteRenderer.sprite = waterFrames[currentFrame];
            currentFrame = (currentFrame + 1) % waterFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}
