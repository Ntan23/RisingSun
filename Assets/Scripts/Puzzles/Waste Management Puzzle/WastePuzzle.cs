using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WastePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject pipesParent;
    public GameObject[] pipes;
    [SerializeField] private Pipes[] pipeScripts;
    private int totalPipes = 0;
    private int correctedPipes = 0;
    private int filledPipes;
    [SerializeField] private  float totalTime;
    private float intialTime;
    private float[] intialPipeRotations;
    private bool isTimeStart;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private SpriteRenderer endPipeSpriteRenderer;
    [SerializeField] private Sprite finishedEndPipeSprite;

    void Start()
    {
        totalPipes = pipesParent.transform.childCount;
        intialTime = totalTime;

        // pipes = new GameObject[totalPipes];
        intialPipeRotations = new float[totalPipes];

        for(int i = 0; i < pipeScripts.Length; i++) 
        {
            //pipes[i] = pipesParent.transform.GetChild(i).gameObject;
            // intialPipeRotations[i] = pipes[i].transform.eulerAngles.z;
            intialPipeRotations[i] = pipeScripts[i].gameObject.transform.eulerAngles.z;
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

        CheckFilledPipes();

        Debug.Log("Correct Move");

        if(correctedPipes == totalPipes)
        {
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

    private void ResetPuzzle()
    {
        for(int i = 0; i < pipeScripts.Length; i++) 
        {
            LeanTween.rotateZ(pipeScripts[i].gameObject, intialPipeRotations[i], 0.0f);
            pipeScripts[i].ResetPipe();
            pipeScripts[i].CheckRotation();
        }

        totalTime = intialTime;
        isTimeStart = true;
    }
}
