using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    private int difficultyIndex = 1;
    [SerializeField] private GameObject[] trafficManagementPuzzles;
    [SerializeField] private GameObject[] resourceAllocationPuzzles;
    [SerializeField] private GameObject[] wasteManagementPuzzles;
    [SerializeField] private GameObject[] energyManagementPuzzles;
    [SerializeField] private GameObject[] cyberSecurityPuzzles;
    [SerializeField] private GameObject[] needToPopUp;
    [SerializeField] private GameObject[] taskScreen;
    private int completedPuzzle;

   
    void Start()
    {
        ShowPopUp();
    }

    void Update()
    {
        
    }

    public void OpenTaskScreen(int index)
    {
        LeanTween.scale(needToPopUp[index + 1], Vector3.zero, 0.5f).setOnComplete(() =>
        {
            needToPopUp[index + 1].SetActive(false);
        });
        LeanTween.scale(taskScreen[index], Vector3.one, 0.5f);
    }

    public void CloseTaskScreen(int index)
    {
        needToPopUp[index + 1].SetActive(true);
        LeanTween.scale(needToPopUp[index + 1], Vector3.one, 0.5f);
        LeanTween.scale(taskScreen[index], Vector3.zero, 0.5f);
    }

    public void PlayTrafficManagementPuzzle()
    {
        
    }

    public void UpdateDifficultyIndex() => difficultyIndex++;

    public void AddCompletedPuzzle() 
    {
        completedPuzzle++;

        
    }

    public void ShowPopUp()
    {
        LeanTween.scale(needToPopUp[0], Vector2.one, 0.5f).setOnComplete(() =>
        {
            LeanTween.scale(needToPopUp[1], Vector2.one, 0.5f).setOnComplete(() =>
            {
                LeanTween.scale(needToPopUp[2], Vector2.one, 0.5f).setOnComplete(() =>
                {
                    LeanTween.scale(needToPopUp[3], Vector2.one, 0.5f).setOnComplete(() =>
                    {
                        LeanTween.scale(needToPopUp[4], Vector2.one, 0.5f);
                    });
                });
            });
        });
    }

    public int GetDifficultyIndex() 
    {
        return difficultyIndex;
    }
}
