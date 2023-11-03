using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private float timeNeeded;
    [SerializeField] private GameObject skipCreditText;
    private bool canSkip;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && canSkip) SkipCredits();
    }

    public void StartCredits() 
    {
        StartCoroutine(SkipDelay());

        LeanTween.moveLocalY(gameObject, 1812.0f, timeNeeded).setOnComplete(() =>
        {
            Debug.Log("Quit");
            Application.Quit();
        });
    }

    public void SkipCredits()
    {
        LeanTween.cancel(this.gameObject);
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator SkipDelay()
    {
        yield return new WaitForSeconds(0.5f);
        skipCreditText.SetActive(true);
        canSkip = true;
    }
}
