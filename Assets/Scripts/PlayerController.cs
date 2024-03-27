using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour, ITakeDamage
{
    // Reference to the Continuous Move Provider component
    public ContinuousMoveProviderBase continuousMoveProvider;
    public HealthSystem healthSystem;
    public EnemyBoss enemyBoss;
    public GameObject battleFieldPos;
    public GameObject lobbyPos;
    public GameObject[] directController;
    public GameObject[] rayController;

    public TextMeshProUGUI lobbyText;
    public GameObject[] buttons;
    public Bullet[] bullets;
    public BulletData bulletData;

    public bool isHit = false;
    public bool onBuff = false;
    public bool IsStart = false;

    [SerializeField]
    private bool IsDead = false;
    [SerializeField]
    private float damageBuffDuration;
    [SerializeField]
    private float speedBuffDuration;
    // Start is called before the first frame update
    void Start()
    {
        continuousMoveProvider = FindObjectOfType<ContinuousMoveProviderBase>();
        healthSystem = GetComponent<HealthSystem>();
        enemyBoss = FindObjectOfType<EnemyBoss>();
        healthSystem.SetMaxHealth(100);
        healthSystem.SetHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        bullets = FindObjectsOfType<Bullet>();

        if (!IsStart || IsDead)
        {
            SetController();
        }

        if (healthSystem.GetHealth() <= 0 && !IsDead)
        {
            IsDead = true;
            lobbyText.SetText("You Died\nDo you want to reset?");
            
            TeleportPlayer();
        }

        if (speedBuffDuration > 0)
        {
            speedBuffDuration = Mathf.Clamp(speedBuffDuration, 0f, 10f);
            speedBuffDuration -= Time.deltaTime;
        }
        else if (speedBuffDuration < 0f)
        {
            continuousMoveProvider.moveSpeed = 6;
        }

        if (damageBuffDuration > 0)
        {
            if (!onBuff)
            {
                foreach (Bullet bullet in bullets)
                {
                    bullet.damage *= 2;
                }
            }

            onBuff = true;
            damageBuffDuration = Mathf.Clamp(damageBuffDuration, 0f, 10f);
            damageBuffDuration -= Time.deltaTime;
        }
        else if (damageBuffDuration < 0 && onBuff)
        {
            PhysicsProjectile.dmg /= 2;
            SimpleShoot.damage /= 2;
            
            onBuff = false;
        }
    }

    public void TeleportPlayer()
    {
        IsStart = true;
        if (IsDead)
        {
            gameObject.transform.position = lobbyPos.transform.position;
            IsStart = false;
        }
        else if (!IsDead)
        {
            foreach (GameObject controller in rayController)
            {
                controller.SetActive(false);
            }

            foreach (GameObject controller in directController)
            {
                controller.SetActive(true);
            }

            gameObject.transform.position = battleFieldPos.transform.position;
        }
    }

    public void takeDamage(float enemyDamage)
    {
        healthSystem.Damage(enemyDamage);
        Debug.Log(healthSystem.GetHealth());
    }

    public void SetController()
    {
        foreach (GameObject controller in rayController)
        {
            controller.SetActive(true);
        }

        foreach (GameObject controller in directController)
        {
            controller.SetActive(false);
        }
    }

    public void SetButton()
    {
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Heal")
        {
            healthSystem.Heal(10);
            Debug.Log(healthSystem.GetHealth());
        }

        if (other.gameObject.tag == "DamageUp")
        {
            PhysicsProjectile.dmg *= 2;
            SimpleShoot.damage *= 2;
            Debug.Log("Damage Up");
            damageBuffDuration = 10f;
        }

        if (other.gameObject.tag == "SpeedUp")
        {
            Debug.Log("Speed Up");
            continuousMoveProvider.moveSpeed = 12;
            speedBuffDuration = 10f;
        }
    }
}
