using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCursor : MonoBehaviour
{   
    [SerializeField] private float movementSpeed;
    public float boundaryPadding;
    private float minX, minY, maxX, maxY;
    private Vector3 mousePosition;
    private Vector3 worldMousePosition;
    private Vector3 movementDirection;
    private Vector3 newPosition;
    private GameObject virusGO;
    
    void Start()
    {
        minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + boundaryPadding;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - boundaryPadding;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + boundaryPadding;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - boundaryPadding;
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mousePosition.z));

        // Calculate the movement direction by subtracting the current object position from the mouse position
        movementDirection = new Vector3(-worldMousePosition.x, worldMousePosition.y, worldMousePosition.z) - transform.position;

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
                virusGO.GetComponent<Animator>().Play("VirusKilled");
                Destroy(virusGO, 0.3f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Virus") && other.GetComponent<Virus>().IsKillable())
        {
            virusGO = other.gameObject;

            Debug.Log(virusGO);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        virusGO = null;
    }
}