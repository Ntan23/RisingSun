using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    private enum Type
    {
        horizontalUpper, horizontalLower, vertical, curved1, curved2, curved3, curved4, curved5, curved6
    }

    [SerializeField] private Type pipeType;
    private float[] rotations = {0, 90, 180, 270};
    private float intialRotation;
    [SerializeField] private float[] correctRotations;
    private float currentRotation;
    private bool isPlaced = false;
    private bool isRotating;
    private bool isFilled;
    private int possibleRotations;
    [SerializeField] private WastePuzzle wastePuzzle;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite filledSprite;
    private Sprite originalSprite;
    [Header("For Curved Pipes")]
    [SerializeField] private Sprite rotatedSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSprite = spriteRenderer.sprite;
    }

    private void Start()
    {
        possibleRotations = correctRotations.Length;
        int rand = Random.Range(0, rotations.Length);

        intialRotation = rotations[rand];
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);
        
        CheckRotation();
        
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
        if(!isRotating && !wastePuzzle.GetIsComplete())
        {
            isRotating = true;
            currentRotation = Mathf.Round(transform.eulerAngles.z);

            currentRotation += 90;

            LeanTween.rotateZ(gameObject, currentRotation, 0.3f).setOnComplete(() => 
            {
                isRotating = false;
                CheckRotation();

                if(possibleRotations > 1)
                {
                    if(currentRotation == correctRotations[0] || currentRotation == correctRotations[1] && !isPlaced)
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
                    if(currentRotation == correctRotations[0] && !isPlaced)
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

    public void CheckRotation()
    {
        isFilled = false;

        currentRotation = Mathf.Round(transform.eulerAngles.z);

        if(pipeType == Type.horizontalUpper || pipeType == Type.vertical)
        {
            spriteRenderer.sprite = originalSprite;

            if(currentRotation == 0 || currentRotation == 90) spriteRenderer.flipX = false;

            if(currentRotation == 180 || currentRotation == 270) spriteRenderer.flipX = true;
        }

        if(pipeType ==  Type.horizontalLower)
        {
            spriteRenderer.sprite = originalSprite;
            
            if(currentRotation == 0 || currentRotation == 270) spriteRenderer.flipX = false;

            if(currentRotation == 90 || currentRotation == 180) spriteRenderer.flipX = true;
        }

        if(pipeType == Type.curved1)
        {
            if(currentRotation == 0 || currentRotation == 270)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipX = true;
            }

            if(currentRotation == 90 || currentRotation == 180) 
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipX = false;
            }
        }

        if(pipeType == Type.curved2)
        {
            if(currentRotation == 270)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipX = true;
            }

            if(currentRotation == 0 || currentRotation == 90 || currentRotation == 180) 
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipX = false;
            }
        }

        if(pipeType == Type.curved3)
        {
            if(currentRotation == 90 || currentRotation == 180 || currentRotation == 270)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipY = true;
            }

            if(currentRotation == 0)
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipY = false;
            }
        }

        if(pipeType == Type.curved4)
        {
            if(currentRotation == 0 || currentRotation == 90)
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipY = false;
            }

            if(currentRotation == 180 || currentRotation == 270)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipY = true;
            }
        }

        if(pipeType == Type.curved5)
        {
            if(currentRotation == 0 || currentRotation == 90)
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipX = false;
            }

            if(currentRotation == 180 || currentRotation == 270)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipX = true;
            }
        }

        if(pipeType == Type.curved6)
        {
            if(currentRotation == 0 || currentRotation == 90)
            {
                spriteRenderer.sprite = rotatedSprite;
                spriteRenderer.flipX = true;
            }

            if(currentRotation == 180 || currentRotation == 270)
            {
                spriteRenderer.sprite = originalSprite;
                spriteRenderer.flipX = false;
            }
        }
    }

    public void ChangeToFilledSprite() 
    {
        spriteRenderer.sprite = filledSprite;
        isFilled = true;
    }

    public void ChangeToUnfilledSprite()
    {
        spriteRenderer.sprite = originalSprite;
        
        if(pipeType == Type.curved1 || pipeType == Type.curved2 || pipeType == Type.curved3 || pipeType == Type.curved4 || pipeType == Type.curved5 || pipeType == Type.curved6) CheckRotation();
        
        isFilled = false;
    }

    public void ResetPipe()
    {
        isPlaced = false;
        isFilled = false;

        LeanTween.rotateZ(gameObject, intialRotation, 0.0f);

        CheckRotation();
        
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

    public bool GetIsPlaced()
    {
        return isPlaced;
    }

    public bool GetIsFilled()
    {
        return isFilled;
    }

}
