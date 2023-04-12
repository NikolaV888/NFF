using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 100f;
    public float currentStamina;
    public float minStamina = 0f;
    public float staminaDecreaseRate = 1f;
    public float maxChakra = 100f;
    public float currentChakra;
    public void DecreaseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public float GetStamina()
    {
        return currentStamina;
    }


    // Additional attributes and variables
    public string villageAffiliation;
    public string ninjaRank;
    public string ninjaClass;
    public int kills;
    public int deaths;
    public float killDeathRatio;
    public int yen;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentChakra = maxChakra;

        // Initialize other attributes and variables
        villageAffiliation = "Leaf";
        ninjaRank = "Genin";
        ninjaClass = "C";
        kills = 0;
        deaths = 0;
        killDeathRatio = 0;
        yen = 0;
    }

    void Update()
    {
        Movement movement = GetComponent<Movement>();
        if (movement.isRunning && movement.movementDirection.magnitude > 0)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        else
        {
            float staminaRegenRate = 0.5f; // Adjust this value to control how fast stamina regenerates
            currentStamina += staminaRegenRate * Time.deltaTime;

            // Clamp stamina to the range [0, maxStamina]
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.label);

        int fontSize = (int)(Screen.height * 0.03f);
        guiStyle.fontSize = fontSize;
        guiStyle.alignment = TextAnchor.UpperLeft;

        float labelWidth = Screen.width * 0.2f;
        float labelHeight = Screen.height * 0.05f;
        Rect labelRect = new Rect(10, 10, labelWidth, labelHeight);

        GUI.Label(labelRect, $"Health: {currentHealth}/{maxHealth}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Stamina: {currentStamina}/{maxStamina}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Chakra: {currentChakra}/{maxChakra}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Village: {villageAffiliation}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Rank: {ninjaRank}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Class: {ninjaClass}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Kills: {kills}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Deaths: {deaths}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"K/D Ratio: {killDeathRatio}", guiStyle);
        labelRect.y += labelHeight;
        GUI.Label(labelRect, $"Yen: {yen}", guiStyle);
    }
}
