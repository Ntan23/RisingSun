using UnityEngine;

public class CarStopDetection : MonoBehaviour
{
    [SerializeField] private TrafficPuzzle tp;
    [SerializeField] private CarMovement cm;
    private bool isStop;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cars") && tp.GetIsPlaying()) 
        {
            isStop = true;
            cm.CarIdle();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(tp.GetIsPlaying() && isStop) 
        {
            cm.StartMove();
            isStop = false;
        }
    }
}
