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
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color32 inactiveColor;
    private Color32 intialColor;
    private ResourcePuzzle rp;
    
    void Start()
    {
        rp = GetComponentInParent<ResourcePuzzle>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();

        go = new GameObject[maxSpawnCount];
        
        intialColor = spriteRenderer.color;
    }   

    void OnMouseDown()
    {
        if(spawnedCount < maxSpawnCount && rp.GetCanSpawn()) 
        {
            go[spawnedCount] = Instantiate(objectToSpawn, spawnPosition.position, Quaternion.identity);
            go[spawnedCount].transform.parent = spawnPosition.parent;
            go[spawnedCount].GetComponent<ObjectController>().Initialization();

            if(go[spawnedCount].transform.childCount > 1) go[spawnedCount].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = rp.GetOrderInLayer();
            if(go[spawnedCount].transform.childCount == 1) go[spawnedCount].GetComponent<SpriteRenderer>().sortingOrder = rp.GetOrderInLayer();

            spawnedCount++;
            rp.UpdateOrderInLayer();
            rp.ChangeCanSpawnValue(false);
        }

        if(spawnedCount == maxSpawnCount) spriteRenderer.color = inactiveColor;
    }

    public void DecreaseSpawnedCount() 
    {
        spawnedCount--;
        spriteRenderer.color = intialColor;
    }

    public void ChangeBackCanSpawnValue() => rp.ChangeCanSpawnValue(true);

    public void ResetObjects()
    {
        spawnedCount = 0;
        spriteRenderer.color = intialColor;

        for(int i = 0; i < maxSpawnCount; i++) 
        {
            if(go[i] != null) Destroy(go[i]);
            else continue;
        }
    }

    public Transform GetSpawnPosition()
    {
        return spawnPosition;
    }
}
