using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private Animator npcAnim;
    private Vector3 lastDirection;
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    public float minRange;
    [SerializeField]
    private float avoidDistance = 1f;
    [SerializeField]
    private LayerMask obstacleLayers;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;

    public bool isFollowingPlayer()
    {
        return npcAnim.GetBool("withinRange");
    }

    private NPCAttributes npcAttributes;

    void Start()
    {
        npcAnim = GetComponent<Animator>();
        target = FindObjectOfType<Movement>().transform;
        npcAttributes = GetComponent<NPCAttributes>();
    }

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

        npcAnim.SetBool("IsRunning", isRunning);

        lastDirection = avoidObstacleDirection;

        transform.position += avoidObstacleDirection * speed * Time.deltaTime;
    }

    public Vector3 GetDirection()
    {
        return lastDirection;
    }


    private Vector3 AvoidObstacle(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, avoidDistance, obstacleLayers);

        if (hit.collider != null)
        {
            Vector3 avoidDirection = (hit.point - (Vector2)transform.position).normalized;
            avoidDirection = Quaternion.Euler(0, 0, 90) * avoidDirection;
            return avoidDirection;
        }

        return direction;
    }
}
