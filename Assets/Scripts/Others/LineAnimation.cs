using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimation : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    private LineRenderer lineRenderer ;
    [SerializeField] private Vector3[] linePoints;
    private Vector3 startPos, endPos, currentPos;
    private int pointsCount ;
    private float segmentDuration;
    private float startTime;
    private bool isAnimating;
    [SerializeField] private GameObject imageToShow;
    private GameManager gm;

    private void Start () 
    {
        gm = GameManager.instance;

        lineRenderer = GetComponentInChildren<LineRenderer>();

        pointsCount = linePoints.Length;
    }

    public void Hover() 
    {
        if(!gm.GetIsShowingPopUp())
        {
            gameObject.SetActive(true);

            if(isAnimating) 
            {
                StopAllCoroutines();
                isAnimating = false;
            }
            
            if(gameObject.activeInHierarchy && !isAnimating) StartCoroutine(AnimateLine(true));
            else gameObject.SetActive(true);
        }
    }

    public void Unhover() 
    {
        if(isAnimating) 
        {
            StopAllCoroutines();  
            isAnimating = false;
        } 
        
        if(!isAnimating) StartCoroutine(AnimateLine(false));
    }

    private IEnumerator AnimateLine(bool isHover) 
    {
        isAnimating = true;

        if(!isHover)
        {
            LeanTween.cancel(imageToShow);
            LeanTween.scaleX(imageToShow, 0.0f, 0.15f).setOnComplete(() => imageToShow.SetActive(false));
            yield return new WaitForSeconds(0.25f);
        }

        gameObject.SetActive(true);
        segmentDuration = animationDuration / pointsCount ; 

        for (int i = 0; i < pointsCount - 1; i++) 
        {
            float startTime = Time.time;

            if(isHover)
            {
                startPos = linePoints[i];
                endPos = linePoints[i+1];
            }

            if(!isHover)
            {
                startPos = linePoints[pointsCount - (i + 1)];
                endPos = linePoints[pointsCount - (i + 2)];
            }

            currentPos = startPos;

            while(currentPos != endPos) 
            {
                float t = (Time.time - startTime) / segmentDuration ;
                currentPos = Vector3.Lerp(startPos, endPos, t);

                if(isHover) for (int j = i + 1; j < pointsCount; j++)
                lineRenderer.SetPosition(j, currentPos);

                if(!isHover) for(int j = pointsCount - 1; j > 0; j--) lineRenderer.SetPosition(j, currentPos);

                yield return null;
            }
            
            if(endPos == linePoints[pointsCount - 1] && isHover) 
            {
                isAnimating = false;
                imageToShow.SetActive(true);
                LeanTween.scaleX(imageToShow, 15.0f, 0.15f);
            }

            if(endPos == linePoints[0] && !isHover) 
            {
                gameObject.SetActive(false);
                isAnimating = false;
            }
        }
    }
}
