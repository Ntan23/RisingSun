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
    private int clickCount;
    [SerializeField] private int carIndex;
    private bool isSelected;
    private bool isTouched;	
    private Vector3 mousePos;
    private Vector3 previousMousePos;
    private Rigidbody2D rb;
    private Collider2D objCollider;
    private SpriteRenderer objRenderer;
    private TrafficPuzzle tp;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objCollider = GetComponent<Collider2D>();
        objRenderer = GetComponent<SpriteRenderer>();
        tp = GetComponentInParent<TrafficPuzzle>();

        originalSprite = objRenderer.sprite;
        rb.isKinematic = true;
    }

    void OnMouseDown()
    {
        clickCount++;

        if(clickCount == 1) isSelected = !isSelected;
        
        if(isSelected)
        {
            if(tp.GetIsFirstTime()) 
            {
                tp.SetIsCarSelectedIndex(carIndex);
                tp.ChangeIsFirstTimeValue();
            }
            if(!tp.GetIsFirstTime() && carIndex != tp.GetIsCarSelectedIndex()) 
            {
                tp.ResetSelectedCar();           
                tp.SetIsCarSelectedIndex(carIndex);
            }
            
            objRenderer.sprite = selectedSprite;

            //previousMousePos = Input.mousePosition;
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
        if(isSelected && clickCount > 1)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(objCollider == Physics2D.OverlapPoint(mousePos) && isTouched) rb.MovePosition(new Vector2(mousePos.x - deltaX, mousePos.y - deltaY));
        }
    }

    void OnMouseUp()
    {
        if(clickCount > 1 && Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), mousePos) < 0.1f)
        {
            isSelected = false;
            objRenderer.sprite = originalSprite;
            clickCount = 0;
        }

        isTouched = false;
        rb.isKinematic = true;
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public void ResetValue()
    {
        objRenderer.sprite = originalSprite;
        isSelected = false;
        clickCount = 0;
    }

    IEnumerator ResetCar()
    {
        tp.ResetSelectedCar();
        yield return new WaitForSeconds(0.1f);
        tp.SetIsCarSelectedIndex(carIndex);
    }
}
