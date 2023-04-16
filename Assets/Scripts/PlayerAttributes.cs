using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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

    //new
    //  public SortingLayer waterSortingLayer;
    //  public LayerMask groundLayers;
    public TilemapManager tilemapManager;
    private Animator animator; // Define the animator variable here//

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

        // Initialize the animator variable
        animator = GetComponent<Animator>();
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
            float staminaRegenRate = 0.5f;
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            float chakraRegenRate = 2f;
            movement.canMove = false;
            currentChakra += chakraRegenRate * Time.deltaTime;
            currentChakra = Mathf.Clamp(currentChakra, 0, maxChakra);
            animator.SetBool("IsChargingChakra", true);
        }
        else
        {
            movement.canMove = true;
            animator.SetBool("IsChargingChakra", false);
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
