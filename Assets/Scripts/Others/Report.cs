using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Report : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private string reportText;
    [SerializeField] private TextMeshProUGUI reportTextUI;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject closeReportButton;
    [SerializeField] private GameObject skipInstruction;
    [SerializeField] private GameObject spriteMask;
    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject[] background;
    [SerializeField] private GameObject screenImage;
    [SerializeField] private GameObject gameScene;
    private GameManager gm;

    void Start() => gm = GameManager.instance;


    public IEnumerator StartReport()
    {
        foreach(GameObject go in background) go.SetActive(false);

        screenImage.SetActive(true);

        reportTextUI.text = "";
        scrollRect.enabled = false;
        yield return new WaitForSeconds(0.2f);

        skipInstruction.SetActive(true);

		foreach (char letter in reportText.ToCharArray())
		{
			reportTextUI.text += letter;

            if(Input.GetKeyDown(KeyCode.Return)) 
            {
                skipInstruction.SetActive(false);
                reportTextUI.text = reportText;
                break;
            }
		    yield return null;
		}

        skipInstruction.SetActive(false);
        scrollRect.enabled = true;
        LeanTween.scale(closeReportButton, Vector3.one, 0.5f).setEaseSpring();
    }

    public void CloseReport()
    {
        gm.ChangeBackBackground();
        LeanTween.scale(screen, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            spriteMask.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            gameObject.SetActive(false);

            gm.AddCompletedPuzzle();
            gm.ChangeBackCanBeClicked();
        });
    }
}
