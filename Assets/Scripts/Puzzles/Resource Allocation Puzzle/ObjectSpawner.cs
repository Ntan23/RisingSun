using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private int maxSpawnCount;
    private int spawnedCount;
    [SerializeField] private GameObject objectToSpawn;
    private GameObject[] go;
    [SerializeField] private Transform spawnPosition;
    private ResourcePuzzle rp;
    
    void Start()
    {
        rp = GetComponentInParent<ResourcePuzzle>(); 

        go = new GameObject[maxSpawnCount];
    }   

    void OnMouseDown()
    {
        if(spawnedCount < maxSpawnCount && rp.GetCanSpawn()) 
        {
            go[spawnedCount] = Instantiate(objectToSpawn, spawnPosition.position, Quaternion.identity);
            go[spawnedCount].transform.parent = spawnPosition.parent;
            go[spawnedCount].GetComponent<ObjectController>().Initialization();

            spawnedCount++;
            rp.ChangeCanSpawnValue(false);
        }
    }

    public void DecreaseSpawnedCount() => spawnedCount--;

    public void ChangeBackCanSpawnValue() => rp.ChangeCanSpawnValue(true);

    public void ResetObjects()
    {
        spawnedCount = 0;

        for(int i = 0; i < maxSpawnCount; i++) if(go[i] != null) Destroy(go[i]);
    }
}
