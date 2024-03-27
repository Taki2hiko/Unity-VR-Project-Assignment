using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        ITakeDamage[] damageTakers = other.GetComponentsInChildren<ITakeDamage>();

        foreach (var taker in damageTakers)
        {
            taker.takeDamage(damage);
        }
    }
}
