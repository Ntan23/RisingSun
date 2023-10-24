using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    [SerializeField] private int neededHitCount;
    [SerializeField] private string hitTag;
    private int hitCount;
    private Collider2D[] hittedColliders;
    private Collider2D objCollider;
    private Transform[] pointsTransform;

    // Start is called before the first frame update
    void Start()
    {
        objCollider = GetComponent<Collider2D>();

        hittedColliders = new Collider2D[neededHitCount];
        pointsTransform = new Transform[neededHitCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(hitTag))
        {
            hitCount++;

            objCollider.OverlapCollider(new ContactFilter2D(), hittedColliders);
            //Debug.Log(hittedColliders.Length);

            // for(int i = 0; i < hittedColliders.Length; i++)
            // {
            //     Debug.Log(hittedColliders[i]);
            //     // hittedColliders[i].enabled = false;

            //     // if((i + 1) == hittedColliders.Length) objCollider.enabled = false;
            // }

            if(hitCount == neededHitCount) 
            {
                //Debug.Log(hitCount);
                Debug.Log("Complete");

                objCollider.OverlapCollider(new ContactFilter2D(), hittedColliders);
                //Debug.Log(hittedColliders.Length);

                for(int i = 0; i < hittedColliders.Length; i++)
                {
                    //Debug.Log(hittedColliders[i]);
                    hittedColliders[i].enabled = false;
                    pointsTransform[i] = hittedColliders[i].gameObject.GetComponent<Transform>();

                    if((i + 1) == hittedColliders.Length) 
                    {
                        objCollider.enabled = false;
                        // transform.position = CalculateMiddlePoint(pointsTransform);
                        // Debug.Log(hittedColliders[hittedColliders.Length - 1].transform.parent.gameObject);
                        LeanTween.move(gameObject, CalculateMiddlePoint(pointsTransform), 0.25f);

                        transform.parent = hittedColliders[hittedColliders.Length - 1].transform.parent;

                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(hitTag)) hitCount--;
    }

    private Vector2 CalculateMiddlePoint(Transform[] points)
    {
        Vector2 sum = Vector2.zero;

        for (int i = 0; i < points.Length; i++) sum += new Vector2(points[i].position.x, points[i].position.y);

        return sum / points.Length;
    }

    // void OnCollisionEnter(Collider2D other)
    // {
    //     if(other.CompareTag(hitTag)) 
    //     {
    //         //Debug.Log(hitCount);
    //         //other.gameObject.GetComponent<Collider2D>().enabled = false;
    //         hitCount++;

    //         if(hitCount == neededHitCount) 
    //         {
    //             //Debug.Log(hitCount);
    //             Debug.Log("Complete");

    //             objCollider.OverlapCollider(new ContactFilter2D(), hittedColliders);
    //             Debug.Log(hittedColliders.Length);

    //             for(int i = 0; i < hittedColliders.Length; i++)
    //             {
    //                 Debug.Log(hittedColliders[i]);
    //             }
    //         }
    //     }
    // }

    // void OnCollisionExit(Collider2D other)
    // {
    //     if(other.CompareTag(hitTag)) 
    //     {
    //         //Debug.Log("Exit");
    //         hitCount--;
    //         //Debug.Log("After Exit : " + hitCount);
    //         //other.gameObject.GetComponent<Collider2D>().enabled = true;
    //     }
    // }
}
