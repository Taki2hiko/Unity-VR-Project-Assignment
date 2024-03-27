using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerController player;
    //public PhysicsProjectile physicsProjectile = new PhysicsProjectile();

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        


    }
}
