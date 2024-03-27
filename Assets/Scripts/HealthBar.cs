using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    [SerializeField] private HealthSystem healthSystem;

    public float chipSpeed = 4f;



    // Start is called before the first frame update
    void Start()
    {
       
    }

    void Update()
    {
        UpdateHealthGUI();
    }

    public void HitButton()
    {
        healthSystem.Damage(10);
    }

    public void HealButton()
    {
        healthSystem.Heal(10);
    }

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {

    }
    public void UpdateHealthGUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = healthSystem.GetHealthPercentage();

        if (fillB > hFraction) 
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            float percentComplete = 1 / chipSpeed;
            
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.blue;
            float percentComplete = 1 / chipSpeed;
            
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);

        }
    }
}
