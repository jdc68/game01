using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[ExecuteInEditMode]
public class Painting : MonoBehaviour
{
    public Sprite[] sprites;
    [Range(1, 11)]
    public int painting;
    public SpriteRenderer spriteRenderer;

    private void Update()
    {
        spriteRenderer.sprite = sprites[painting - 1];
    }
}
