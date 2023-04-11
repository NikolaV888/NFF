using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    private Animator npcAnim;
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float minRange;
    // Start is called before the first frame update
    void Start()
    {
        npcAnim = GetComponent<Animator>();
        target = FindObjectOfType<Movement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) > minRange)
        {
            FollowPlayer();
        }
        else if (Vector3.Distance(target.position, transform.position) >= maxRange)
        {
            npcAnim.SetBool("withinRange", false);
        }
        else
        {
            npcAnim.SetBool("withinRange", false);
        }
    }
    public void FollowPlayer()
    {
        npcAnim.SetBool("withinRange", true);
        npcAnim.SetFloat("Horizontal", (target.position.x - transform.position.x));
        npcAnim.SetFloat("Vertical", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
