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
    [SerializeField] private GameObject[] imageToShow;
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

            for(int i = 0; i < imageToShow.Length; i++) LeanTween.cancel(imageToShow[i]);
            StopAllCoroutines();
            
            if(gameObject.activeInHierarchy) StartCoroutine(AnimateLine());
        }
    }

    public void Unhover() 
    {
        gameObject.SetActive(false);

        if(gm.GetDifficultyIndex() == 1) 
        {
            imageToShow[0].SetActive(false);
            imageToShow[0].GetComponent<Animator>().ResetTrigger("Activate");
        }
        if(gm.GetDifficultyIndex() == 2) 
        {
            imageToShow[1].SetActive(false);
            imageToShow[1].GetComponent<Animator>().ResetTrigger("Activate");
        }
        // LeanTween.cancel(imageToShow);
        // StopAllCoroutines(); 
        // StartCoroutine(AnimateLine(false));
    }

    private IEnumerator AnimateLine() 
    {
        yield return new WaitForEndOfFrame();

        segmentDuration = animationDuration / pointsCount ; 

        for (int i = 0; i < pointsCount - 1; i++) 
        {
            float startTime = Time.time;

            startPos = linePoints[i];
            endPos = linePoints[i+1];

            currentPos = startPos;

            while(currentPos != endPos) 
            {
                float t = (Time.time - startTime) / segmentDuration ;
                currentPos = Vector3.Lerp(startPos, endPos, t);

                for (int j = i + 1; j < pointsCount; j++)
                lineRenderer.SetPosition(j, currentPos);

                yield return null;
            }
            
            if(endPos == linePoints[pointsCount - 1]) 
            {
                if(gm.GetDifficultyIndex() == 1) 
                {
                    imageToShow[0].SetActive(false);
                    imageToShow[0].SetActive(true);
                    imageToShow[0].transform.localScale = new Vector3(0.0f, imageToShow[0].transform.localScale.y, imageToShow[0].transform.localScale.z);
                    LeanTween.scaleX(imageToShow[0], 0.9f, 0.15f).setOnComplete(() =>
                    {
                        imageToShow[0].GetComponent<Animator>().SetTrigger("Activate");
                    });
                }

                if(gm.GetDifficultyIndex() == 2) 
                {
                    imageToShow[0].SetActive(false);
                    imageToShow[1].SetActive(true);
                    imageToShow[1].transform.localScale = new Vector3(0.0f, imageToShow[1].transform.localScale.y, imageToShow[1].transform.localScale.z);
                    LeanTween.scaleX(imageToShow[1], 0.9f, 0.15f).setOnComplete(() =>
                    {
                        imageToShow[1].GetComponent<Animator>().SetTrigger("Activate");
                    });
                }
            }

            //if(endPos == linePoints[0] && !isHover) gameObject.SetActive(false);
        }
    }
}
