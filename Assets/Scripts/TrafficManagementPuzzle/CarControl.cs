using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    #region Variables
    [SerializeField] private Sprite selectedSprite;
    private Sprite originalSprite;
    private float deltaX;
    private float deltaY;
    private bool isSelected;
    private bool isTouched;	
    private Vector3 mousePos;
    private Rigidbody2D rb;
    private Collider2D objCollider;
    private SpriteRenderer objRenderer;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objCollider = GetComponent<Collider2D>();
        objRenderer = GetComponent<SpriteRenderer>();

        originalSprite = objRenderer.sprite;
        rb.isKinematic = true;
    }

    void OnMouseDown()
    {
        if(isSelected)
        {
            isSelected = false;
            objRenderer.sprite = originalSprite;
        }
        else if(!isSelected)
        {
            isSelected = true;
            objRenderer.sprite = selectedSprite;

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
            if(objCollider == Physics2D.OverlapPoint(mousePos)) 
            {
                isTouched = true;
                    
                rb.isKinematic = false;

                //offset
                deltaX = mousePos.x - transform.position.x;
                deltaY = mousePos.y - transform.position.y;
            }
        }
        
    }

    void OnMouseDrag()
    {
        if(isSelected)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(objCollider == Physics2D.OverlapPoint(mousePos) && isTouched) rb.MovePosition(new Vector2(mousePos.x - deltaX, mousePos.y - deltaY));
        }
    }

    void OnMouseUp()
    {
        isTouched = false;
        rb.isKinematic = true;
    }
}
