using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    //vars 
    public float punchDamage = 10f;
    public float staminaDecreaseRate = 1f;
    //other 
    public LayerMask enemiesLayers;
    public Transform leftPunchOrigin;
    public Transform rightPunchOrigin;
    //punchrates
    public float punchRate = 1f;
    float nextPunchTime = 0f;

    //reference to Movement
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
        // Update punch origin based on the player's direction
        UpdatePunchOrigin(leftPunchOrigin);
        UpdatePunchOrigin(rightPunchOrigin);

        if (Time.time >= nextPunchTime)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                LeftPunch();
                nextPunchTime = Time.time + 1f / punchRate;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RightPunch();
                nextPunchTime = Time.time + 1f / punchRate;
            }
        }
    }


    void LeftPunch()
    {
        //animation
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

        //stop movement when punching
        StartCoroutine(DisableMovementDuringPunch(0.3f)); // Adjust the delay based on your punch animation duration
    }

    void RightPunch()
    {
        //animation
        animator.SetTrigger("RightPunch");
        playerAttributes.DecreaseStamina(staminaDecreaseRate);

        // Update punch origin based on the player's direction
        UpdatePunchOrigin(rightPunchOrigin);

        // Detect enemies in the box for the right punch
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(rightPunchOrigin.position, rightPunchOrigin.localScale, 0, enemiesLayers);

        // Damage enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<NPCAttributes>().TakeDamage(punchDamage);
            Debug.Log("NPC hit" + enemy.name);
        }

        //stop movement when punching
        StartCoroutine(DisableMovementDuringPunch(0.3f)); // Adjust the delay based on your punch animation duration
    }

    //Proc for the movement stop
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
            punchOrigin.localPosition = new Vector3(-0.15f, -0.5f, 0);
        }
        else if (playerMovement.direction == Vector2.right)
        {
            punchOrigin.localPosition = new Vector3(0.12f, -0.5f, 0);
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