using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    [SerializeField] private int healthCount;
    private CyberPuzzle cyberPuzzle;
    private DataMovement dataMovement;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;

        cyberPuzzle = transform.parent.parent.GetChild(gm.GetDifficultyIndex()).GetComponent<CyberPuzzle>();
        dataMovement = GetComponent<DataMovement>();

        healthCount = cyberPuzzle.GetVirusHealth();
    }

    void OnMouseDown()
    {
        healthCount--;

        if(healthCount == 0) 
        {
            healthCount = cyberPuzzle.GetVirusHealth();
            
            cyberPuzzle.UpdateKilledVirus();
            Destroy(gameObject);
        }
    }
}
