using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerAttributes staminaMan;
    public Slider staminaBar;
    // Start is called before the first frame update
    void Start()
    {
        staminaMan = FindObjectOfType<PlayerAttributes>();
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
    }
}
