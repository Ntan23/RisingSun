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

            LeanTween.cancel(imageToShow);
            StopAllCoroutines();
            
            if(gameObject.activeInHierarchy) StartCoroutine(AnimateLine());
        }
    }

    public void Unhover() 
    {
        gameObject.SetActive(false);
        imageToShow.SetActive(false);
        // LeanTween.cancel(imageToShow);
        // StopAllCoroutines(); 
        // StartCoroutine(AnimateLine(false));
    }

    private IEnumerator AnimateLine() 
    {
        yield return new WaitForEndOfFrame();

        // if(!isHover)
        // {
        //     LeanTween.cancel(imageToShow);
        //     LeanTween.scaleX(imageToShow, 0.0f, 0.15f).setOnComplete(() => imageToShow.SetActive(false));
        //     yield return new WaitForSeconds(0.25f);
        // }

        segmentDuration = animationDuration / pointsCount ; 

        for (int i = 0; i < pointsCount - 1; i++) 
        {
            float startTime = Time.time;

            // if(isHover)
            // {
            startPos = linePoints[i];
            endPos = linePoints[i+1];
            //}

            // if(!isHover)
            // {
            //     startPos = linePoints[pointsCount - (i + 1)];
            //     endPos = linePoints[pointsCount - (i + 2)];
            // }

            currentPos = startPos;

            while(currentPos != endPos) 
            {
                float t = (Time.time - startTime) / segmentDuration ;
                currentPos = Vector3.Lerp(startPos, endPos, t);

                /*if(isHover)*/ for (int j = i + 1; j < pointsCount; j++)
                lineRenderer.SetPosition(j, currentPos);

                //if(!isHover) for(int j = pointsCount - 1; j > 0; j--) lineRenderer.SetPosition(j, currentPos);

                yield return null;
            }
            
            if(endPos == linePoints[pointsCount - 1] /*&& isHover*/) 
            {
                imageToShow.SetActive(true);
                imageToShow.transform.localScale = new Vector3(0.0f, imageToShow.transform.localScale.y, imageToShow.transform.localScale.z);
                LeanTween.scaleX(imageToShow, 15.0f, 0.15f);
            }

            //if(endPos == linePoints[0] && !isHover) gameObject.SetActive(false);
        }
    }
}
