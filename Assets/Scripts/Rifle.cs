using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Rifle : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Transform barrelLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip noAmmo;

    public float damage;
    public float timeBetweenShooting;
    public float spread;
    public float range;
    public float timeBetweenShots;

    private bool shooting;
    private bool readyToShoot = true;

    RaycastHit hit;

    Magazine mag;
    public TextMeshProUGUI ammoText;
    public XRBaseInteractor socketInteractor;
    public XRBaseController hapticController;
    public Haptic hapticOnActivated;

    // Start is called before the first frame update
    void Start()
    {
        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);


    }

    // Update is called once per frame
    void Update()
    {
        if (!mag)
        {
            ammoText.SetText("No Mag");
        }
    }

    public void AddMagazine(XRBaseInteractable interactable)
    {
        mag = interactable.GetComponent<Magazine>();
        audioSource.PlayOneShot(reload);
        ammoText.SetText("Ammo\n" + mag.bullets + "/30");

    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        mag = null;
        audioSource.PlayOneShot(reload);
    }

    public void PullTrigger()
    {
        shooting = true;
        if (shooting) 
        {
            if (shooting && mag && mag.bullets > 0)
            {
                Shoot();
            }
            else if (shooting && mag && mag.bullets <= 0)
            {
                ammoText.SetText("No Ammo!");
                audioSource.PlayOneShot(noAmmo);
            }
            else if (shooting && !mag)
            {
                ammoText.SetText("No Mag");
                audioSource.PlayOneShot(noAmmo);
            }
        }
    }

    public void ReleaseTrigger()
    {
        shooting = false;
        CancelInvoke("Shoot");
    }

    public void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = barrelLocation.forward + new Vector3(x,y,0);

        ////Raycast
        //if (Physics.Raycast(barrelLocation.transform.position, direction, out hit, range))
        //{
        //    if (hit.collider.CompareTag("Enemy"))
        //        hit.collider.GetComponent<HealthSystem>().Damage(damage);
        //}

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, 1f);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject theBullet;
        theBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        theBullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        //Destroy bullet after X seconds
        Destroy(theBullet, 2f);

        audioSource.PlayOneShot(shoot);

        //XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        //interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
        mag.bullets--;

        XRBaseController hapticController = GetHapticController();
        if (hapticController != null)
        {
            hapticController.SendHapticImpulse(1, 0.1f); // Adjust the parameters as needed
        }

        ammoText.SetText("Ammo\n" + mag.bullets + "/30");

        Invoke("ResetShot", timeBetweenShooting);

        if (mag.bullets > 0)
        Invoke("Shoot", timeBetweenShots);

        
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private XRBaseController GetHapticController()
    {
        // Check if the rifle is interactable and has an interactor
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null && interactable.selectingInteractor is XRBaseControllerInteractor controllerInteractor)
        {
            return controllerInteractor.xrController;
        }

        // If not, you may need to find the controller in another way based on your setup
        // ...

        return null;
    }
}
