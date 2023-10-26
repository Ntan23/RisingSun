using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ResourcePuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Items
    {
        public string name;
        public int needed;
        public int current;
        public TextMeshProUGUI text;
    }
    // [SerializeField] private int piecesCount;
    // private int completedPieces;
    // private int randomIndex;
    // private int[] randomIndexes;
    // [SerializeField] private float totalTime;
    // private float initialTime;
    // private bool isTimeStart;
    [SerializeField] private Items[] items;
    private bool isFirstTime = true;
    private bool isComplete;
    private bool canSpawn = true;
    // private Vector3[] intialPiecesPosition;
    // [SerializeField] private ValueForRandomizer valuesForRandomizerSO;
    // [SerializeField] private List<PuzzleSlot> slotPrefabs;
    // private List<PuzzleSlot> randomSet;
    // [SerializeField] private PuzzlePiece piecePrefab;
    // [SerializeField] private Transform slotParent;
    // [SerializeField] private Transform pieceParent;
    // private GameObject[] pieces;
    // private PuzzleSlot spawnedSlot;
    // private PuzzlePiece spawnedPiece;
    // private PuzzlePiece[] spawnedPieces;
    [SerializeField] private Collider2D[] circleColliders;
    // [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject puzzleSpriteMask;
    [SerializeField] private GameObject[] objectThatNeedToDisable;
    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private Sprite bgSprite;
    [SerializeField] private Report report;
    [SerializeField] private ObjectSpawner[] objectSpawners;

    // void Awake()
    // {
    //     randomIndexes = new int[piecesCount];
    //     valuesForRandomizerSO.value = new int[piecesCount];
    //     valuesForRandomizerSO.isUsed = new bool[piecesCount];
    //     intialPiecesPosition = new Vector3[piecesCount];
    //     pieces = new GameObject[piecesCount];
    //     spawnedPieces = new PuzzlePiece[piecesCount];

    //     //gameObject.SetActive(false);
    // }

    void Start() 
    {
        // initialTime = totalTime;

        // for(int i = 0; i < piecesCount; i++) valuesForRandomizerSO.value[i] = i;

        for(int i = 0; i < items.Length; i++) UpdateObject(i);
        
        // StartCoroutine(Delay());
    }

    // IEnumerator Delay()
    // {
    //     yield return new WaitForSeconds(0.6f);
    //     Spawn();
    // }

    // public void StartTimer() 
    // {
    //     isTimeStart = true;
        
    //     StartCoroutine(Timer());
    // }
    
    // public void Spawn()
    // {
    //     Debug.Log("Spawn");
    //     Randomizer();

    //     randomSet = slotPrefabs.OrderBy(s => UnityEngine.Random.value).Take(piecesCount).ToList();

    //     for(int i = 0; i < randomSet.Count; i++)
    //     {
    //         spawnedPiece = Instantiate(piecePrefab, pieceParent.GetChild(i).position, Quaternion.identity);
    //         spawnedPiece.gameObject.transform.parent = pieceParent.GetChild(i);

    //         spawnedSlot = Instantiate(randomSet[i], slotParent.GetChild(randomIndexes[i]).position, Quaternion.identity);
    //         spawnedSlot.gameObject.transform.parent = slotParent.GetChild(randomIndexes[i]);

    //         intialPiecesPosition[i] = spawnedPiece.gameObject.transform.position;
    //         pieces[i] = spawnedPiece.gameObject;
    //         spawnedPieces[i] = spawnedPiece;

    //         spawnedPiece.Initialization(spawnedSlot);
    //     }

    //     //StartTimer();
    // }

    // private void Randomizer()
    // {
    //     for(int i = 0; i < piecesCount; i++)
    //     {
    //         randomIndex = UnityEngine.Random.Range(0, piecesCount);

    //         if(i == 0)
    //         {
    //             randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
    //             valuesForRandomizerSO.isUsed[randomIndex] = true;
    //         }
            
    //         if(i > 0)
    //         {
    //             if(!valuesForRandomizerSO.isUsed[randomIndex]) 
    //             {
    //                 randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
    //                 valuesForRandomizerSO.isUsed[randomIndex] = true;
    //                 continue;
    //             }

    //             while(valuesForRandomizerSO.isUsed[randomIndex]) 
    //             {
    //                 randomIndex = UnityEngine.Random.Range(0, piecesCount);

    //                 if(!valuesForRandomizerSO.isUsed[randomIndex]) 
    //                 {
    //                     randomIndexes[i] = valuesForRandomizerSO.value[randomIndex];
    //                     valuesForRandomizerSO.isUsed[randomIndex] = true;
    //                     break;
    //                 }
    //             }
    //         }
    //     }
    // }

    // IEnumerator Timer()
    // {
    //     while(isTimeStart)
    //     {
    //         totalTime -= Time.deltaTime;

    //         timerText.text = TimeSpan.FromSeconds(totalTime).ToString("mm':'ss");

    //         if(totalTime <= 0.0f) 
    //         {
    //             isTimeStart = false;
    //             Debug.Log("You Lose");
    //             ResetPuzzle();
    //         }

    //         yield return null;
    //     }
    // }

    public void UpdateObject(int index)
    {
        if(!isFirstTime) items[index].current++;
        if(isFirstTime && index == items.Length - 1) isFirstTime = false;

        if(items[index].needed > 0) 
        {
            if(items[index].current == items[index].needed) items[index].text.GetComponentInChildren<Image>().enabled = true;

            items[index].text.text = items[index].name + " " + items[index].current + " / " + items[index].needed;

        }
        else if(items[index].needed == 0) return;
    }

    public void ResetPuzzle()
    {
        if(!isComplete)
        {
            canSpawn = true;
            isFirstTime = true;

            foreach(Items item in items) 
            {
                item.current = 0;
                item.text.GetComponentInChildren<Image>().enabled = false;
            }

            for(int i = 0; i < items.Length; i++) UpdateObject(i);

            foreach(Collider2D collider in circleColliders) 
            {
                if(collider.enabled == false) collider.enabled = true;
                else if(collider.enabled == true) continue;
            }

            foreach(ObjectSpawner objectSpawner in objectSpawners) objectSpawner.ResetObjects();
            
            // completedPieces = 0;

            // for(int i = 0; i < piecesCount; i++)
            // {
            //     pieces[i].transform.position = intialPiecesPosition[i];
            //     spawnedPieces[i].SetBackCanBeDrag();
            // }

            // totalTime = initialTime;
            // isTimeStart = true;
        }
    }

    public void CheckLevel()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].current < items[i].needed) 
            {
                isComplete = false;
                break;
            }
            if(items[i].current >= items[i].needed && items[i].needed > 0)
            {
                if(i < items.Length - 1) continue;
                if(i == items.Length - 1) isComplete = true;
            }
        }
        // completedPieces++;
        Debug.Log(isComplete);

        if(isComplete)
        {
            Debug.Log("Show Report");
            //isTimeStart = false;

            LeanTween.rotateZ(puzzleSpriteMask, 90.0f, 0.5f);
            LeanTween.rotateZ(gameObject, 90.0f, 0.5f).setOnComplete(() =>
            {
                bgSpriteRenderer.sprite = bgSprite;
                
                foreach(GameObject go in objectThatNeedToDisable) go.SetActive(false);
            
                report.gameObject.SetActive(true);
                StartCoroutine(report.StartReport());
            });
        }
    }

    public void ChangeCanSpawnValue(bool value) => canSpawn = value; 

    // public int GetPiecesCount()
    // {
    //     return piecesCount;
    // }

    public bool GetCanSpawn() 
    {
        return canSpawn;
    }
}
