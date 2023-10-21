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
    private Vector3 goalPos;
    private int index;
    [SerializeField] private float timeToTurnLeft;
    [SerializeField] private float timeToTurnRight;
    [SerializeField] private float timeToUTurn;
    [SerializeField] private float stopTime;
    [SerializeField] private bool canTurnLeft;
    [SerializeField] private bool canTurnRight;
    [SerializeField] private bool canUTurn;
    private bool initialCanTurnLeft;
    private bool initialCanTurnRight;
    private bool initialCanUTurn;
    private bool isStarted;
    private bool isStopBecauseOfStopSign;
    private TrafficPuzzle tp;

    // Start is called before the first frame update
    void Start()
    {
        intialWaypoints = new Transform[waypoints.Count];

        goalPos = waypoints[index].position;
        tp = GetComponentInParent<TrafficPuzzle>();

        for(int i = 0; i < waypoints.Count; i++) intialWaypoints[i] = waypoints[i];
        
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
                if(Vector2.Distance(transform.position, goalPos) > 0.0f) 
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[index].position, speed * Time.deltaTime);
                }
                else if(Vector2.Distance(transform.position, goalPos) == 0.0f) 
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

        if(other.CompareTag("Stop")) StartCoroutine(StopCar());
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Cars")) StartCoroutine(tp.Crash());
    }

    public void ResetValues()
    {
        for(int i = 0; i < waypoints.Count; i++) if(intialWaypoints[i] != null) waypoints[i] = intialWaypoints[i];

        CarIdle();
        index = 0;
        goalPos = waypoints[index].position;
        canTurnLeft = initialCanTurnLeft;
        canTurnRight = initialCanTurnRight;
        canUTurn = initialCanUTurn;

    }

    public void CarIdle() => isStarted = false;
    
    public void ChangeToTurnLeftWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = toLeftWapoints[i];

        canTurnLeft = true;
        canTurnRight = false;
        canUTurn = false;
    }

    public void ChangeToTurnRightWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = toRightWaypoints[i];

        canTurnLeft = false;
        canTurnRight = true;
        canUTurn = false;
    }

    public void ChangeToUTurnWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = uTurnWaypoints[i];

        canTurnLeft = false;
        canTurnRight = false;
        canUTurn = true;
    }

    public void StartMove() => isStarted = true;

    IEnumerator StopCar()
    {
        //isStopBecauseOfStopSign = true;
        CarIdle();
        yield return new WaitForSeconds(stopTime);
        StartMove();
        //isStopBecauseOfStopSign = false;
    }

    public bool GetIsStarted() 
    {
        return isStarted;
    }
}
