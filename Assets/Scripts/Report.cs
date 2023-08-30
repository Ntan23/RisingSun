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

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartReport());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartReport()
    {
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

        scrollRect.enabled = true;
        LeanTween.scale(closeReportButton, Vector3.one, 0.5f).setEaseSpring();
    }

    public void CloseReport()
    {
        LeanTween.scale(screen, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            spriteMask.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        });
    }
}
