using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Sprite GetSlotSprite()
    {
        return spriteRenderer.sprite;
    }
}
