using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private int neededHitCount;
    [SerializeField] private string hitTag;
    private int hitCount;
    [SerializeField] private int index;
    private bool canBeDrag = true;
    private bool canBeRotate = true;
    private Vector2 offset;
    private Vector2 intialPosition;
    private Collider2D[] hittedColliders;
    private Collider2D[] circleColliders;
    private Collider2D objCollider;
    private Transform[] pointsTransform;
    private ObjectSpawner spawner;
    private ResourcePuzzle rp;
    
    public void Initialization()
    {
        spawner = transform.parent.GetChild(index).GetComponent<ObjectSpawner>();
        rp = spawner.GetComponentInParent<ResourcePuzzle>();
        //Debug.Log(spawner);

        objCollider = GetComponent<Collider2D>();

        hittedColliders = new Collider2D[neededHitCount];
        pointsTransform = new Transform[neededHitCount];
        circleColliders = new Collider2D[transform.childCount];

        for(int i = 1; i < transform.childCount; i++) circleColliders[i - 1] = transform.GetChild(i).GetComponent<Collider2D>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && canBeRotate) 
        {
            canBeDrag = false;
            canBeRotate = false;

            LeanTween.rotateZ(gameObject, transform.eulerAngles.z + 90.0f, 0.1f).setOnComplete(() => 
            {
                canBeDrag = true;
                canBeRotate = true;
            });
        }
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
        if(hitCount == neededHitCount)
        {
            canBeDrag = false;
            canBeRotate = false;

            for(int i = 0; i < hittedColliders.Length; i++)
            {
                pointsTransform[i] = hittedColliders[i].gameObject.GetComponent<Transform>();
                hittedColliders[i].enabled = false;

                if(i == hittedColliders.Length - 1) 
                {   
                    LeanTween.moveLocal(gameObject, CalculateMiddlePoint(pointsTransform), 0.3f).setOnComplete(() =>
                    {
                        objCollider.enabled = false;

                        for(int i = 1; i < transform.childCount; i++) circleColliders[i - 1].enabled = true;

                        spawner.ChangeBackCanSpawnValue();

                        transform.parent = hittedColliders[hittedColliders.Length - 1].transform.parent;

                        rp.UpdateObject(index);
                    });
                }
            }
        }

        if(hitCount < neededHitCount && canBeDrag && Vector2.Distance(transform.position, spawner.GetSpawnPosition().position) > 0f) 
        {
            LeanTween.move(gameObject, spawner.transform.position, 0.25f).setOnComplete(() =>
            {
                spawner.DecreaseSpawnedCount();
                spawner.ChangeBackCanSpawnValue();
                Destroy(this.gameObject);
            });
        }
    }

    
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(hitTag))
        {
            hitCount++;

            objCollider.OverlapCollider(new ContactFilter2D(), hittedColliders);
            //Debug.Log(hittedColliders.Length);
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(hitTag)) hitCount--;
    }

    private Vector2 CalculateMiddlePoint(Transform[] points)
    {
        Vector2 sum = Vector2.zero;

        foreach (Transform point in points) sum += new Vector2(point.position.x, point.position.y);

        return sum / points.Length;
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
