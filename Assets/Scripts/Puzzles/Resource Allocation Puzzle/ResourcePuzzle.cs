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
    [SerializeField] private Vector3[] intialBoxPartPosition;
    private bool isFirstTime = true;
    private bool isComplete;
    private bool canSpawn = true;
    private bool isAnimating;
    private bool isOpen = false;
    private int currentOrderInLayer;
    [SerializeField] private Collider2D[] circleColliders;
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

    void Start() 
    {
        am = AudioManager.instance;

        for(int i = 0; i < items.Length; i++) UpdateObject(i);
        
        warningSignAnimator = warningSign.GetComponent<Animator>();

        intialBoxPartPosition = new Vector3[partOfTheBox.Length];

        for(int i = 0; i < partOfTheBox.Length; i++)
        {
            intialBoxPartPosition[i] = partOfTheBox[i].partGO.transform.localPosition;
        }
    }

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
        }
    }

    public void UpdateOrderInLayer() => currentOrderInLayer++;

    public void CheckLevel()
    {
        if(!isAnimating)
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
                        isAnimating = true;
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
                    }

                    if(phase == 2) 
                    {
                        isAnimating = true;

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
                                                if(!isOpen) StartCoroutine(Warning());
                                            });
                                        });
                                    });
                                });
                            });
                        });
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
                            isAnimating = true;
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
                        }
                        
                        if(phase == 2) 
                        {
                            isAnimating = true;
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
                                                    if(!isOpen) StartCoroutine(Warning());
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        }
                    }
                }
            }
        }
    }

    private void UpdateLeftAlpha(float alpha) => partOfTheBox[0].partGO.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.386f, 0.372f, alpha);

     private void UpdateBottomAlpha(float alpha) => partOfTheBox[3].partGO.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.386f, 0.372f, alpha);
    
    public void ChangeCanSpawnValue(bool value) => canSpawn = value; 

    public bool GetCanSpawn() 
    {
        return canSpawn;
    }

    IEnumerator Warning()
    {
        isOpen = true;
        phase = 1;
        canSpawn = false;
        warningSign.SetActive(true);
        //am.PlayWarningSFX();
        yield return new WaitForSeconds(0.1f);
        warningSignAnimator.Play("Error_WarningSign");
        yield return new WaitForSeconds(2.0f);
        warningSignAnimator.enabled = false;
        warningSign.SetActive(false);
        //am.StopWarningSFX();
        yield return new WaitForSeconds(0.1f);
        isOpen = false;
        am.PlayPopUpSFX();
        LeanTween.scale(fixErrorPopUp, Vector3.one, 0.3f);
    }

    public void ResetPuzzleWithFixError()
    {
        isAnimating = false;

        LeanTween.scale(fixErrorPopUp, Vector3.zero, 0.3f).setOnComplete(() =>
        {
            ResetDeliverBox();

            for(int i = 0; i < items.Length; i++) 
            {
                if(items[i].name == "Food") items[i].needed = 4;
                else if(items[i].name == "Water") items[i].needed = 5;
                else items[i].needed = 0;

                if(i == items.Length - 1) ResetPuzzle();
            }
        });
    }

    private void ResetDeliverBox()
    {
        box.transform.localPosition = Vector3.zero;
        boxLid.transform.localScale = Vector3.zero;
        
        partOfTheBox[0].partGO.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 0.0f);
        partOfTheBox[3].partGO.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f, 0.0f);

        for(int i = 0; i < partOfTheBox.Length; i++)
        {
            partOfTheBox[i].partGO.transform.localPosition = intialBoxPartPosition[i];
        }
    }

    public int GetOrderInLayer() 
    {
        return currentOrderInLayer;
    }
}
