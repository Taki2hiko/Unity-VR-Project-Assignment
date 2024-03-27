using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollider : MonoBehaviour
{
    public Transform head;
    public Transform feet;

    private void Update()
    {
        gameObject.transform.position = new Vector3(head.position.x, feet.position.y, head.position.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
