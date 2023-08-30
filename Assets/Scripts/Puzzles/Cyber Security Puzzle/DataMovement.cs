using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMovement : MonoBehaviour
{
    private int currentCornerPassed = 0;
    private int difficultyIndex;
    private float speed;
    private bool canMove;
    [SerializeField] private Transform[] cornerPositions;
    [SerializeField] private Transform targetPosition;
    private CyberPuzzle cyberPuzzle;
    private GameManager gm;
    
    void Start()
    {
        gm = GameManager.instance;

        difficultyIndex = gm.GetDifficultyIndex();

        cyberPuzzle = transform.parent.parent.GetChild(difficultyIndex).GetComponent<CyberPuzzle>();

        cornerPositions = new Transform[transform.parent.parent.GetChild(difficultyIndex).GetChild(3).childCount];
        targetPosition = transform.parent.parent.GetChild(difficultyIndex).GetChild(5);
        
        speed = cyberPuzzle.GetDataSpeed();

        for(int i = 0; i < transform.parent.parent.GetChild(difficultyIndex).GetChild(3).childCount ; i++)
        {
            cornerPositions[i] = transform.parent.parent.GetChild(difficultyIndex).GetChild(3).GetChild(i);
        }

        StartCoroutine(Delay());
    }

    void Update()
    {
        if(!canMove) return;

        if(canMove) Move();   
    }

    private void Move()
    {
        if(currentCornerPassed < cornerPositions.Length) 
        {
            transform.position = Vector2.MoveTowards(transform.position, cornerPositions[currentCornerPassed].position, speed * Time.deltaTime);
        
            if(Vector2.Distance(transform.position, cornerPositions[currentCornerPassed].position) < 0.01f) currentCornerPassed++;
        }

        if(currentCornerPassed == cornerPositions.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
            
            if(Vector2.Distance(transform.position, targetPosition.position) < 0.01f) 
            {
                Destroy(gameObject);

                if(gameObject.tag == "Virus") cyberPuzzle.ResetLevel();
                if(gameObject.tag == "UnkillableVirus") cyberPuzzle.ShowEndScreen();
            }
        }
    }

    public void ChangeCanMoveValue() => canMove = false;   
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }
}
