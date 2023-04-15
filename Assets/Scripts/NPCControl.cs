using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private Animator npcAnim;
    private Vector3 lastDirection;//new
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float minRange;
    [SerializeField]
    private float avoidDistance = 1f;
    [SerializeField]
    private LayerMask obstacleLayers;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;

    private NPCAttributes npcAttributes;

    // Start is called before the first frame update
    void Start()
    {
        npcAnim = GetComponent<Animator>();
        target = FindObjectOfType<Movement>().transform;
        npcAttributes = GetComponent<NPCAttributes>(); // Add this line
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) > minRange)
        {
            FollowPlayer();
        }
        else
        {
            npcAnim.SetBool("withinRange", false);
            npcAnim.SetFloat("Horizontal", lastDirection.x);
            npcAnim.SetFloat("Vertical", lastDirection.y);

            // Set the IsRunning parameter to false when not following the player
            npcAnim.SetBool("IsRunning", false);
        }
    }

    public void FollowPlayer()
    {
        bool isRunning = npcAttributes.currentStamina > 20;

        if (isRunning)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        npcAnim.SetBool("withinRange", true);

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 avoidObstacleDirection = AvoidObstacle(directionToTarget);

        npcAnim.SetFloat("Horizontal", avoidObstacleDirection.x);
        npcAnim.SetFloat("Vertical", avoidObstacleDirection.y);

        // Set the IsRunning parameter in the Animator based on the isRunning variable
        npcAnim.SetBool("IsRunning", isRunning);

        lastDirection = avoidObstacleDirection; // Add this line

        // Move the NPC using its Transform
        transform.position += avoidObstacleDirection * speed * Time.deltaTime;
    }




    private Vector3 AvoidObstacle(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, avoidDistance, obstacleLayers);

        if (hit.collider != null)
        {
            Vector3 avoidDirection = (hit.point - (Vector2)transform.position).normalized;
            avoidDirection = Quaternion.Euler(0, 0, 90) * avoidDirection; // Turn 90 degrees to avoid the obstacle
            return avoidDirection;
        }

        return direction;
    }
}
