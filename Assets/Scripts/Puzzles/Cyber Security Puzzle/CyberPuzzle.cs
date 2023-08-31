using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CyberPuzzle : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    [SerializeField] private float virusSpawnChances;
    [Range(0.0f,1.0f)]
    [SerializeField] private float unkillableVirusSpawnChances;
    private float dataSpawnChances;
    private float randomValue;
    [SerializeField] private float intervalBetweenData;
    private float intialInterval;
    [SerializeField] private float dataSpeed;
    [SerializeField] private int virusThatNeedToBeKilled;
    private int killedVirus;
    [SerializeField] private int virusHealth;
    private bool canSpawn = true;
    private GameObject data;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject dataPrefab;
    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject unkillableVirusPrefab;
    [SerializeField] private Transform objParent;
    [SerializeField] private TextMeshProUGUI virusKilledText;
    private GameManager gm;
    private DialogueManager dm;
    private AudioManager am;

    void Start()
    {
        gm = GameManager.instance;
        dm = DialogueManager.instance;
        am = AudioManager.instance;

        dataSpawnChances = 1.0f - virusSpawnChances;
        intialInterval = intervalBetweenData;

        if(gm.GetDifficultyIndex() == 2) virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();
    }

    void Update()
    {
        if(!canSpawn) return;

        if(canSpawn) 
        {
            intervalBetweenData -= Time.deltaTime; 

            if(intervalBetweenData <= 0.0f)
            {
                Spawn();
                intervalBetweenData = intialInterval;
            }
        }
    }

    private void Spawn()
    {
        randomValue = Random.value;

        if(randomValue <= dataSpawnChances) 
        {
            data = Instantiate(dataPrefab, spawnPosition.position, Quaternion.identity);
            data.transform.parent = objParent;
        }
        else 
        {
            randomValue = Random.value;

            if(randomValue <= unkillableVirusSpawnChances)
            {
                data = Instantiate(unkillableVirusPrefab, spawnPosition.position, Quaternion.identity);
                data.transform.parent = objParent;
            }
            else
            {
                data = Instantiate(virusPrefab, spawnPosition.position, Quaternion.identity);
                data.transform.parent = objParent;
            }
        }
    }

    public void ResetLevel()
    {
        intervalBetweenData = intialInterval;

        if(gm.GetDifficultyIndex() == 2)
        {
            killedVirus = 0;

            virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();
        }

        if(objParent.childCount > 0)
        {
            for(int i = 0; i < objParent.childCount; i++)
            {
                Destroy(objParent.GetChild(i).gameObject);
            }
        }
    }

    public void CheckKilledVirus()
    {
        Debug.Log("Kill");
        killedVirus++;
        virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();

        if(killedVirus == virusThatNeedToBeKilled)
        {
            Debug.Log("You Win");
            if(objParent.childCount > 0)
            {
                for(int i = 0; i < objParent.childCount; i++) Destroy(objParent.GetChild(i).gameObject); 
            }

            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
            {
                gameObject.SetActive(false);

                //gm.ShowPopUp();
                gm.ShowEndScreen();
            });

            canSpawn = false;
        }
    }

    public void ShowEndScreen()
    {
        if(objParent.childCount > 0)
        {
            for(int i = 0; i < objParent.childCount; i++) Destroy(objParent.GetChild(i).gameObject); 
        }
            
        if(gm.GetDifficultyIndex() < 2) gm.UpdateDifficultyIndex(); 

        gm.ShowError();

        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            gameObject.SetActive(false);

            //gm.ShowPopUp();
        });

        canSpawn = false;
    } 

    public int GetVirusHealth()
    {
        return virusHealth;
    }

    public float GetDataSpeed()
    {
        return dataSpeed;
    }
}
