using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private List<Transform> toLeftWapoints;
    [SerializeField] private List<Transform> toRightWaypoints;
    [SerializeField] private List<Transform> uTurnWaypoints;
    private Transform[] intialWaypoints;
    [SerializeField] private float speed;
    private float intialSpeed;
    private Vector3 goalPos;
    private int index;
    [SerializeField] private float timeToTurnLeft;
    [SerializeField] private float timeToTurnRight;
    [SerializeField] private float timeToUTurn;
    [SerializeField] private bool canTurnLeft;
    [SerializeField] private bool canTurnRight;
    [SerializeField] private bool canUTurn;
    private bool initialCanTurnLeft;
    private bool initialCanTurnRight;
    private bool initialCanUTurn;
    private bool isStarted;
    private TrafficPuzzle tp;

    // Start is called before the first frame update
    void Start()
    {
        intialWaypoints = new Transform[waypoints.Count];

        goalPos = waypoints[index].position;
        tp = GetComponentInParent<TrafficPuzzle>();

        for(int i = 0; i < waypoints.Count; i++) intialWaypoints[i] = waypoints[i];
        
        intialSpeed = speed;
        initialCanTurnLeft = canTurnLeft;
        initialCanTurnRight = canTurnRight;
        initialCanUTurn = canUTurn;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted)
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Right") && canTurnRight) 
        {
            canTurnRight = false;
            LeanTween.rotateZ(gameObject, transform.eulerAngles.z - 90.0f, timeToTurnRight).setOnComplete(() => 
                {
                    canTurnRight = true;
                }
            );
        }

        if(other.CompareTag("Left") && canTurnLeft) 
        {
            canTurnLeft = false;
            LeanTween.rotateZ(gameObject, transform.eulerAngles.z + 90.0f, timeToTurnLeft).setOnComplete(() => 
                {
                    canTurnLeft = true;
                }
            );
        }

        if(other.CompareTag("UTurn") && canUTurn) 
        {
            canUTurn = false;
            LeanTween.rotateZ(gameObject, transform.eulerAngles.z + 180.0f, timeToUTurn).setOnComplete(() => 
                {
                    canUTurn = true;
                }
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Cars")) StartCoroutine(Crash());
    }

    public void ResetValues()
    {
        index = 0;
        goalPos = waypoints[index].position;
        speed = intialSpeed;
        canTurnLeft = initialCanTurnLeft;
        canTurnRight = initialCanTurnRight;
        canUTurn = initialCanUTurn;

        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = intialWaypoints[i];
    }

    public void ChangeToTurnLeftWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = toLeftWapoints[i];

        canTurnLeft = true;
        canTurnRight = false;
        canTurnLeft = false;
    }

    public void ChangeToTurnRightWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = toRightWaypoints[i];

        canTurnLeft = false;
        canTurnRight = true;
        canTurnLeft = false;
    }

    public void ChangeToUTurnWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = uTurnWaypoints[i];

        canTurnLeft = false;
        canTurnRight = true;
        canTurnLeft = false;
    }

    public void StartMove()
    {
        isStarted = true;
        speed = intialSpeed;
    }

    IEnumerator Crash()
    {
        LeanTween.cancel(this.gameObject);

        speed = 0.0f;
        isStarted = false;
        yield return new WaitForSeconds(0.8f);
        tp.ResetPuzzle();
    }
}
