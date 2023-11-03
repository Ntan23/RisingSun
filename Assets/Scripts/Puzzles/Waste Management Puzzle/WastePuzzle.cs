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
    [SerializeField] private bool thereIsTutorial;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private SpriteRenderer endPipeSpriteRenderer;
    [SerializeField] private Sprite finishedEndPipeSprite;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private Report report;
    [SerializeField] private GameObject[] objectThatNeedToDisable; 
    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private Sprite bgSprite;

    void Start()
    {
        totalPipes = pipeScripts.Length;
        intialTime = totalTime;

        if(!thereIsTutorial) StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.6f);
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
            ShowReport();
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
        correctedPipes = 0;

        for(int i = 0; i < pipeScripts.Length; i++) pipeScripts[i].ResetPipe();
        
        totalTime = intialTime;
        isTimeStart = true;
    }

    private void ShowReport()
    {
        LeanTween.rotateZ(puzzleSpriteMask, 90.0f, 0.5f);
        LeanTween.rotateZ(gameObject, 90.0f, 0.5f).setOnComplete(() =>
        {
            bgSpriteRenderer.sprite = bgSprite;

            foreach(GameObject go in objectThatNeedToDisable) go.SetActive(false);
            
            report.gameObject.SetActive(true);
            StartCoroutine(report.StartReport());
        });
    }

    public bool GetIsComplete()
    {
        return isComplete;
    }
}
