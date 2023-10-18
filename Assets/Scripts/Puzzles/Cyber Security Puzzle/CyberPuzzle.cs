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
    [SerializeField] private float spawnRadius;
    [SerializeField] private int spawnCount;
    private int virusSpawnedCount;
    private float dataSpawnChances;
    private float randomValue;
    [SerializeField] private float intervalBetweenData;
    private float intialInterval;
    [SerializeField] private float dataSpeed;
    [SerializeField] private int virusThatNeedToBeKilled;
    private int killedVirus;
    private int unkillableSpawnedCount = 0;
    [SerializeField] private int virusHealth;
    private bool canSpawn = true;
    private GameObject data;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject dataPrefab;
    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject unkillableVirusPrefab;
    [SerializeField] private Transform objParent;
    [SerializeField] private TextMeshProUGUI virusKilledText;
    private Vector3 randomPos;
    [SerializeField] private Vector3 offset;
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
    
        IntialSpawn();
    }

    void Update()
    {
        // if(!canSpawn) return;

        // if(canSpawn) 
        // {
        //     Spawn();
        //     // intervalBetweenData -= Time.deltaTime; 

        //     // if(intervalBetweenData <= 0.0f)
        //     // {
        //     //     Spawn();
        //     //     intervalBetweenData = intialInterval;
        //     // }
        // }
    }

    private void IntialSpawn()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            if((i + 1) < spawnCount) Spawn();
            else if((i + 1) == spawnCount)
            {
                if(gm.GetDifficultyIndex() == 1)
                {
                    if(unkillableSpawnedCount == 0)  
                    {
                        randomPos = Random.insideUnitCircle * spawnRadius;

                        data = Instantiate(unkillableVirusPrefab, offset + randomPos, Quaternion.identity);
                        data.transform.parent = objParent;
                        unkillableSpawnedCount++;
                    }
                    else if(virusSpawnedCount == 0 && unkillableSpawnedCount > 0) 
                    {
                        randomPos = Random.insideUnitCircle * spawnRadius;

                        data = Instantiate(virusPrefab, offset + randomPos, Quaternion.identity);
                        data.transform.parent = objParent;
                        virusSpawnedCount++;
                    }
                    else if(unkillableSpawnedCount > 0 && virusSpawnedCount > 0) Spawn();
                }
            }
        }
    }

    private void Spawn()
    {
        randomValue = Random.value;
        randomPos = Random.insideUnitCircle * spawnRadius;

        if(randomValue <= dataSpawnChances) 
        {
            data = Instantiate(dataPrefab, offset + randomPos, Quaternion.identity);
            data.transform.parent = objParent;
        }
        else 
        {
            randomValue = Random.value;

            if(randomValue <= unkillableVirusSpawnChances)
            {
                data = Instantiate(unkillableVirusPrefab, offset + randomPos, Quaternion.identity);
                data.transform.parent = objParent;
                unkillableSpawnedCount++;
            }
            else
            {
                data = Instantiate(virusPrefab, offset + randomPos, Quaternion.identity);
                data.transform.parent = objParent;
                virusSpawnedCount++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + offset, spawnRadius);    
    }

    public void ResetLevel()
    {
        canSpawn = true;
        unkillableSpawnedCount = 0;
        virusSpawnedCount = 0;
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

        IntialSpawn();
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
