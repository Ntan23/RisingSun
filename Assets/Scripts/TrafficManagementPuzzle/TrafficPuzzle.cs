using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrafficPuzzle : MonoBehaviour
{
    [SerializeField] private CarControl[] carControl;
    private Vector3[] initialCarPos;
    private int carSelectedIndex;
    [SerializeField] private int availableMove;
    private int intialAvailableMove;
    private bool isFirstTime = true;
    private bool isWin;
    [SerializeField] private TextMeshProUGUI moveCountText;

    void Start()
    {
        initialCarPos = new Vector3[carControl.Length];
        intialAvailableMove = availableMove;

        moveCountText.text = availableMove.ToString();

        for(int i = 0; i < carControl.Length; i++)
        {
            initialCarPos[i] = carControl[i].gameObject.transform.position;
        }
    }

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

    public void UpdateAvailableMove()
    {
        availableMove--;
        moveCountText.text = availableMove.ToString();

        if(availableMove == 0 && !isWin) 
        {
            Debug.Log("You Lose");
            ResetPuzzle();
        }
    }

    public void ChangeIsFirstTimeValue() => isFirstTime = false;

    public void ResetPuzzle()
    {
        availableMove = intialAvailableMove;
        moveCountText.text = availableMove.ToString();

        for(int i = 0; i < carControl.Length; i++)
        {
            carControl[i].gameObject.transform.position = initialCarPos[i];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Target"))
        {
            isWin = true;
            Debug.Log("You Win & Show Report");
        } 
    }
}
