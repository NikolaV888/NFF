using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAttributes : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float minHealth = 0f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float minStamina = 0f;
    public float staminaDecreaseRate = 1f;
    public float maxChakra = 100f;
    public float currentChakra;

    //chakra Variables
    public bool isChargingChakra { get; private set; }
    private bool wasChargingChakra = false;
    private Vector2 chargingDirection;


    // Damage message variables
    public float damageMessageDuration = 1f;
    public float damageMessageSpeed = 2f;
    private List<DamageMessage> damageMessages = new List<DamageMessage>();


    public TilemapManager tilemapManager;
    private Animator animator;

    public void DecreaseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public float GetStamina()
    {
        return currentStamina;
    }

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

        villageAffiliation = "Leaf";
        ninjaRank = "Genin";
        ninjaClass = "C";
        kills = 0;
        deaths = 0;
        killDeathRatio = 0;
        yen = 0;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement movement = GetComponent<Movement>();

        for (int i = damageMessages.Count - 1; i >= 0; i--)
        {
            DamageMessage message = damageMessages[i];
            message.UpdateMessage(Time.deltaTime, damageMessageSpeed);

            if (message.IsFinished())
            {
                damageMessages.RemoveAt(i);
            }
        }

        if (movement.isRunning && movement.movementDirection.magnitude > 0)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            float staminaRegenRate = 0.5f;
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            float chakraRegenRate = 2f;
            isChargingChakra = true;
            currentChakra += chakraRegenRate * Time.deltaTime;
            currentChakra = Mathf.Clamp(currentChakra, 0, maxChakra);
            animator.SetBool("IsChargingChakra", true);

            if (!wasChargingChakra)
            {
                chargingDirection = movement.direction;
                wasChargingChakra = true;
            }
        }
        else
        {
            isChargingChakra = false;
            wasChargingChakra = false;
            animator.SetBool("IsChargingChakra", false);
        }
        /*
        if (!isChargingChakra)
        {
            animator.SetFloat("Horizontal", movement.direction.x);
            animator.SetFloat("Vertical", movement.direction.y);
        }
        */

        if (!tilemapManager.IsPositionOnGrass(transform.position) && tilemapManager.IsPositionOnWater(transform.position) && movement.movementDirection.magnitude > 0)
        {
            float chakraDecreaseRate = 2f;
            currentChakra -= chakraDecreaseRate * Time.deltaTime;
            currentChakra = Mathf.Clamp(currentChakra, 0, maxChakra);
        }



        bool IsPositionOnWater = tilemapManager.IsPositionOnWater(transform.position);
        if (!tilemapManager.IsPositionOnGrass(transform.position) && tilemapManager.IsPositionOnWater(transform.position) && movement.movementDirection.magnitude > 0)
        {
            float chakraDecreaseRate = 2f;
            currentChakra -= chakraDecreaseRate * Time.deltaTime;
            currentChakra = Mathf.Clamp(currentChakra, 0, maxChakra);

            // Add debug message for chakra decrease
            //Debug.Log("Decreasing chakra: " + currentChakra);
        }
    }

    /*
    private bool IsOnWater()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 boxSize = new Vector2(detectionBoxSize, detectionBoxSize);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerPosition, boxSize, 0, tilemapLayers);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Tilemap tilemap = hitCollider.GetComponent<Tilemap>();
            if (tilemap != null && tilemap.gameObject.tag == "Water")
            {
                return true;
            }
        }
        return false;
    }
    */
    private bool IsPlayerOnWater()
    {
        if (tilemapManager == null)
        {
            return false;
        }
        return tilemapManager.IsPositionOnWater(transform.position);
    }

    public void TakeDamage(float damage)
    {
        // Apply damage to stamina first
        currentStamina -= damage;

        // Convert damage to a whole number
        int damageInt = Mathf.RoundToInt(damage);

        // Create a new DamageMessage instance and add it to the list
        damageMessages.Add(new DamageMessage($"-{damageInt}", damageMessageDuration));

        if (currentStamina < 0)
        {
            // Apply remaining damage to health
            currentHealth += currentStamina;
            currentStamina = 0;
        }

        // Clamp health and stamina values within their respective min and max values
        currentHealth = Mathf.Clamp(currentHealth, minHealth, maxHealth);
        currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);

        // Check for player death
        if (currentHealth <= minHealth && currentStamina <= minStamina)
        {
            Die();
        }
    }


    private void Die()
    {
        // Handle player death here (e.g., trigger an animation, restart the level, etc.)
        Debug.Log("Player has died");
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

        //MENSAJE DE DMG
        float yOffset = 100f; // Adjust this value to position the message above the enemy's head
        GUIStyle style = new GUIStyle();
        style.fontSize = 32;
        style.normal.textColor = Color.red;

        foreach (DamageMessage message in damageMessages)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + message.offset);
            screenPosition.y += yOffset;
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 100, 20), message.text, style);
        }
    }

    public class DamageMessage
    {
        public string text;
        public float timer;
        public Vector3 offset;

        public DamageMessage(string text, float duration)
        {
            this.text = text;
            this.timer = duration;
            this.offset = Vector3.zero;
        }

        public void UpdateMessage(float deltaTime, float speed)
        {
            timer -= deltaTime;
            offset.y += speed * deltaTime;
        }

        public bool IsFinished()
        {
            return timer <= 0;
        }
    }
}