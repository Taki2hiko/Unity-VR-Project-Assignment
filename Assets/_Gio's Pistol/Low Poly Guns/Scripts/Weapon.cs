using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(XRGrabInteractable))]

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float shootingForce;
    [SerializeField] public Transform bulletSpawn;
    [SerializeField] private float recoilForce;
    [SerializeField] private float damage;

    [SerializeField] private Projectile bulletPrefab;

    [SerializeField] private Transform gunTransform;

    public GameObject Gun;
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    private XRGrabInteractable interactableWeapon;
    public float fireRate = 1f; //1 sec cooldown
    private float timeToFire;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip noAmmo;

    public TextMeshProUGUI ammoText;
    public Magazine mag;
    public XRBaseInteractor socketInteractor;

    void Start()
    {
        interactableWeapon = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    private void Update()
    {
        if (!mag)
        {
            ammoText.SetText("No Mag");
        }
    }


    //public float rayLength = 10f;
    //private void ApplyRecoil(Collider targetCollider)
    //{
    //    //rb.AddRelativeForce(Vector3.back*recoilForce, ForceMode.Force);
    //    //Debug.Log("Applying recoil force: " + (Vector3.back * recoilForce));

    //    Rigidbody targetRigidbody = targetCollider.GetComponent<Rigidbody>();

    //    if (targetRigidbody != null)
    //    {
    //        // Apply recoil force to the object hit by the ray
    //        targetRigidbody.AddForce(-transform.forward * recoilForce, ForceMode.Impulse);
    //    }
    //}

    public void AddMagazine(XRBaseInteractable interactable)
    {
        mag = interactable.GetComponent<Magazine>();
        audioSource.PlayOneShot(reload);
        ammoText.SetText("Ammo\n" + mag.bullets + "/6");
    }
    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        mag = null;
        audioSource.PlayOneShot(reload);
    }

    public float GetShootingForce()
    {
        return shootingForce;
    }

    public float GetDamage()
    {
        return damage;
    }


    public void StartShooting()
    {
        //base.StartShooting(interactor);
        if (Time.time >= timeToFire && mag && mag.bullets > 0) 
        {
            timeToFire = Time.time + 0.4f / fireRate;
            Shoot();
            StartCoroutine(StartRecoil());
        }
        else if (mag && mag.bullets <= 0) 
        {
            ammoText.SetText("No Ammo!");
            audioSource.PlayOneShot(noAmmo);
        }
        else
        {
            ammoText.SetText("No Mag");
            audioSource.PlayOneShot(noAmmo);
        }


    }

    protected virtual void Shoot()
    {
        //base.Shoot();
        //ApplyRecoil();
        audioSource.PlayOneShot(shoot);
        mag.bullets--;
        ammoText.SetText("Ammo\n" + mag.bullets + "/6");

        Projectile projectileInstance = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        projectileInstance.Init(this);
        projectileInstance.Launch();
        animator.SetTrigger("Recoil");
        

    }

    IEnumerator StartRecoil()
    {
        //Gun.GetComponent<Animator>().Play("Recoil");
        yield return new WaitForSeconds(0.7f);
        //Gun.GetComponent<Animator>().Play("New State");
    }

}
