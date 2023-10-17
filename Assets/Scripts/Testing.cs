using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform arrow; 
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;

            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            arrow.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
