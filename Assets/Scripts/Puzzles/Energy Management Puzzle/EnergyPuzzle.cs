using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPuzzle : MonoBehaviour
{
    [SerializeField] private float arrowRotateSpeed;
    [SerializeField] private TextMeshProUGUI roundText;
    private float currentArrowRotation;
    private float absCurrentArrowRotation;
    private int roundIndex;
    private int hitCount;
    private bool isActive;
    private bool isWin;
    [SerializeField] private GameObject[] rounds;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform[] arrows;
    [SerializeField] private Threshold[] thresholds;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private GameObject[] objectThatNeedToDisable;
    [SerializeField] private Report report;

    void Start() => roundText.text = "Round " + (roundIndex + 1).ToString() + " / " + rounds.Length.ToString();
    
    void Update()
    {
        if(isActive)
        {
            if(currentArrowRotation > -180.0f)
            {
                currentArrowRotation -= arrowRotateSpeed * Time.deltaTime;
                arrows[roundIndex].rotation = Quaternion.Euler(0, 0, currentArrowRotation);
            }
            else if(currentArrowRotation <= -180.0f)
            {
                Debug.Log("You Lose");
                ResetPuzzle();
            }
        }
    }

    public void StartPuzzle() 
    {
        isActive = true;
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
    }

    public void StopPuzzle()
    {
        absCurrentArrowRotation = Mathf.Abs(currentArrowRotation);

        if(isActive)
        {
            isActive = false;
            
            for(int i = hitCount; i < thresholds[roundIndex].start.Length; i++)
            {   
                if(i + 1 < thresholds[roundIndex].start.Length)
                {
                    if(absCurrentArrowRotation < thresholds[roundIndex].start[i] || absCurrentArrowRotation > thresholds[roundIndex].end[i]) 
                    {
                        Debug.Log("You Lose");
                        ResetPuzzle();
                    }

                    if(absCurrentArrowRotation >= thresholds[roundIndex].start[i] && absCurrentArrowRotation <= thresholds[roundIndex].end[i]) 
                    {
                        hitCount++;
                        Debug.Log("Hit");
                        isActive = true;
                        break;
                    }
                }
                
                if(i + 1 == thresholds[roundIndex].start.Length)
                {
                    if(absCurrentArrowRotation < thresholds[roundIndex].start[i] || absCurrentArrowRotation > thresholds[roundIndex].end[i]) 
                    {
                        Debug.Log("You Lose In " + (i + 1));
                        ResetPuzzle();
                    }

                    if(absCurrentArrowRotation >= thresholds[roundIndex].start[i] && absCurrentArrowRotation <= thresholds[roundIndex].end[i])
                    {
                        hitCount++;

                        if(hitCount == thresholds[roundIndex].start.Length)
                        {
                            roundIndex++;

                            if(roundIndex < rounds.Length)
                            {
                                roundText.text = "Round " + (roundIndex + 1).ToString() + " / " + rounds.Length.ToString();
                                ResetPuzzle();
                                rounds[roundIndex].SetActive(true);
                                rounds[roundIndex - 1].SetActive(false);
                            }
                            
                            if(roundIndex == rounds.Length)
                            {
                                Debug.Log("Show Report");
                                isWin = true;
                                ShowReport();
                            }
                        }
                    }
                }
            }
        }
    }

    public void ResetPuzzle()
    {
        if(!isWin)
        {
            hitCount = 0;
            currentArrowRotation = 0.0f;
            arrows[roundIndex].rotation = Quaternion.identity;
            isActive = false;
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
        }
    }

    private void ShowReport()
    {
        LeanTween.rotateZ(puzzleSpriteMask, 90.0f, 0.5f);
        LeanTween.rotateZ(gameObject, 90.0f, 0.5f).setOnComplete(() =>
        {
            foreach(GameObject go in objectThatNeedToDisable) go.SetActive(false);
            
            report.gameObject.SetActive(true);
            StartCoroutine(report.StartReport());
        });
    }
}
