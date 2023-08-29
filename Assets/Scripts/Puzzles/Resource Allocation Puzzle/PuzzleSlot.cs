using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite originalSprite;

    public Sprite GetSlotSprite()
    {
        return originalSprite;
    }
}
