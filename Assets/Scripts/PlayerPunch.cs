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


    private Animator animator;
    private PlayerAttributes playerAttributes;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAttributes = GetComponent<PlayerAttributes>();
    }

    void Update()
    {
        if(Time.time >= nextPunchTime)
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

        //Detect enemimes in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rightPunchOrigin.position, punchRange, enemiesLayers);

        //damage enemy
        foreach(Collider2D enemy in hitEnemies)
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

    /*
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerPunch : MonoBehaviour
    {
        public float punchDamage = 10f;
        public float punchRange = 1f;
        public LayerMask npcLayer;
        public Transform leftPunchOrigin;
        public Transform rightPunchOrigin;
        public float staminaDecreaseRate = 1f;


        private Animator animator;
        private PlayerAttributes playerAttributes;

        void Start()
        {
            animator = GetComponent<Animator>();
            playerAttributes = GetComponent<PlayerAttributes>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Punch("LeftPunch");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Punch("RightPunch");
            }
        }

        void Punch(string punchType)
        {
            animator.SetTrigger(punchType);
            playerAttributes.DecreaseStamina(staminaDecreaseRate);

            StartCoroutine(DisableMovementDuringPunch(0.5f)); // Adjust the delay based on your punch animation duration

            Vector2 punchOrigin = punchType == "LeftPunch" ? leftPunchOrigin.position : rightPunchOrigin.position;
            Collider2D[] hitNPCs = Physics2D.OverlapCircleAll(punchOrigin, punchRange, npcLayer);

            foreach (Collider2D npc in hitNPCs)
            {
                npc.GetComponent<NPCAttributes>().TakeDamage(punchDamage);
            }
        }

        IEnumerator DisableMovementDuringPunch(float delay)
        {
            Movement movement = GetComponent<Movement>();
            movement.canMove = false;
            yield return new WaitForSeconds(delay);
            movement.canMove = true;
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
    */

}


