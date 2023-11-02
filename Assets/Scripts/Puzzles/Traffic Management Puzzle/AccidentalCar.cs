using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentalCar : MonoBehaviour
{
    [SerializeField] private GameObject car;
    private CarMovement carMovement;
    private TrafficPuzzle tp;

    void Start()
    {
        carMovement = car.GetComponent<CarMovement>();
        tp = GetComponentInParent<TrafficPuzzle>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cars")) carMovement.StartMove();
    }
}
