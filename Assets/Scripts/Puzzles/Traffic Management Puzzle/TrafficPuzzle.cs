using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrafficPuzzle : MonoBehaviour
{
    //[SerializeField] private CarControl[] carControl;
    [SerializeField] private GameObject[] cars;
    private Vector3[] initialCarPos;
    private Quaternion[] initialRotation;
    private int carSelectedIndex;
    [SerializeField] private int availableMove;
    private int intialAvailableMove;
    private bool isFirstTime = true;
    private bool isWin;
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private GameObject[] objectThatNeedToDisable;
    [SerializeField] private Report report;

    void Start()
    {
        initialCarPos = new Vector3[cars.Length];
        initialRotation = new Quaternion[cars.Length];
        intialAvailableMove = availableMove;

        moveCountText.text = availableMove.ToString();

        for(int i = 0; i < cars.Length; i++)
        {
            initialCarPos[i] = cars[i].transform.localPosition;
            initialRotation[i] = cars[i].transform.rotation;
        }
    }

    //public void ResetSelectedCar() => carControl[carSelectedIndex].ResetValue();
    
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
        if(!isWin)
        {
            availableMove = intialAvailableMove;

            //moveCountText.text = availableMove.ToString();

            for(int i = 0; i < cars.Length; i++)
            {
                LeanTween.cancel(cars[i]);
                cars[i].transform.localPosition = initialCarPos[i];
                cars[i].transform.rotation = initialRotation[i];

                if(cars[i].GetComponent<CarMovement>() != null) cars[i].GetComponent<CarMovement>().ResetValues();
                // carControl[i].ResetValue();
            }

            isWin = false;
        }
    }

    public void StartPuzzle()
    {
        for(int i = 0; i < cars.Length; i++)
        {
            if(cars[i].GetComponent<CarMovement>() != null) cars[i].GetComponent<CarMovement>().StartMove();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Target"))
        {
            isWin = true;
            Debug.Log("You Win & Show Report");

            ShowReport();
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

    public bool GetIsWin()
    {
        return isWin;
    }
}
