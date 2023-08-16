using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficPuzzle : MonoBehaviour
{
    [SerializeField] private CarControl[] carControl;
    private int carSelectedIndex;
    private bool isFirstTime = true;

    public void ResetSelectedCar() => carControl[carSelectedIndex].ResetValue();
    
    public void SetIsCarSelectedIndex(int index)
    {
        carSelectedIndex = index;
    }

    public int GetIsCarSelectedIndex()
    {
        return carSelectedIndex;
    }

    public bool GetIsFirstTime()
    {
        return isFirstTime;
    }

    public void ChangeIsFirstTimeValue() => isFirstTime = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Target")) Debug.Log("You Win");
    }
}
