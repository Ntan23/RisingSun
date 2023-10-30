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
    private bool isPlaying;
    private bool canDetectCollision;
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private GameObject[] objectThatNeedToDisable;
    [SerializeField] private Report report;
    [SerializeField] private TrafficSign[] ts;

    void Start() => StartCoroutine(Delay());

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);

        initialCarPos = new Vector3[cars.Length];
        initialRotation = new Quaternion[cars.Length];
        intialAvailableMove = availableMove;

        moveCountText.text = availableMove.ToString();

        for(int i = 0; i < cars.Length; i++)
        {
            initialCarPos[i] = cars[i].transform.localPosition;
            initialRotation[i] = cars[i].transform.rotation;
        }

        canDetectCollision = true;
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
        isPlaying = false;
        availableMove = intialAvailableMove;

        //moveCountText.text = availableMove.ToString();

        for(int i = 0; i < cars.Length; i++)
        {
            LeanTween.cancel(cars[i]);
            cars[i].transform.localPosition = initialCarPos[i];
            cars[i].transform.rotation = initialRotation[i];

            cars[i].GetComponent<CarMovement>().ResetValues();
            // carControl[i].ResetValue();
        }

        for(int j = 0; j < ts.Length; j++) ts[j].ResetTrafficSign();

        isWin = false;
    }

    public void StartPuzzle()
    {
        if(!isPlaying)
        {
            for(int i = 0; i < cars.Length; i++)
            {
                cars[i].GetComponent<CarMovement>().StartMove();
                isPlaying = true;
            }
        }
    }

    public IEnumerator Crash()
    {
        for(int i = 0; i < cars.Length; i++)
        {
            LeanTween.cancel(cars[i]);

            cars[i].GetComponent<CarMovement>().CarIdle();
        }

        yield return new WaitForSeconds(0.8f);
        ResetPuzzle();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cars") && canDetectCollision)
        {
            if(other.gameObject.name == "Pink Car" && !isWin)
            {
                isWin = true;
                Debug.Log("You Win & Show Report");

                ShowReport();
            }
            
            Destroy(other.gameObject);
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

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}
