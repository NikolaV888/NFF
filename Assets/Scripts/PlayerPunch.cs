using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    //vars 
    public float punchDamage = 10f;
    public float punchRange = 1f;
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

        //Update punch collider based on the player's direction
        UpdatePunchCollider(leftPunchOrigin);

        //Detect enemimes in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(leftPunchOrigin.position, punchRange, enemiesLayers);

        //damage enemy
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

        //Update punch collider based on the player's direction
        UpdatePunchCollider(rightPunchOrigin);

        //Detect enemimes in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rightPunchOrigin.position, punchRange, enemiesLayers);

        //damage enemy
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

    void UpdatePunchCollider(Transform punchOrigin)
    {
        BoxCollider2D punchCollider = punchOrigin.GetComponent<BoxCollider2D>();

        if (playerMovement.direction == Vector2.up)
        {
            punchCollider.offset = new Vector2(0, 0.5f);
            punchCollider.size = new Vector2(1f, 1f);
        }
        else if (playerMovement.direction == Vector2.down)
        {
            punchCollider.offset = new Vector2(0, -0.5f);
            punchCollider.size = new Vector2(1f, 1f);
        }
        else if (playerMovement.direction == Vector2.left)
        {
            punchCollider.offset = new Vector2(-0.5f, 0);
            punchCollider.size = new Vector2(1f, 1f);
        }
        else if (playerMovement.direction == Vector2.right)
        {
            punchCollider.offset = new Vector2(0.5f, 0);
            punchCollider.size = new Vector2(1f, 1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (leftPunchOrigin != null)
        {
            Gizmos.DrawWireSphere(leftPunchOrigin.position, punchRange);
        }

        if (rightPunchOrigin != null)
        {
            Gizmos.DrawWireSphere(rightPunchOrigin.position, punchRange);
        }
    }
}