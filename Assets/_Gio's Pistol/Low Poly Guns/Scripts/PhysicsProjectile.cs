using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : Projectile
{
    [SerializeField] private float lifetime;
    public GameObject hitEffect;
    private Rigidbody rb;
    public static float dmg = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        Destroy(gameObject, lifetime);
    }

    public override void Launch()
    {
        base.Launch();
        rb.AddForce(weapon.bulletSpawn.forward * weapon.GetShootingForce());
        //rb.AddRelativeForce(Vector3.forward * weapon.GetShootingForce(), ForceMode.Impulse);
    }

    IEnumerator HitEffectPlay()
    {
        Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(HitEffectPlay());
        ITakeDamage[] damageTakers = other.GetComponentsInChildren<ITakeDamage>();

        foreach (var taker in damageTakers)
        {
            taker.takeDamage(dmg);
        }
        Destroy(gameObject);

    }

}
