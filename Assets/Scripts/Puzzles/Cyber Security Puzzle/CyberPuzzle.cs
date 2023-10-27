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
    // [SerializeField] private float intervalBetweenData;
    // private float intialInterval;
    [SerializeField] private float dataSpeed;
    // [SerializeField] private int virusThatNeedToBeKilled;
    private int killedVirus;
    private int unkillableSpawnedCount = 0;
    [SerializeField] private int maxWave;
    private int currentWave;
    //[SerializeField] private int virusHealth;
    private GameObject data;
    //[SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject dataPrefab;
    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject unkillableVirusPrefab;
    [SerializeField] private Transform objParent;
    [SerializeField] private TextMeshProUGUI wavesText;
    private Vector3 randomPos;
    [SerializeField] private Vector3 offset;
    private GameManager gm;
    private DialogueManager dm;
    private AudioManager am;
    [SerializeField] private MirrorCursor mc;

    void Start()
    {
        gm = GameManager.instance;
        dm = DialogueManager.instance;
        am = AudioManager.instance;

        dataSpawnChances = 1.0f - virusSpawnChances;
        //intialInterval = intervalBetweenData;
        currentWave = 1;

        wavesText.text = currentWave.ToString() + " / " + maxWave.ToString();

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
        virusSpawnedCount = 0;
        unkillableSpawnedCount = 0;

        for(int i = 0; i < spawnCount; i++)
        {
            if(currentWave == maxWave && gm.GetDifficultyIndex() == 1)
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
            else
            {
                unkillableVirusSpawnChances = 0.0f;
                Spawn();
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
        currentWave = 1;
        unkillableSpawnedCount = 0;
        virusSpawnedCount = 0;
        // intervalBetweenData = intialInterval;

        killedVirus = 0;

        wavesText.text = currentWave.ToString() + " / " + maxWave.ToString();

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
        // wavesText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();

        if(killedVirus == virusSpawnedCount)
        {
            killedVirus = 0;
            Debug.Log("You Win");

            if(objParent.childCount > 0)
            {
                for(int i = 0; i < objParent.childCount; i++) Destroy(objParent.GetChild(i).gameObject); 
            }

            if(currentWave == maxWave)
            {
                Debug.Log(gm.GetDifficultyIndex());
                if(gm.GetDifficultyIndex() == 2)
                {
                    LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
                    {
                        gameObject.SetActive(false);

                        //gm.ShowPopUp();
                        gm.ShowEndScreen();
                    });
                }

                if(gm.GetDifficultyIndex() == 1) ShowEndScreen();
            }

            if(currentWave < maxWave) UpdateWave();
        }
    }

    public void ShowEndScreen()
    {
        if(objParent.childCount > 0)
        {
            for(int i = 0; i < objParent.childCount; i++) Destroy(objParent.GetChild(i).gameObject); 
        }
            
        if(gm.GetDifficultyIndex() == 1) gm.UpdateDifficultyIndex(); 

        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            gameObject.SetActive(false);

            gm.ShowError();
            //gm.ShowPopUp();
        });
    } 

    public void UpdateWave()
    {
        if(currentWave < maxWave)
        {
            currentWave++;
            
            wavesText.text = currentWave.ToString() + " / " + maxWave.ToString();
            mc.ChangeMode();

            if(objParent.childCount > 0)
            {
                for(int i = 0; i < objParent.childCount; i++) Destroy(objParent.GetChild(i).gameObject);
            }

            IntialSpawn();
        }
    }

    // public int GetVirusHealth()
    // {
    //     return virusHealth;
    // }

    public float GetDataSpeed()
    {
        return dataSpeed;
    }

    public int GetVirusCount()
    {
        return virusSpawnedCount;
    }
}
