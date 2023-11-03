using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private List<Transform> CantTurnRightWaypoints;
    [SerializeField] private List<Transform> CantTurnLeftWaypoints;
    [SerializeField] private List<Transform> uTurnWaypoints;
    private Transform[] intialWaypoints;
    [SerializeField] private string targetWaypointName;
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
    private bool canDetectCollision;
    private TrafficPuzzle tp;
    private GameManager gm;
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start() => StartCoroutine(Delay());

    IEnumerator Delay()
    {
        gm = GameManager.instance;

        particles = GetComponentInChildren<ParticleSystem>();

        yield return new WaitForSeconds(0.6f);
        intialWaypoints = new Transform[waypoints.Count];

        goalPos = waypoints[index].position;
        tp = GetComponentInParent<TrafficPuzzle>();

        for(int i = 0; i < waypoints.Count; i++) intialWaypoints[i] = waypoints[i];
        
        initialCanTurnLeft = canTurnLeft;
        initialCanTurnRight = canTurnRight;
        initialCanUTurn = canUTurn;

        canDetectCollision = true;
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
        if(canDetectCollision)
        {
            if(other.CompareTag("Right") && canTurnRight) 
            {
                if(other.transform.parent.gameObject.name == targetWaypointName)
                {
                    canTurnRight = false;
                    LeanTween.rotateZ(gameObject, transform.eulerAngles.z - 90.0f, timeToTurnRight).setOnComplete(() => 
                        {
                            canTurnRight = true;
                        }
                    );
                }
                else if(other.transform.parent.gameObject.name == "Cant Turn Left Waypoint" || other.transform.parent.gameObject.name == "Cant Turn Right Waypoint")
                {
                    canTurnRight = false;
                    LeanTween.rotateZ(gameObject, transform.eulerAngles.z - 90.0f, timeToTurnRight).setOnComplete(() => 
                        {
                            canTurnRight = true;
                        }
                    );
                }
            }

            if(other.CompareTag("Left") && canTurnLeft) 
            {
                if(other.transform.parent.gameObject.name == targetWaypointName)
                {
                    canTurnLeft = false;
                    LeanTween.rotateZ(gameObject, transform.eulerAngles.z + 90.0f, timeToTurnLeft).setOnComplete(() => 
                        {
                            canTurnLeft = true;
                        }
                    );
                }
                else if(other.transform.parent.gameObject.name == "Cant Turn Left Waypoint" || other.transform.parent.gameObject.name == "Cant Turn Right Waypoint")
                {
                    canTurnRight = false;
                    LeanTween.rotateZ(gameObject, transform.eulerAngles.z - 90.0f, timeToTurnRight).setOnComplete(() => 
                        {
                            canTurnRight = true;
                        }
                    );
                }
            }

            if(other.CompareTag("UTurn") && canUTurn && other.transform.parent.gameObject.name == targetWaypointName) 
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
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("AccidentCar") && canDetectCollision) tp.ShowError();
        if(collisionInfo.gameObject.CompareTag("Cars") && canDetectCollision) 
        {
            if(particles != null) particles.Play();
            StartCoroutine(tp.Crash());
        }
    }

    public void ResetValues()
    {
        StopAllCoroutines();

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
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = CantTurnRightWaypoints[i];
        
        // canTurnLeft = true;
        // canTurnRight = false;
        // canUTurn = false;
    }

    public void ChangeToTurnRightWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = CantTurnLeftWaypoints[i];

        // canTurnLeft = false;
        // canTurnRight = true;
        // canUTurn = false;
    }

    public void ChangeToUTurnWaypoint()
    {
        for(int i = 0; i < waypoints.Count; i++) waypoints[i] = uTurnWaypoints[i];

        canTurnLeft = false;
        canTurnRight = false;
        canUTurn = true;
    }

    public void StartMove() => isStarted = true;

    public void DisablePaticleEffect() => particles.gameObject.SetActive(false);

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
