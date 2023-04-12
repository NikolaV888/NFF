using System.Collections.Generic;
using UnityEngine;

public class NPCAttributes : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    // Damage message variables
    public float damageMessageDuration = 1f;
    public float damageMessageSpeed = 2f;
    private List<DamageMessage> damageMessages = new List<DamageMessage>();

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        damageMessages.Add(new DamageMessage($"-{damage}", damageMessageDuration));

        //play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Implement the NPC's death behavior (e.g., play a death animation and disable the GameObject)
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
