using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WastePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject pipesParent;
    [SerializeField] private Pipes[] pipeScripts;
    private int totalPipes = 0;
    private int correctedPipes = 0;
    [SerializeField] private  float totalTime;
    private float intialTime;
    private bool isTimeStart;
    private bool isComplete;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private SpriteRenderer endPipeSpriteRenderer;
    [SerializeField] private Sprite finishedEndPipeSprite;

    void Start()
    {
        totalPipes = pipeScripts.Length;
        intialTime = totalTime;

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

        CheckFilledPipes();

        Debug.Log("Correct Move");

        if(correctedPipes == totalPipes)
        {
            isComplete = true;
            isTimeStart = false;
            endPipeSpriteRenderer.sprite = finishedEndPipeSprite;
            Debug.Log("You Win! & Show Report");
        }
    }

    public void WrongMove() 
    {
        correctedPipes -= 1;

        CheckFilledPipes();
    }
    
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

    public void CheckFilledPipes()
    {
        for(int i = 0; i < pipeScripts.Length; i++)
        {
            if(i == 0) 
            {
                if(pipeScripts[i].GetIsPlaced()) pipeScripts[i].ChangeToFilledSprite();
                else pipeScripts[i].ChangeToUnfilledSprite();
            }

            if(i > 0) 
            {
                if(pipeScripts[i - 1].GetIsFilled() && pipeScripts[i].GetIsPlaced()) pipeScripts[i].ChangeToFilledSprite();
                else pipeScripts[i].ChangeToUnfilledSprite();
            }
        }
    }

    public void ResetPuzzle()
    {
        for(int i = 0; i < pipeScripts.Length; i++) pipeScripts[i].ResetPipe();
        
        totalTime = intialTime;
        isTimeStart = true;
    }

    public bool GetIsComplete()
    {
        return isComplete;
    }
}
