using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip noAmmo;

    public Magazine mag;
    public TextMeshProUGUI ammoText;
    public XRBaseInteractor socketInteractor;
    private bool hasSlide = true;
    public static float damage;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    void Update()
    {
        if (!mag)
        {
            ammoText.SetText("No Mag!");
        }

    }


    public void AddMagazine(XRBaseInteractable interactable)
    {
        hasSlide = false;
        mag = interactable.GetComponent<Magazine>();
        audioSource.PlayOneShot(reload);
        ammoText.SetText("Slide!");
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        mag = null;
        audioSource.PlayOneShot(reload);
    }

    public void Slide()
    {
        hasSlide = true;
        audioSource.PlayOneShot(reload);
        ammoText.SetText("Ammo\n" + mag.bullets + "/15");
    }
    public void PullTrigger()
    {
        if (mag && mag.bullets > 0 && hasSlide) 
        {
            gunAnimator.SetTrigger("Fire");
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

    //This function creates the bullet behavior
    public void Shoot()
    {
        audioSource.PlayOneShot(shoot);
        mag.bullets--;
        ammoText.SetText("Ammo\n" + mag.bullets + "/15");

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject theBullet;
        theBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        theBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        //Destroy bullet after X seconds
        Destroy(theBullet, destroyTimer);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
