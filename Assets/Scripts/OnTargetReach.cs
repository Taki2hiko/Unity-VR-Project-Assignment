using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReach : MonoBehaviour
{
    public float threshold = 0.02f;
    public Transform target;
    public UnityEvent onReach;
    public bool isReach = false;


    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < threshold && !isReach) 
        {
            onReach.Invoke();
            isReach = true;
        }
        else if (distance >= threshold)
        {
            isReach = false;
        }
    }
}
