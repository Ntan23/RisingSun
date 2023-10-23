using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    private enum Type 
    {
        killable, unkillable
    }

    [SerializeField] private Type virusType;
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

    private void CheckKilledVirus() => cyberPuzzle.CheckKilledVirus();

    // void OnMouseDown()
    // {
    //     if(virusType == Type.killable)
    //     {
    //         healthCount--;

    //         if(healthCount == 0) 
    //         {
    //             healthCount = cyberPuzzle.GetVirusHealth();

    //             dataMovement.ChangeCanMoveValue();
                
    //             GetComponent<Animator>().Play("VirusKilled");
    //             Destroy(gameObject, 0.3f);

    //             if(gm.GetDifficultyIndex() == 2) cyberPuzzle.CheckKilledVirus();
    //         }
    //     } 
    // }

    public bool IsKillable() 
    {
        return virusType == Type.killable;
    }
}
