using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSign : MonoBehaviour
{
    [System.Serializable]
    public class TrafficSignContainers
    {
        public GameObject container;
        public CarMovement[] carsThatGetTheEffect;
    } 

    private enum Type{
        UTurn, CantTurnLeft, CantTurnRight, Stop
    }
    
    [SerializeField] private Type signType;
    private bool canBeDrag = true;
    private Vector2 initialPosition;
    private Vector2 offset;
    //[SerializeField] private GameObject[] trafficSignContainer;
    [SerializeField] private TrafficPuzzle tp;
    //[SerializeField] private CarMovement[] carThatGetTheEffect;
    [SerializeField] private TrafficSignContainers[] trafficSignContainers;

    void Start() => initialPosition = transform.localPosition;

    void OnMouseDown() 
    {
        if(!tp.GetIsPlaying())
        {
            if(canBeDrag) offset = GetMousePosition() - (Vector2)transform.position;

            for(int i = 0; i < trafficSignContainers.Length; i++)
            {
                if(Vector2.Distance(transform.position, trafficSignContainers[i].container.transform.position) <= 0.1f && !canBeDrag)
                {
                    LeanTween.move(gameObject, initialPosition, 0.5f).setEaseSpring().setOnComplete(() =>
                    {
                        trafficSignContainers[i].container.tag = "Untagged";
                        canBeDrag = true;

                        if(trafficSignContainers[i].carsThatGetTheEffect.Length > 0) for(int j = 0; j < trafficSignContainers[i].carsThatGetTheEffect.Length; j++) trafficSignContainers[i].carsThatGetTheEffect[j].ResetValues();
                    });

                    break;
                }
                else continue;
            }
        }
    }

    void OnMouseDrag()
    {
        if(canBeDrag && !tp.GetIsPlaying()) transform.position = GetMousePosition() - offset;
    }

    void OnMouseUp()
    {
        if(!tp.GetIsPlaying())
        {
            for(int i = 0; i < trafficSignContainers.Length; i++)
            {
                if(Vector2.Distance(transform.position, trafficSignContainers[i].container.transform.position) <= 0.5f && canBeDrag)
                {
                    canBeDrag = false;
                    LeanTween.move(gameObject, trafficSignContainers[i].container.transform.position, 0.5f).setEaseSpring().setOnComplete(() =>
                    {
                        if(signType == Type.UTurn) 
                        {
                            for(int j = 0; j < trafficSignContainers[i].carsThatGetTheEffect.Length; j++) trafficSignContainers[i].carsThatGetTheEffect[j].ChangeToUTurnWaypoint(); 
                        }
                        if(signType == Type.CantTurnLeft)
                        {
                            for(int j = 0; j < trafficSignContainers[i].carsThatGetTheEffect.Length; j++) trafficSignContainers[i].carsThatGetTheEffect[j].ChangeToTurnRightWaypoint();
                        } 
                        if(signType == Type.CantTurnRight) 
                        {
                            for(int j = 0; j < trafficSignContainers[i].carsThatGetTheEffect.Length; j++) trafficSignContainers[i].carsThatGetTheEffect[j].ChangeToTurnLeftWaypoint();
                        }
                        if(signType == Type.Stop) trafficSignContainers[i].container.tag = "Stop"; 

                        //Debug.Log("Container " + i + " tag : " + trafficSignContainer[i].tag);
                    });
                    break;
                }
                else if(i + 1 < trafficSignContainers.Length) continue;
                else if(i + 1 == trafficSignContainers.Length)
                {
                    LeanTween.move(gameObject, initialPosition, 0.5f).setEaseSpring().setOnComplete(() =>
                    {
                        trafficSignContainers[i].container.tag = "Untagged";
                        canBeDrag = true;
                        //Debug.Log("Container " + i + " tag : " + trafficSignContainer[i].tag);
                    });
                    break;
                }
            }
        }
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void ResetTrafficSign()
    {
        transform.localPosition = initialPosition;
        
        for(int i = 0; i < trafficSignContainers.Length; i++)
        {
            trafficSignContainers[i].container.tag = "Untagged";
            canBeDrag = true;
        }
    }
}
