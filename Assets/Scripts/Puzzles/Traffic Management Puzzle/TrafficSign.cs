using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSign : MonoBehaviour
{
    private enum Type{
        UTurn, CantTurnLeft, CantTurnRight, Stop
    }
    
    [SerializeField] private Type signType;
    private bool canBeDrag = true;
    private Vector2 initialPosition;
    private Vector2 offset;
    [SerializeField] private GameObject[] trafficSignContainer;

    void Start()
    {
        initialPosition = transform.position;
    }

    void OnMouseDown() 
    {
        if(canBeDrag) offset = GetMousePosition() - (Vector2)transform.position;

        for(int i = 0; i < trafficSignContainer.Length; i++)
        {
            if(Vector2.Distance(transform.position, trafficSignContainer[i].transform.position) <= 0.1f && !canBeDrag)
            {
                LeanTween.move(gameObject, initialPosition, 0.5f).setEaseSpring().setOnComplete(() =>
                {
                    trafficSignContainer[i].tag = "Untagged";
                    canBeDrag = true;
                    Debug.Log("Container " + i + " tag : " + trafficSignContainer[i].tag);
                });
                break;
            }
            else continue;
        }
    }

    void OnMouseDrag()
    {
        if(canBeDrag) transform.position = GetMousePosition() - offset;
    }

    void OnMouseUp()
    {
        for(int i = 0; i < trafficSignContainer.Length; i++)
        {
            if(Vector2.Distance(transform.position, trafficSignContainer[i].transform.position) <= 1.0f && canBeDrag)
            {
                canBeDrag = false;
                LeanTween.move(gameObject, trafficSignContainer[i].transform.position, 0.5f).setEaseSpring().setOnComplete(() =>
                {
                    if(signType == Type.UTurn) trafficSignContainer[i].tag = "UTurn";
                    if(signType == Type.CantTurnLeft) trafficSignContainer[i].tag = "Right";
                    if(signType == Type.CantTurnRight) trafficSignContainer[i].tag = "Left";
                    if(signType == Type.Stop) trafficSignContainer[i].tag = "Stop"; 

                    Debug.Log("Container " + i + " tag : " + trafficSignContainer[i].tag);
                });
                break;
            }
            else if(i + 1 < trafficSignContainer.Length) continue;
            else if(i + 1 == trafficSignContainer.Length)
            {
                LeanTween.move(gameObject, initialPosition, 0.5f).setEaseSpring().setOnComplete(() =>
                {
                    trafficSignContainer[i].tag = "Untagged";
                    canBeDrag = true;
                    Debug.Log("Container " + i + " tag : " + trafficSignContainer[i].tag);
                });
                break;
            }
        }
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
