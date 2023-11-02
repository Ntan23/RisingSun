using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsUI : MonoBehaviour
{
    private enum Type{
        Traffic, Energy, Waste, Resource, Cyber
    }   

    [SerializeField] private Type tutorialFor;
    [SerializeField] private GameObject[] tutorials;
    [SerializeField] private GameObject previousButtons;
    [SerializeField] private GameObject skipButton;
    [Header("For Waste Puzzle Only")]
    [SerializeField] private WastePuzzle wastePuzzle;
    private int currentIndex;
    private bool canSkip;
    
    void Start()
    {
        if(tutorialFor == Type.Traffic) canSkip = PlayerPrefs.GetInt("TrafficTutorSkipIndicator", 0) == 0 ? false : true;

        if(tutorialFor == Type.Energy) canSkip = PlayerPrefs.GetInt("EnergyTutorSkipIndicator", 0) == 0 ? false : true;

        if(tutorialFor == Type.Waste) canSkip = PlayerPrefs.GetInt("WasteTutorSkipIndicator", 0) == 0 ? false : true;

        if(tutorialFor == Type.Resource) canSkip = PlayerPrefs.GetInt("ResourceTutorSkipIndicator", 0) == 0 ? false : true;

        if(tutorialFor == Type.Cyber) canSkip = PlayerPrefs.GetInt("CyberTutorSkipIndicator", 0) == 0 ? false : true;

        if(canSkip) skipButton.SetActive(true);
    }

    public void Next()
    {
        if(currentIndex == tutorials.Length - 1) 
        {
            if(tutorialFor == Type.Traffic) PlayerPrefs.SetInt("TrafficTutorSkipIndicator", 1);

            if(tutorialFor == Type.Energy) PlayerPrefs.SetInt("EnergyTutorSkipIndicator", 1);

            if(tutorialFor == Type.Waste) 
            {
                PlayerPrefs.SetInt("WasteTutorSkipIndicator", 1);
                wastePuzzle.StartTimer();
            }

            if(tutorialFor == Type.Resource) PlayerPrefs.SetInt("ResourceTutorSkipIndicator", 1);

            if(tutorialFor == Type.Cyber) PlayerPrefs.SetInt("CyberTutorSkipIndicator", 1);

            gameObject.SetActive(false); 
        }
        
        if(currentIndex < tutorials.Length - 1) 
        {
            LeanTween.moveLocalX(tutorials[currentIndex], -1920.0f, 0.5f);
            currentIndex++;

            if(currentIndex > 0) previousButtons.SetActive(true);
        }

    }

    public void Previous()
    {
        currentIndex--;

        if(currentIndex == 0) previousButtons.SetActive(false);
        
        LeanTween.moveLocalX(tutorials[currentIndex], 0.0f, 0.5f);
    }

    public void SkipTutorial() 
    {
        if(tutorialFor == Type.Waste) wastePuzzle.StartTimer();

        gameObject.SetActive(false);
    }
}
