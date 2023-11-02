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
    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private Color bgColor;
    [SerializeField] private GameObject[] warning;
    [SerializeField] private Animator[] warningAnimator;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject sytemUpdateText;
    [SerializeField] private GameObject systemResetText;
    private int completedPuzzle;
    private bool canBeClicked = true;
    private bool isShowingPopUp;
    private DialogueManager  dm;
    private AudioManager am;
    [SerializeField] private CreditsUI creditsUI;
   
    void Start()
    {
        //ShowPopUp();
        dm = DialogueManager.instance;
        am = AudioManager.instance;
    }

    public void OpenTaskScreen(int index)
    {
        if(canBeClicked)
        {
            isShowingPopUp = true;
            bgSpriteRenderer.color = bgColor;
            canBeClicked = false;
            LeanTween.scale(needToPopUp[index + 1], Vector3.zero, 0.5f).setOnComplete(() =>
            {
                needToPopUp[index + 1].SetActive(false);
            });
            LeanTween.scale(taskScreen[index], Vector3.one, 0.5f);
        }
    }

    public void CloseTaskScreen(int index)
    {
        ChangeBackBackground();
        canBeClicked = true;
        needToPopUp[index + 1].SetActive(true);
        LeanTween.scale(needToPopUp[index + 1], Vector3.one, 0.5f);
        LeanTween.scale(taskScreen[index], Vector3.zero, 0.5f).setOnComplete(() => isShowingPopUp = false);
    }

    public void PlayTrafficManagementPuzzle()
    {
        if(difficultyIndex == 1)
        {
            trafficManagementPuzzles[0].SetActive(true);
            LeanTween.scale(trafficManagementPuzzles[0], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[0], Vector3.zero, 0.5f);
        }

        if(difficultyIndex == 2)
        {
            trafficManagementPuzzles[1].SetActive(true);
            LeanTween.scale(trafficManagementPuzzles[1], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[0], Vector3.zero, 0.5f);
        }
    }

    public void PlayWasteManagementPuzzle()
    {
        if(difficultyIndex == 1)
        {
            wasteManagementPuzzles[0].SetActive(true);
            LeanTween.scale(wasteManagementPuzzles[0], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[1], Vector3.zero, 0.5f);
        }

        if(difficultyIndex == 2)
        {
            wasteManagementPuzzles[1].SetActive(true);
            LeanTween.scale(wasteManagementPuzzles[1], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[1], Vector3.zero, 0.5f);
        }
    }

    public void PlayEnergyManagementPuzzle()
    {
        if(difficultyIndex == 1)
        {
            energyManagementPuzzles[0].SetActive(true);
            LeanTween.scale(energyManagementPuzzles[0], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[2], Vector3.zero, 0.5f);
        }

        if(difficultyIndex == 2)
        {
            energyManagementPuzzles[1].SetActive(true);
            LeanTween.scale(energyManagementPuzzles[1], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[2], Vector3.zero, 0.5f);
        }
    }

    public void PlayResourceAllocationPuzzle()
    {
        if(difficultyIndex == 1)
        {
            resourceAllocationPuzzles[0].SetActive(true);
            LeanTween.scale(resourceAllocationPuzzles[0], Vector3.one, 0.5f);//.setOnComplete(() =>
            // {
            //     resourceAllocationPuzzles[0].GetComponent<ResourcePuzzle>().Spawn();
            // });
            LeanTween.scale(taskScreen[3], Vector3.zero, 0.5f);
        }

        if(difficultyIndex == 2)
        {
            resourceAllocationPuzzles[1].SetActive(true);
            LeanTween.scale(resourceAllocationPuzzles[1], Vector3.one, 0.5f);//.setOnComplete(() =>
            // {
            //     resourceAllocationPuzzles[1].GetComponent<ResourcePuzzle>().Spawn();
            // });
            LeanTween.scale(taskScreen[3], Vector3.zero, 0.5f);
        }
    }

    public void PlayCyberSecurityMinigame()
    {
        LeanTween.scale(needToPopUp[0], Vector3.zero, 0.5f);

        if(difficultyIndex == 1)
        {
            cyberSecurityPuzzles[0].SetActive(true);
            LeanTween.scale(cyberSecurityPuzzles[0], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[4], Vector3.zero, 0.5f);
        }

        if(difficultyIndex == 2)
        {
            cyberSecurityPuzzles[1].SetActive(true);
            LeanTween.scale(cyberSecurityPuzzles[1], Vector3.one, 0.5f);
            LeanTween.scale(taskScreen[4], Vector3.zero, 0.5f);
        }
    }

    public void UpdateDifficultyIndex() => difficultyIndex++;

    public void AddCompletedPuzzle() 
    {
        completedPuzzle++;

        if(completedPuzzle == 4) 
        {
            needToPopUp[5].SetActive(true);
            LeanTween.scale(needToPopUp[5], Vector3.one, 0.5f);
        }
    }

    public void ShowPopUp()
    {
        for(int i = 0; i < 5; i++) if(!needToPopUp[i].activeInHierarchy) needToPopUp[i].SetActive(true);

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

    public void ShowError() => StartCoroutine(Error());
    public void ShowEndScreen() => StartCoroutine(End());
    private void UpdateAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;

    private IEnumerator End()
    {
        blackScreen.SetActive(true);
        LeanTween.value(blackScreen, UpdateAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>{
            sytemUpdateText.SetActive(true);
            LeanTween.scale(sytemUpdateText, new Vector3(1.2f, 1.2f, 1.2f), 0.8f).setLoopPingPong();
        });
        yield return new WaitForSeconds(2.0f);
        sytemUpdateText.SetActive(false);
        systemResetText.SetActive(true);
        LeanTween.scale(systemResetText, new Vector3(1.2f, 1.2f, 1.2f), 0.8f).setLoopPingPong();
        yield return new WaitForSeconds(1.7f);
        systemResetText.gameObject.SetActive(false);

        Debug.Log("Quit");
        Application.Quit();
    }

    private IEnumerator Error()
    {
        completedPuzzle = 0;
        am.PlayWarningSFX();
        warning[0].SetActive(true);
        warning[1].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        warningAnimator[0].Play("Error");
        warningAnimator[1].Play("Error_WarningSign");
        yield return new WaitForSeconds(2.0f);
        am.StopWarningSFX();
        warningAnimator[0].enabled = false;
        warningAnimator[1].enabled = false;
        dm.PlayEndClip();
    }

    public void ChangeBackBackground() => bgSpriteRenderer.color = Color.white;
    public void ChangeBackCanBeClicked() => canBeClicked = true;
    public int GetDifficultyIndex() 
    {
        return difficultyIndex;
    }
    
    public bool GetIsShowingPopUp()
    {
        return isShowingPopUp;
    }
}
