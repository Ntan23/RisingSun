using System.Collections;
using UnityEngine;

public class CarStopDetection : MonoBehaviour
{
    [SerializeField] private TrafficPuzzle tp;
    [SerializeField] private CarMovement cm;
    private bool canDetect = true;
    private bool isStop;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Cars") && tp.GetIsPlaying() && canDetect) 
        {
            isStop = true;
            cm.CarIdle();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(tp.GetIsPlaying() && isStop) 
        {
            StartCoroutine(Delay());
            cm.StartMove();
            isStop = false;
        }
    }

    IEnumerator Delay()
    {
        canDetect = false;
        yield return new WaitForSeconds(0.5f);
        canDetect = true;
    }
}
