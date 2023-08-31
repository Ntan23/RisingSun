using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool canBeDrag = true;
    private Vector2 initialPosition;
    private Vector2 offset;
    private PuzzleSlot slot;
    private ResourcePuzzle resourcePuzzle;

    void Start() 
    {
        resourcePuzzle = GetComponentInParent<ResourcePuzzle>();

        initialPosition = transform.position;
    }
    
    public void Initialization(PuzzleSlot puzzleSlot)
    {
        slot = puzzleSlot;
        spriteRenderer.sprite = puzzleSlot.GetSlotSprite();
    }

    void OnMouseDown() 
    {
        if(canBeDrag) offset = GetMousePosition() - (Vector2)transform.position;
    }

    void OnMouseDrag()
    {
        if(canBeDrag) transform.position = GetMousePosition() - offset;
    }

    void OnMouseUp()
    {
        if(Vector2.Distance(transform.position, slot.transform.position) < 2.0f)
        {
            canBeDrag = false;
            LeanTween.move(gameObject, slot.transform.position, 0.5f).setEaseSpring().setOnComplete(() =>
            {
                resourcePuzzle.CheckLevel();
            });
        }
        else
        {
            canBeDrag = false;

            LeanTween.move(gameObject, initialPosition, 0.5f).setEaseSpring().setOnComplete(() =>
            {
                canBeDrag = true;
            });
        }
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool SetBackCanBeDrag() => canBeDrag = true;
}
