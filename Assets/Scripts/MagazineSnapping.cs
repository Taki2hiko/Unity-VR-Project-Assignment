using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSnapping : MonoBehaviour
{
    [SerializeField] private GameObject snappingArea;
    [SerializeField] private Vector3 snappingOffset;
    [SerializeField] private GameObject Magazines;
    // Start is called before the first frame update
    void Start()
    {
        snappingOffset = snappingArea.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Magazines"))
        {
            Debug.Log("Triggered");
            other.transform.position = snappingOffset;
        }
    }
}
