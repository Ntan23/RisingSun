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
    private bool isActive;
    [SerializeField] private GameObject[] rounds;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform[] arrows;
    [SerializeField] private Threshold[] thresholds;

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

            if(absCurrentArrowRotation >= thresholds[roundIndex].start && absCurrentArrowRotation <= thresholds[roundIndex].end)
            {
                Debug.Log("You Win");
                roundIndex++;

                if(roundIndex < rounds.Length)
                {
                    roundText.text = "Round " + (roundIndex + 1).ToString() + " / " + rounds.Length.ToString();
                    ResetPuzzle();
                    rounds[roundIndex].SetActive(true);
                    rounds[roundIndex - 1].SetActive(false);
                }
                else if(roundIndex == rounds.Length)
                {
                    Debug.Log("Show Report");
                }
            }
            else 
            {
                Debug.Log("You Lose");
                ResetPuzzle();
            }
        }
    }

    public void ResetPuzzle()
    {
        currentArrowRotation = 0.0f;
        arrows[roundIndex].rotation = Quaternion.identity;
        isActive = false;
        buttons[0].SetActive(true);
        buttons[1].SetActive(false);
    }
}
