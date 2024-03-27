using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public GameObject hitEffect;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(HitEffectPlay());
        ITakeDamage[] damageTakers = other.GetComponentsInChildren<ITakeDamage>();

        foreach (var taker in damageTakers)
        {
            taker.takeDamage(damage);
        }
        Destroy(gameObject);

    }

    IEnumerator HitEffectPlay()
    {
        Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
    }
}
