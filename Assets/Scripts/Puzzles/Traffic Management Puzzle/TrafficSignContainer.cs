using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignContainer : MonoBehaviour
{
    private bool isFilled;
    
    public void SetIsFilled() => isFilled = true;

    public void SetBackIsFilled() => isFilled = false;

    public bool GetIsFilled() 
    {
        return isFilled;
    }
}
