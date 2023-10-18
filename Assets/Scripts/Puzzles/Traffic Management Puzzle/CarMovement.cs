using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed;
    private Vector3 goalPos;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        goalPos = waypoints[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(index < waypoints.Count)
        {
            if(Vector2.Distance(transform.position, goalPos) > 0.0f ) 
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[index].position, speed * Time.deltaTime);
            }
            else if(Vector2.Distance(transform.position, goalPos) == 0.0f ) 
            {
                index++;
                if(index < waypoints.Count) goalPos = waypoints[index].position;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Right")) LeanTween.rotateZ(gameObject, 90.0f, 1.0f);
    }

    public void Reset()
    {
        index = 0;
        goalPos = waypoints[index].position;
    }
}
