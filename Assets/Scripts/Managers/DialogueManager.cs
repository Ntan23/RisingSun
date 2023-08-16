using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class DialogueManager : MonoBehaviour
{
    private bool canNext;
    private int dialogueIndex;
    [Header("For Dialogue")]
    [SerializeField] private GameObject dialogueScene;
    [SerializeField] private GameObject nextDialogueButton;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject arrowIndicator;
    [SerializeField] private string[] dialogues;
    [Header("For Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] videos;

    void Start() => ShowNextDialogue();
    
    public void NextDialogue()
    {
        if(dialogueIndex <= dialogues.Length) dialogueIndex++;

        if(dialogueIndex < dialogues.Length)
        {
            arrowIndicator.SetActive(false);

            if(dialogueIndex == 4) videoPlayer.clip = videos[0];
            if(dialogueIndex == 6) videoPlayer.clip = videos[1];
            if(dialogueIndex == 7) videoPlayer.clip = videos[0];
            if(dialogueIndex == 8) videoPlayer.clip = videos[1];
            if(dialogueIndex == 10) videoPlayer.clip = videos[0];
            if(dialogueIndex == 14) videoPlayer.clip = videos[1];
            if(dialogueIndex == 16) videoPlayer.clip = videos[0];

            ShowNextDialogue();
        }

        if(dialogueIndex == dialogues.Length)
        {
            videoPlayer.gameObject.SetActive(false);
            dialogueScene.SetActive(false);
            nextDialogueButton.SetActive(false);
        }
    }

    private void ShowNextDialogue()
    {
        dialogueText.text = dialogues[dialogueIndex];
        arrowIndicator.SetActive(true);
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
