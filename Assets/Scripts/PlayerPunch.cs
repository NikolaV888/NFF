using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    // Variables
    public float punchDamage = 10f;
    public float staminaDecreaseRate = 1f;
    // Other
    public LayerMask enemiesLayers;
    public Transform leftPunchOrigin;
    public Transform rightPunchOrigin;
    // Punch rates
    public float punchRate = 1f;
    float nextPunchTime = 0f;
    // Charge punch
    private float chargeTime = 0f;
    public float minChargeTime = 2f;

    // Reference to Movement
    private Movement playerMovement;

    private Animator animator;
    private PlayerAttributes playerAttributes;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerMovement = GetComponent<Movement>();
    }

    void Update()
    {
        if (Time.time >= nextPunchTime)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                chargeTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                if (Time.time >= nextPunchTime)
                {
                    LeftPunch();
                    nextPunchTime = Time.time + 1f / punchRate;
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                chargeTime = Time.time;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                if (Time.time >= nextPunchTime)
                {
                    RightPunch();
                    nextPunchTime = Time.time + 1f / punchRate;
                }
            }
        }

        // Update punch origin based on the player's direction
        UpdatePunchOrigin(leftPunchOrigin);
        UpdatePunchOrigin(rightPunchOrigin);
    }

    void LeftPunch()
    {
        float elapsedTime = Time.time - chargeTime;
        bool isChargedPunch = elapsedTime >= minChargeTime;

        if (isChargedPunch)
        {
            punchDamage = Random.Range(10, 13);
        }
        else
        {
            punchDamage = Random.Range(6, 10);
        }

        // Animation
        animator.SetTrigger("LeftPunch");
        playerAttributes.DecreaseStamina(staminaDecreaseRate);

        // Update punch origin based on the player's direction
        UpdatePunchOrigin(leftPunchOrigin);

        // Detect enemies in the box for the left punch
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(leftPunchOrigin.position, leftPunchOrigin.localScale, 0, enemiesLayers);

        // Damage enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<NPCAttributes>().TakeDamage(punchDamage);
            Debug.Log("NPC hit" + enemy.name);
        }

        // Stop movement when punching
        StartCoroutine(DisableMovementDuringPunch(0.3f)); // Adjust the delay based on your punch animation duration
    }

    void RightPunch()
    {
        float elapsedTime = Time.time - chargeTime;
        bool isChargedPunch = elapsedTime >= minChargeTime;

        if (isChargedPunch)
        {
            punchDamage = Random.Range(10, 13);
        }
        else
        {
            punchDamage = Random.Range(6, 10);
        }

        // Animation
        animator.SetTrigger("RightPunch");
        playerAttributes.DecreaseStamina(staminaDecreaseRate);

        // Update punch origin based
        UpdatePunchOrigin(rightPunchOrigin);


        // Detect enemies in the box for the right punch
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(rightPunchOrigin.position, rightPunchOrigin.localScale, 0, enemiesLayers);

        // Damage enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<NPCAttributes>().TakeDamage(punchDamage);
            Debug.Log("NPC hit" + enemy.name);
        }

        // Stop movement when punching
        StartCoroutine(DisableMovementDuringPunch(0.3f)); // Adjust the delay based on your punch animation duration
    }

    // Coroutine for the movement stop
    IEnumerator DisableMovementDuringPunch(float delay)
    {
        Movement movement = GetComponent<Movement>();
        movement.canMove = false;
        yield return new WaitForSeconds(delay);
        movement.canMove = true;
    }

    void UpdatePunchOrigin(Transform punchOrigin)
    {
        punchOrigin.localRotation = Quaternion.Euler(0, 0, 0);
        punchOrigin.localScale = new Vector3(0.44f, 0.85f, 0);

        if (playerMovement.direction == Vector2.up)
        {
            punchOrigin.localPosition = new Vector3(0, 0.13f, 0);
        }
        else if (playerMovement.direction == Vector2.down)
        {
            punchOrigin.localPosition = new Vector3(0, -0.18f, 0);
        }
        else if (playerMovement.direction == Vector2.left)
        {
            punchOrigin.localPosition = new Vector3(-0.15f, 0, 0);
        }
        else if (playerMovement.direction == Vector2.right)
        {
            punchOrigin.localPosition = new Vector3(0.12f, 0, 0);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (leftPunchOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(leftPunchOrigin.position, leftPunchOrigin.localScale);
        }

        if (rightPunchOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rightPunchOrigin.position, rightPunchOrigin.localScale);
        }
    }
}