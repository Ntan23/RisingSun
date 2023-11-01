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
    private class Items
    {
        public string name;
        public int needed;
        public int current;
        public TextMeshProUGUI text;
    }

    [System.Serializable]
    private class PartOfTheBox
    {
        public GameObject partGO;
        public Vector3 partDestination;
    }
    
    // [SerializeField] private int piecesCount;
    // private int completedPieces;
    // private int randomIndex;
    // private int[] randomIndexes;
    // [SerializeField] private float totalTime;
    // private float initialTime;
    // private bool isTimeStart;
    [SerializeField] private Items[] items;
    [SerializeField] private PartOfTheBox[] partOfTheBox;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject boxLid;
    [SerializeField] private Vector3 boxLidSize;
    private bool isFirstTime = true;
    private bool isComplete;
    private bool canSpawn = true;
    private int currentOrderInLayer;
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
    [SerializeField] private GameObject container;
    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private Sprite bgSprite;
    [SerializeField] private Report report;
    [SerializeField] private ObjectSpawner[] objectSpawners;
    [SerializeField] private int phase;
    [SerializeField] private GameObject warningSign;
    [SerializeField] private GameObject fixErrorPopUp;
    private Animator warningSignAnimator;
    private AudioManager am;

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

        am = AudioManager.instance;

        for(int i = 0; i < items.Length; i++) UpdateObject(i);
        
        warningSignAnimator = warningSign.GetComponent<Animator>();

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

        if(items[index].needed == 0) items[index].text.gameObject.SetActive(false);
        if(items[index].needed > 0) items[index].text.gameObject.SetActive(true);

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

    public void UpdateOrderInLayer() => currentOrderInLayer++;

    public void CheckLevel()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].current < items[i].needed) 
            {
                isComplete = false;
                ResetPuzzle();
                break;
            }

            if(items[i].needed == 0 && i < items.Length - 1) continue;
            if(items[i].needed == 0 && i == items.Length - 1) 
            {
                if(phase == 1) 
                {
                    LeanTween.value(partOfTheBox[0].partGO, UpdateLeftAlpha, 0.0f, 1.0f, 0.5f);
                    LeanTween.moveLocal(partOfTheBox[0].partGO, partOfTheBox[0].partDestination, 0.5f).setOnComplete(() =>
                    {
                        LeanTween.moveLocal(partOfTheBox[1].partGO, partOfTheBox[1].partDestination, 0.5f).setOnComplete(() =>
                        {
                            LeanTween.moveLocal(partOfTheBox[2].partGO, partOfTheBox[2].partDestination, 0.5f).setOnComplete(() =>
                            {
                                LeanTween.value(partOfTheBox[3].partGO, UpdateBottomAlpha, 0.0f, 1.0f, 0.5f);
                                LeanTween.moveLocal(partOfTheBox[3].partGO, partOfTheBox[3].partDestination, 0.5f).setOnComplete(() => 
                                {
                                    LeanTween.scale(boxLid, boxLidSize, 0.6f).setOnComplete(() =>
                                    {
                                        for(int i = 13; i < container.transform.childCount; i++) Destroy(container.transform.GetChild(i).gameObject);
                                        
                                        LeanTween.moveX(box, 8.5f, 0.6f).setOnComplete(() =>
                                        {
                                            isComplete = true;

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
                                        });
                                    });
                                });
                            });
                        });
                    });
                    //isComplete = true;
                }
                if(phase == 2) 
                {
                    LeanTween.value(partOfTheBox[0].partGO, UpdateLeftAlpha, 0.0f, 1.0f, 0.5f);
                    LeanTween.moveLocal(partOfTheBox[0].partGO, partOfTheBox[0].partDestination, 0.5f).setOnComplete(() =>
                    {
                        LeanTween.moveLocal(partOfTheBox[1].partGO, partOfTheBox[1].partDestination, 0.5f).setOnComplete(() =>
                        {
                            LeanTween.moveLocal(partOfTheBox[2].partGO, partOfTheBox[2].partDestination, 0.5f).setOnComplete(() =>
                            {
                                LeanTween.value(partOfTheBox[3].partGO, UpdateBottomAlpha, 0.0f, 1.0f, 0.5f);
                                LeanTween.moveLocal(partOfTheBox[3].partGO, partOfTheBox[3].partDestination, 0.5f).setOnComplete(() => 
                                {
                                    LeanTween.scale(boxLid, boxLidSize, 0.6f).setOnComplete(() =>
                                    {
                                        for(int i = 13; i < container.transform.childCount; i++) Destroy(container.transform.GetChild(i).gameObject);
                                        
                                        LeanTween.moveX(box, 8.5f, 0.6f).setOnComplete(() =>
                                        {
                                            StartCoroutine(Warning());
                                            phase = 1;
                                        });
                                    });
                                });
                            });
                        });
                    });
                    // StartCoroutine(Warning());
                    // phase = 1;
                }
                break;
            }

            if(items[i].current >= items[i].needed && items[i].needed > 0)
            {
                if(i < items.Length - 1) continue;
                if(i == items.Length - 1) 
                {
                    if(phase == 1) 
                    {
                        LeanTween.value(partOfTheBox[0].partGO, UpdateLeftAlpha, 0.0f, 1.0f, 0.5f);
                        LeanTween.moveLocal(partOfTheBox[0].partGO, partOfTheBox[0].partDestination, 0.5f).setOnComplete(() =>
                        {
                            LeanTween.moveLocal(partOfTheBox[1].partGO, partOfTheBox[1].partDestination, 0.5f).setOnComplete(() =>
                            {
                                LeanTween.moveLocal(partOfTheBox[2].partGO, partOfTheBox[2].partDestination, 0.5f).setOnComplete(() =>
                                {
                                    LeanTween.value(partOfTheBox[3].partGO, UpdateBottomAlpha, 0.0f, 1.0f, 0.5f);
                                    LeanTween.moveLocal(partOfTheBox[3].partGO, partOfTheBox[3].partDestination, 0.5f).setOnComplete(() => 
                                    {
                                        LeanTween.scale(boxLid, boxLidSize, 0.6f).setOnComplete(() =>
                                        {
                                            for(int i = 13; i < container.transform.childCount; i++) Destroy(container.transform.GetChild(i).gameObject);
                                            
                                            LeanTween.moveX(box, 8.5f, 0.6f).setOnComplete(() =>
                                            {
                                                isComplete = true;

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
                                            });
                                        });
                                    });
                                });
                            });
                        });
                        // isComplete = true;
                    }
                    if(phase == 2) 
                    {
                        StartCoroutine(Warning());
                        phase = 1;
                    }
                }
            }
        }
        // completedPieces++;
    }

    private void UpdateLeftAlpha(float alpha) => partOfTheBox[0].partGO.GetComponent<SpriteRenderer>().color = new Color(0.0117f, 0.694f, 0.792f, alpha);

     private void UpdateBottomAlpha(float alpha) => partOfTheBox[3].partGO.GetComponent<SpriteRenderer>().color = new Color(0.0117f, 0.694f, 0.792f, alpha);
    
    public void ChangeCanSpawnValue(bool value) => canSpawn = value; 

    // public int GetPiecesCount()
    // {
    //     return piecesCount;
    // }

    public bool GetCanSpawn() 
    {
        return canSpawn;
    }

    IEnumerator Warning()
    {
        canSpawn = false;
        warningSign.SetActive(true);
        am.PlayWarningSFX();
        yield return new WaitForSeconds(0.1f);
        warningSignAnimator.Play("Error_WarningSign");
        yield return new WaitForSeconds(2.0f);
        warningSignAnimator.enabled = false;
        warningSign.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        LeanTween.scale(fixErrorPopUp, Vector3.one, 0.3f);
    }

    public void ResetPuzzleWithFixError()
    {
        LeanTween.scale(fixErrorPopUp, Vector3.zero, 0.3f).setOnComplete(() =>
        {
            for(int i = 0; i < items.Length; i++) 
            {
                if(items[i].name == "Food") items[i].needed = 4;
                else if(items[i].name == "Water") items[i].needed = 5;
                else items[i].needed = 0;

                if(i == items.Length - 1) ResetPuzzle();
            }
        });
    }

    public int GetOrderInLayer() 
    {
        return currentOrderInLayer;
    }
}
