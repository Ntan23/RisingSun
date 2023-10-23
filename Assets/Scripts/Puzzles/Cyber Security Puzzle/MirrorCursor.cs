using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCursor : MonoBehaviour
{   
    private enum Type {
        horizontal, vertical
    }

    [SerializeField] private Type mirrorType;
    [SerializeField] private float movementSpeed;
    public float boundaryPadding;
    private float minX, minY, maxX, maxY;
    private int randomValue;
    private Vector3 mousePosition;
    private Vector3 worldMousePosition;
    private Vector3 movementDirection;
    private Vector3 newPosition;
    private GameObject virusGO;
    [SerializeField] private SpriteRenderer bg;
    [SerializeField] private Sprite[] bgSprites;
    private CyberPuzzle cp;
    
    void Start()
    {
        cp = GetComponentInParent<CyberPuzzle>();

        minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + boundaryPadding;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - boundaryPadding;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + boundaryPadding;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - boundaryPadding;
    
        ChangeMode();
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mousePosition.z));

        // Calculate the movement direction by subtracting the current object position from the mouse position
        if(mirrorType == Type.horizontal) movementDirection = new Vector3(-worldMousePosition.x, worldMousePosition.y, worldMousePosition.z) - transform.position;
        if(mirrorType == Type.vertical) movementDirection = new Vector3(worldMousePosition.x, -worldMousePosition.y, worldMousePosition.z) - transform.position;

        // Apply the opposite movement to the object to mirror the mouse's movement
        newPosition = transform.position + movementDirection * movementSpeed * Time.deltaTime;

        // Clamp the object's position within the screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = 0.0f;

        // Update the object's position
        transform.position = newPosition;

        if(virusGO != null && Input.GetMouseButtonDown(0)) 
        {
            if(virusGO.GetComponent<Animator>() != null) 
            {
                Debug.Log("Hit");
                virusGO.GetComponent<Animator>().Play("VirusKilled");
                Destroy(virusGO, 0.3f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Virus") && other.GetComponent<Virus>().IsKillable()) virusGO = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        virusGO = null;
    }

    public void ChangeMode()
    {
        randomValue = Random.Range(0,2);

        if(randomValue == 0) 
        {
            mirrorType = Type.horizontal; 
            bg.sprite = bgSprites[0];
        }
        else
        {
            mirrorType = Type.vertical;
            bg.sprite = bgSprites[1];
        }
    }
}
