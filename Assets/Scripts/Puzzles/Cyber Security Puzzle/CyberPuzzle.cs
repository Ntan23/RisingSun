using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CyberPuzzle : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    [SerializeField] private float virusSpawnChances;
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
    [SerializeField] private Transform objParent;
    [SerializeField] private TextMeshProUGUI virusKilledText;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;

        dataSpawnChances = 1.0f - virusSpawnChances;
        intialInterval = intervalBetweenData;

        virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();
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
            data = Instantiate(virusPrefab, spawnPosition.position, Quaternion.identity);
            data.transform.parent = objParent;
        }
    }

    public void UpdateKilledVirus()
    {
        killedVirus++;

        virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();

        if(killedVirus == virusThatNeedToBeKilled) 
        {
            Debug.Log("You Win & Show Report");
            canSpawn = false;

            if(objParent.childCount > 0)
            {
                for(int i = 0; i < objParent.childCount; i++)
                {
                    Destroy(objParent.GetChild(i).gameObject);
                }
            }
            
            if(gm.GetDifficultyIndex() < 2) gm.UpdateDifficultyIndex(); 
        }
    }

    public void ResetLevel()
    {
        intervalBetweenData = intialInterval;
        killedVirus = 0;

        virusKilledText.text = killedVirus.ToString() + " / " + virusThatNeedToBeKilled.ToString();

        if(objParent.childCount > 0)
        {
            for(int i = 0; i < objParent.childCount; i++)
            {
                Destroy(objParent.GetChild(i).gameObject);
            }
        }
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
