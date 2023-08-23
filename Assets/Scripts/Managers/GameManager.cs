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
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateDifficultyIndex() => difficultyIndex++;

    public int GetDifficultyIndex() 
    {
        return difficultyIndex;
    }
}
