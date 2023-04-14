using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed { get; private set; }
    //vars
    public float walkSpeed = 3f; // The speed at which the player walks
    public float runSpeed = 6f; // The speed at which the player runs

    //other
    public Rigidbody2D rb; // The Rigidbody2D component attached to the player
    public Vector2 movementDirection; // The current movement direction of the player

    public Animator animator;
    public LayerMask treesLayer;

    //vars
    public bool canMove = true;
    public bool isRunning = false;
    public Vector2 direction { get; private set; }

    //public bool canRotate = true;
    //   public bool canRotate = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        animator = GetComponent<Animator>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        treesLayer = LayerMask.GetMask("Trees");
    }


    void Update()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            movementDirection = new Vector2(horizontalInput, verticalInput).normalized;
            movementDirection.Normalize();

            // Update the direction variable
            direction = movementDirection;

            if (movementDirection.magnitude > 0)
            {
                animator.SetFloat("Horizontal", movementDirection.x);
                animator.SetFloat("Vertical", movementDirection.y);
                animator.SetFloat("Speed", movementDirection.magnitude * (isRunning ? runSpeed : walkSpeed));

                if (isRunning)
                {
                    animator.Play("Run");
                }
                else
                {
                    animator.Play("Movement");
                }
            }
            else
            {
                animator.SetFloat("Speed", 0f);
                animator.Play("Idle");
            }

            if (horizontalInput == 0 && verticalInput == 0)
            {
                movementDirection = Vector2.zero;
            }

            PlayerAttributes playerAttributes = GetComponent<PlayerAttributes>();

            if (playerAttributes.GetStamina() < 20f)
            {
                isRunning = false;
                walkSpeed = 2f;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (playerAttributes.GetStamina() >= 20f)
                {
                    isRunning = !isRunning;
                    if (isRunning)
                    {
                        walkSpeed = runSpeed;
                    }
                    else
                    {
                        walkSpeed = 2f;
                    }
                }
            }
        } // <- Add this closing bracket
    }



    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.2f, movementDirection, 0.1f, treesLayer);

        if (hit)
        {
            Vector2 collisionNormal = hit.normal;
            Vector2 movementDirectionAlongTree = movementDirection - collisionNormal * Vector2.Dot(movementDirection, collisionNormal);
            rb.MovePosition(rb.position + movementDirectionAlongTree * (isRunning ? runSpeed : walkSpeed) * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + movementDirection * (isRunning ? runSpeed : walkSpeed) * Time.fixedDeltaTime);
        }
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

        if (movementDirection.magnitude > 0)
        {
            GUI.Label(labelRect, isRunning ? "Running" : "Walking", guiStyle);
        }
        else
        {
            GUI.Label(labelRect, "Standing Still", guiStyle);
        }
    }
}