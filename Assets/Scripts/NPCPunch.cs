using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPunch : MonoBehaviour
{
    public float minPunchDamage = 6f;
    public float maxPunchDamage = 10f;
    public float punchRate = 2f;
    private float nextPunchTime = 0f;
    public float punchRange = 0.3f;

    private NPCControl npcControl;
    private Animator npcAnim;
    private NPCAttributes npcAttributes;

    void Start()
    {
        npcControl = GetComponent<NPCControl>();
        npcAnim = GetComponent<Animator>();
        npcAttributes = GetComponent<NPCAttributes>();
    }

    void Update()
    {
        Transform playerTransform = FindObjectOfType<Movement>().transform;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= punchRange)
        {
            if (Time.time >= nextPunchTime)
            {
                PunchPlayer(playerTransform.GetComponent<PlayerAttributes>());
                nextPunchTime = Time.time + 1f / punchRate;
            }
        }
        else
        {
            npcAnim.SetBool("IsPunching", false);
        }
    }

    void PunchPlayer(PlayerAttributes playerAttributes)
    {
        if (playerAttributes != null)
        {
            float punchDamage = Random.Range(minPunchDamage, maxPunchDamage);
            string currentPunchAnimation = Random.Range(0, 2) == 0 ? "LeftPunch" : "RightPunch";
            npcAnim.Play(currentPunchAnimation);
            npcAnim.SetBool("IsPunching", true);
            playerAttributes.TakeDamage(punchDamage);
            StartCoroutine(WaitForAnimation(npcAnim.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        npcAnim.SetBool("IsPunching", false);
    }
}
