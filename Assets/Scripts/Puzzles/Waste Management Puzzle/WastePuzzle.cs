using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WastePuzzle : MonoBehaviour
{
    public GameObject pipesParent;
    public GameObject[] pipes;
    private int totalPipes = 0;
    private int correctedPipes = 0;
    [SerializeField] private  float totalTime;
    private float intialTime;
    private float[] intialPipeRotations;
    private bool isTimeStart;
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        totalPipes = pipesParent.transform.childCount;
        intialTime = totalTime;

        pipes = new GameObject[totalPipes];
        intialPipeRotations = new float[totalPipes];

        for(int i = 0; i < pipes.Length; i++) 
        {
            pipes[i] = pipesParent.transform.GetChild(i).gameObject;
            intialPipeRotations[i] = pipes[i].transform.eulerAngles.z;
        }
    
        StartTimer();
    }

    public void StartTimer() 
    {
        isTimeStart = true;
        StartCoroutine(Timer());
    }

    public void CorrectMove()
    {
        correctedPipes += 1;

        Debug.Log("Correct Move");

        if(correctedPipes == totalPipes)
        {
            Debug.Log("You Win! & Show Report");
        }
    }

    public void WrongMove() => correctedPipes -= 1;
    
    IEnumerator Timer()
    {
        while(isTimeStart)
        {
            totalTime -= Time.deltaTime;

            timerText.text = TimeSpan.FromSeconds(totalTime).ToString("mm':'ss");

            if(totalTime <= 0.0f) 
            {
                isTimeStart = false;
                Debug.Log("You Lose");
                ResetPuzzle();
            }

            yield return null;
        }
    }

    private void ResetPuzzle()
    {
        for(int i = 0; i < totalPipes; i++) pipes[i].transform.Rotate(0.0f, 0.0f, intialPipeRotations[i]);
        totalTime = intialTime;
        isTimeStart = true;
    }
}
