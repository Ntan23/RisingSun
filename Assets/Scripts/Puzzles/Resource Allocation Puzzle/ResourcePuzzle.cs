using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class ResourcePuzzle : MonoBehaviour
{
    [SerializeField] private int piecesCount;
    private int completedPieces;
    private int randomIndex;
    private int[] randomIndexes;
    [SerializeField] private float totalTime;
    private float initialTime;
    private bool isTimeStart;
    private Vector3[] intialPiecesPosition;
    [SerializeField] private ValueForRandomizer valuesForRandomizerSO;
    [SerializeField] private List<PuzzleSlot> slotPrefabs;
    private List<PuzzleSlot> randomSet;
    [SerializeField] private PuzzlePiece piecePrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private Transform pieceParent;
    private GameObject[] pieces;
    private PuzzleSlot spawnedSlot;
    private PuzzlePiece spawnedPiece;
    private PuzzlePiece[] spawnedPieces;
    [SerializeField] private TextMeshProUGUI timerText;

    void Awake()
    {
        randomIndexes = new int[piecesCount];
        valuesForRandomizerSO.value = new int[piecesCount];
        valuesForRandomizerSO.isUsed = new bool[piecesCount];
        intialPiecesPosition = new Vector3[piecesCount];
        pieces = new GameObject[piecesCount];
        spawnedPieces = new PuzzlePiece[piecesCount];
    }

    void Start() 
    {
        initialTime = totalTime;

        for(int i = 0; i < piecesCount; i++) valuesForRandomizerSO.value[i] = i;
        
        Spawn();
    }

    public void StartTimer() 
    {
        isTimeStart = true;
        
        StartCoroutine(Timer());
    }
    
    private void Spawn()
    {
        Randomizer();

        randomSet = slotPrefabs.OrderBy(s => UnityEngine.Random.value).Take(piecesCount).ToList();

        for(int i = 0; i < randomSet.Count; i++)
        {
            spawnedPiece = Instantiate(piecePrefab, pieceParent.GetChild(i).position, Quaternion.identity);
            spawnedPiece.gameObject.transform.parent = pieceParent.GetChild(i);

            spawnedSlot = Instantiate(randomSet[i], slotParent.GetChild(randomIndexes[i]).position, Quaternion.identity);
            spawnedSlot.gameObject.transform.parent = slotParent.GetChild(randomIndexes[i]);

            intialPiecesPosition[i] = spawnedPiece.gameObject.transform.position;
            pieces[i] = spawnedPiece.gameObject;
            spawnedPieces[i] = spawnedPiece;

            spawnedPiece.Initialization(spawnedSlot);
        }

        StartTimer();
    }

    private void Randomizer()
    {
        for(int i = 0; i < piecesCount; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, piecesCount);

            if(i == 0)
            {
                randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
                valuesForRandomizerSO.isUsed[randomIndex] = true;
            }
            
            if(i > 0)
            {
                if(!valuesForRandomizerSO.isUsed[randomIndex]) 
                {
                    randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
                    valuesForRandomizerSO.isUsed[randomIndex] = true;
                    continue;
                }

                while(valuesForRandomizerSO.isUsed[randomIndex]) 
                {
                    randomIndex = UnityEngine.Random.Range(0, piecesCount);

                    if(!valuesForRandomizerSO.isUsed[randomIndex]) 
                    {
                        randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
                        valuesForRandomizerSO.isUsed[randomIndex] = true;
                        break;
                    }
                }
            }
        }
    }

    IEnumerator Timer()
    {
        while(isTimeStart)
        {
            totalTime -= Time.deltaTime;

            timerText.text = TimeSpan.FromSeconds(totalTime).ToString("mm':'ss");

            if(totalTime <= 0.0f) 
            {
                isTimeStart = false;
                Debug.Log("You Lose");
                ResetPuzzle();
            }

            yield return null;
        }
    }

    private void ResetPuzzle()
    {
        for(int i = 0; i < piecesCount; i++)
        {
            pieces[i].transform.position = intialPiecesPosition[i];
            spawnedPieces[i].SetBackCanBeDrag();
        }

        totalTime = initialTime;
        isTimeStart = true;
    }

    public void CheckLevel()
    {
        completedPieces++;

        if(completedPieces == piecesCount)
        {
            Debug.Log("Show Report");
            isTimeStart = false;
        }
    }

    public int GetPiecesCount()
    {
        return piecesCount;
    }
}
