using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrafficPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject[] indicator;
    private Vector3[] initialCarPos;
    private Quaternion[] initialRotation;
    private int currentCarInDestination;
    private bool isWin;
    private bool isPlaying;
    private bool canDetectCollision;
    private bool isError;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private GameObject[] objectThatNeedToDisable;
    [SerializeField] private GameObject popUpError;
    [SerializeField] private GameObject accidentCar;
    [SerializeField] private AccidentalCar accidentalCar;
    [SerializeField] private Report report;
    [SerializeField] private TrafficSign[] ts;
    private AudioManager am;

    void Start() => StartCoroutine(Delay());

    IEnumerator Delay()
    {
        am = AudioManager.instance;

        yield return new WaitForSeconds(0.6f);

        initialCarPos = new Vector3[cars.Length];
        initialRotation = new Quaternion[cars.Length];

        for(int i = 0; i < cars.Length; i++)
        {
            initialCarPos[i] = cars[i].transform.localPosition;
            initialRotation[i] = cars[i].transform.rotation;
        }

        canDetectCollision = true;
    }

    public void ResetPuzzle()
    {
        currentCarInDestination = 0;
        isPlaying = false;

        for(int i = 0; i < indicator.Length; i++) indicator[i].SetActive(true);

        for(int i = 0; i < cars.Length; i++)
        {
            if(cars[i] != null)
            {
                LeanTween.cancel(cars[i]);
                cars[i].transform.localPosition = initialCarPos[i];
                cars[i].transform.rotation = initialRotation[i];

                cars[i].GetComponent<CarMovement>().ResetValues();
            }
        }

        for(int j = 0; j < ts.Length; j++) ts[j].ResetTrafficSign();

        isWin = false;
    }

    public void StartPuzzle()
    {
        if(!isPlaying)
        {
            for(int i = 0; i < indicator.Length; i++) indicator[i].SetActive(false);

            for(int i = 0; i < cars.Length; i++)
            {
                cars[i].GetComponent<CarMovement>().StartMove();
                isPlaying = true;
            }
        }
    }

    public IEnumerator Crash()
    {
        if(!isWin) am.PlayCarCrashSFX();
        yield return new WaitForSeconds(0.1f);

        if(!isError)
        {
            for(int i = 0; i < cars.Length; i++)
            {
                if(cars[i] != null)
                {
                    LeanTween.cancel(cars[i]);

                    cars[i].GetComponent<CarMovement>().CarIdle();
                }
            }

            yield return new WaitForSeconds(0.8f);
            ResetPuzzle();
        }
    }

    public void ShowError()
    {
        isError = true;

        accidentCar.GetComponent<CarMovement>().CarIdle();

        for(int i = 0; i < cars.Length; i++)
        {
            if(cars[i] != null)
            {
                LeanTween.cancel(cars[i]);

                cars[i].GetComponent<CarMovement>().StopAllCoroutines();
                cars[i].GetComponent<CarMovement>().CarIdle();
            }
        }

        LeanTween.scale(popUpError, Vector3.one, 0.3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cars") && canDetectCollision)
        {
            currentCarInDestination++;

            other.GetComponent<CarMovement>().DisablePaticleEffect();
            
            if(currentCarInDestination == cars.Length && !isWin) 
            {
                isWin = true;
                Debug.Log("You Win & Show Report");

                ShowReport();
            }
            
            //Destroy(other.gameObject);
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

    public void ResetWithFixError()
    {
        LeanTween.scale(popUpError, Vector3.zero, 0.3f).setOnComplete(() =>
        {
            for(int i = 0; i < indicator.Length; i++) indicator[i].SetActive(true);

            isError = false;
            accidentalCar.enabled = false;
            Destroy(accidentCar);
            ResetPuzzle();
        });
    }

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}
