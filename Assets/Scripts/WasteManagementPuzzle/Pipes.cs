using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    private float[] rotations = {0, 90, 180, 270};
    private float intialRotation;
    [SerializeField] private float[] correctRotations;
    private bool isPlaced = false;
    private bool isRotating;
    private int possibleRotations;
    [SerializeField] private WastePuzzle wastePuzzle;

    private void Start()
    {
        possibleRotations = correctRotations.Length;
        int rand = Random.Range(0, rotations.Length);

        intialRotation = rotations[rand];
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);
        
        if(possibleRotations > 1)
        {
            if(transform.eulerAngles.z == correctRotations[0] || transform.eulerAngles.z == correctRotations[1])
            {
                isPlaced = true;
                wastePuzzle.CorrectMove();
            }
        }
        else
        {
            if(transform.eulerAngles.z == correctRotations[0])
            {
                isPlaced = true;
                wastePuzzle.CorrectMove();
            }
        }
    }

    private void OnMouseDown()
    {
        if(!isRotating)
        {
            isRotating = true;
            LeanTween.rotateZ(gameObject, transform.eulerAngles.z + 90.0f, 0.3f).setOnComplete(() => 
            {
                isRotating = false;

                if(possibleRotations > 1)
                {
                    if(Mathf.Round(transform.eulerAngles.z) == correctRotations[0] || Mathf.Round(transform.eulerAngles.z) == correctRotations[1] && !isPlaced)
                    {
                        isPlaced = true;
                        wastePuzzle.CorrectMove();
                    }
                    else if(isPlaced)
                    {
                        isPlaced = false;
                        wastePuzzle.WrongMove();
                    }
                }
                else
                {
                    if(Mathf.Round(transform.eulerAngles.z) == correctRotations[0] && !isPlaced)
                    {
                        isPlaced = true;
                        wastePuzzle.CorrectMove();
                    }
                    else if(isPlaced)
                    {
                        isPlaced = false;
                        wastePuzzle.WrongMove();
                    }
                }
            });
        }
        //transform.Rotate(new Vector3(0, 0, 90));
    }
}
