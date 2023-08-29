using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Report : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private string reportText;
    [SerializeField] private TextMeshProUGUI reportTextUI;
    [SerializeField] private GameObject closeReportButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartReport());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartReport()
    {
        reportTextUI.text = "";

		foreach (char letter in reportText.ToCharArray())
		{
			reportTextUI.text += letter;
		    yield return null;
		}

        LeanTween.scale(closeReportButton, Vector3.one, 0.5f).setEaseSpring();
    }
}
