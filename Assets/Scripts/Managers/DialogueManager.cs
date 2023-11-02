using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    private bool canSkip;
    private int dialogueIndex;
    [Header("For Dialogue")]
    [SerializeField] private GameObject dialogueScene;
    [SerializeField] private GameObject nextDialogueButton;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject arrowIndicator;
    [SerializeField] private string[] dialogues;
    [Header("For Video")]
    private bool alreadyPlay;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videos;
    [SerializeField] private VideoClip endClip;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject skipText;
    [SerializeField] private AudioSource audioSource;
    private GameManager gm;
    private AudioManager am;

    void Start() 
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        canSkip = PlayerPrefs.GetInt("CanSkip", 0) == 0 ? false : true;

        if(canSkip) skipText.SetActive(true);
        else if(!canSkip) skipText.SetActive(false);

        ShowNextDialogue();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && canSkip && !alreadyPlay) 
        {
            dialogueIndex = dialogues.Length - 1;
            TurnOffDialogue();
        }
    }
    
    public void NextDialogue()
    {
        if(dialogueIndex <= dialogues.Length) dialogueIndex++;

        if(dialogueIndex < dialogues.Length)
        {
            arrowIndicator.SetActive(false);

            if(dialogueIndex == 4) videoPlayer.clip = videos[0];
            if(dialogueIndex == 6) videoPlayer.clip = videos[1];
            if(dialogueIndex == 7) videoPlayer.clip = videos[2];
            if(dialogueIndex == 8) videoPlayer.clip = videos[3];
            if(dialogueIndex == 10) videoPlayer.clip = videos[4];
            if(dialogueIndex == 14) videoPlayer.clip = videos[5];
            if(dialogueIndex == 16) videoPlayer.clip = videos[6];

            ShowNextDialogue();
        }

        if(dialogueIndex == dialogues.Length) TurnOffDialogue();
    }

    private void ShowNextDialogue()
    {
        dialogueText.text = dialogues[dialogueIndex];
        arrowIndicator.SetActive(true);
    }

    private void TurnOffDialogue()
    {
        alreadyPlay = true;
        videoPlayer.gameObject.SetActive(false);
        dialogueScene.SetActive(false);
        nextDialogueButton.SetActive(false);
        gm.ShowPopUp();

        if(!canSkip) PlayerPrefs.SetInt("CanSkip", 1);
        else if(canSkip) skipText.SetActive(false);
    }

    public void PlayEndClip()
    {
        am.StopBGM1();
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = endClip;
        am.PlayGlitchCutsceneAudio();
        videoPlayer.Play();
        videoPlayer.loopPointReached += CheckOver;
    }

    private void UpdateAlpha(float alpha) => blackScreen.GetComponent<CanvasGroup>().alpha = alpha;
    private void CheckOver(VideoPlayer source)
    {
        videoPlayer.gameObject.SetActive(false);
        am.StopGlitchCutsceneAudio();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        am.PlayBGM2();
        blackScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(blackScreen, UpdateAlpha, 0.0f, 1.0f, 0.3f);
        yield return new WaitForSeconds(1.5f);
        overlay.SetActive(true);
        LeanTween.value(blackScreen, UpdateAlpha, 1.0f, 0.0f, 1.0f).setOnComplete(() =>
        {
            blackScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
            gm.ShowPopUp();
            gm.ChangeBackCanBeClicked();
        });
    }
    // IEnumerator TypeText(string sentence)
    // {
    // 	dialogueText.text = "";
    // 	foreach (char letter in sentence.ToCharArray())
    // 	{
    // 		dialogueText.text += letter;
    // 		yield return new WaitForSeconds(0.02f);
    // 	}

    //     arrowIndicator.SetActive(true);
    //     canNext = true;
    // }
}
