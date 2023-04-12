using System.Collections;
using UnityEngine;

public class NPCRespawner : MonoBehaviour
{
    public NPCAttributes npcAttributes;
    public float respawnTime = 20f;
    public float respawnRange = 5f;

    private void Start()
    {
        npcAttributes.onDeath += StartRespawn;
    }

    private void StartRespawn()
    {
        StartCoroutine(RespawnAfterDelay(respawnTime));
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        npcAttributes.gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);

        Vector3 newPosition = new Vector3(
            npcAttributes.transform.position.x + Random.Range(-respawnRange, respawnRange),
            npcAttributes.transform.position.y + Random.Range(-respawnRange, respawnRange),
            npcAttributes.transform.position.z
        );

        npcAttributes.Respawn(newPosition);
        npcAttributes.gameObject.SetActive(true);
    }
}
