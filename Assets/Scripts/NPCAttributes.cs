using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttributes : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 100f;
    public float currentStamina;
    public float minStamina = 0f;
    public float staminaDecreaseRate = 1f;
    public float maxChakra = 100f;
    public float currentChakra;
    public Rigidbody2D rb;
    public Collider2D col;

    // Damage message variables
    public float damageMessageDuration = 1f;
    public float damageMessageSpeed = 2f;
    private List<DamageMessage> damageMessages = new List<DamageMessage>();

    // Respawn and corpse variables
    public float respawnTime = 20f;
    public GameObject corpsePrefab;
    public float corpseDuration = 60f;

    // Respawn position variables
    public float respawnRange = 5f;

    //death
    public delegate void OnDeath();
    public event OnDeath onDeath;

    //new for running
    public float staminaRecoveryRate = 5f;


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        currentStamina = maxStamina;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to a minimum of 0
        damageMessages.Add(new DamageMessage($"-{damage}", damageMessageDuration));

        //play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Disable movement and collisions
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        col.enabled = false;

        // Implement the NPC's death behavior (e.g., play a death animation)
        // ...

        // Spawn the corpse
        GameObject corpse = Instantiate(corpsePrefab, transform.position, transform.rotation);
        Destroy(corpse, corpseDuration);

        // Invoke the onDeath event
        onDeath?.Invoke();
    }



    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Show the NPC GameObject again
        gameObject.SetActive(true);

        // Reset health and enable movement
        currentHealth = maxHealth;
        rb.isKinematic = false;

        // Respawn at a random nearby location
        Vector3 newPosition = new Vector3(
            transform.position.x + Random.Range(-respawnRange, respawnRange),
            transform.position.y + Random.Range(-respawnRange, respawnRange),
            transform.position.z
        );
        transform.position = newPosition;

        // Enable the collider after updating the position
        col.enabled = true;
    }


    public void Respawn(Vector3 newPosition)
    {
        // Reset health and enable movement
        currentHealth = maxHealth;
        rb.isKinematic = false;

        // Respawn at the new position
        transform.position = newPosition;

        // Enable the collider after updating the position
        col.enabled = true;
    }


    void Update()
    {
        for (int i = damageMessages.Count - 1; i >= 0; i--)
        {
            DamageMessage message = damageMessages[i];
            message.UpdateMessage(Time.deltaTime, damageMessageSpeed);

            if (message.IsFinished())
            {
                damageMessages.RemoveAt(i);
            }
        }
        currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }
    }

    void OnGUI()
    {
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