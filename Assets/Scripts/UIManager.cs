using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerAttributes staminaMan;
    private PlayerAttributes healthMan;
    public Slider staminaBar;
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        staminaMan = FindObjectOfType<PlayerAttributes>();
        healthMan = FindObjectOfType<PlayerAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (staminaMan != null && staminaBar != null)
        {
            staminaBar.maxValue = staminaMan.maxStamina;
            staminaBar.minValue = staminaMan.minStamina;
            staminaBar.value = staminaMan.currentStamina;
        }

        if (healthMan != null && healthBar != null)
        {
            healthBar.maxValue = healthMan.maxHealth;
            healthBar.minValue = healthMan.minHealth;
            healthBar.value = healthMan.currentHealth;
        }
    }
}
