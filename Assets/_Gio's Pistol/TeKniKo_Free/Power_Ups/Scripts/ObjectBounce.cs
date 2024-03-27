using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounce : MonoBehaviour
{
    public float bounceSpeed = 8;
    public float bounceAmplitude = 0.05f;
    public float rotationSpeed = 90;

    private float startHeight;
    private float timeOffset;

    public float DestroyDelay;

    // Start is called before the first frame update
    void Start()
    {
        DestroyDelay = Random.Range(15, 21);
        Destroy(gameObject, DestroyDelay);
        startHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
    }

    // Update is called once per frame
    void Update()
    {
        //animate
        float finalheight = startHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;

        //spin
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision");
            Destroy(gameObject);
        }
    }


}
